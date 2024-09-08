using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using telegramShpigonGameBot;

internal class Program
{
    // Bot API token (Set the environment variable with the name SHPIGON_TOKEN for your Bot API token)
    static string tokenShpigon = Environment.GetEnvironmentVariable("SHPIGON_TOKEN") ?? "null";

    // Delay in ms for requests from bot
    static int requestDelay = 500;

    // Class for dictionaries in English (to change the language, just use another class and constructor)
    static BotMessagesEnglish botMessages = new BotMessagesEnglish();

    // Class for topics in English (to change the language, just use another class and constructor)
    static TopicsEnglish topics = new TopicsEnglish();
    
    // Dictionaries for storing information about game from chats (long data type - Chat.Id)
    private static Dictionary<long, List<List<string>>> usersIds = new Dictionary<long, List<List<string>>>();
    private static Dictionary<long, long> spyIds = new Dictionary<long, long>();
    private static Dictionary<long, bool> isStarted = new Dictionary<long, bool>();
    private static Dictionary<long, bool> isStartedAndReady = new Dictionary<long, bool>();
    private static Dictionary<long, int> roundNow = new Dictionary<long, int>();
    private static Dictionary<long, Dictionary<string, int>> votes = new Dictionary<long, Dictionary<string, int>>();
    private static Dictionary<long, Dictionary<long, bool>> isVoted = new Dictionary<long, Dictionary<long, bool>>();
    private static Dictionary<long, bool> isSpyWon = new Dictionary<long, bool>();
    
    // Using the async task for Main() to use infinite delay  
    static async Task Main()
    {
        Host host = new Host(tokenShpigon);
        host.HandleMessages += HandleMessages;
        host.HandleCallbackQuery += HandleCallbackQuery;
        host.Start();
        await Task.Delay(Timeout.Infinite);
    }

    // Method for creating voting inline keyboard markup with the usernames of players  
    private static InlineKeyboardMarkup CreateInlineKeyboard(List<string> userNames)
    {
        var inlineKeyboard = new List<List<InlineKeyboardButton>>();

        foreach (var userName in userNames)
        {
            var row = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData($"@{userName}", $"user_{userName}")
            };
            inlineKeyboard.Add(row);
        }

        return new InlineKeyboardMarkup(inlineKeyboard);
    }

    // Method for creating output string with the results of voting
    private static string CreateVotesList(Dictionary<string, int> votes)
    {
        string list = "";

        foreach (string user in votes.Keys)
            list += $"\n@{user} - {votes[user]}";

        return list;
    }

    // Timer method
    private static void Timer(int minutes, Message message, long chatId, ITelegramBotClient client)
    {
        // Stopwatch for handling execution delays
        Stopwatch stopWatch = new Stopwatch();

        for (int m = minutes - 1; m >= 0; m --)
        {
            for (int s = 60; s > 0; s -= 5)
            {
                stopWatch.Start();

                if (s == 60)
                    client.EditMessageTextAsync(chatId, message.MessageId, botMessages.textMessages["countdownStarted"] + $"\n\n0{m + 1}:00");

                else if (s < 10)
                    client.EditMessageTextAsync(chatId, message.MessageId, botMessages.textMessages["countdownStarted"] + $"\n\n0{m}:0{s}");

                else     
                    client.EditMessageTextAsync(chatId, message.MessageId, botMessages.textMessages["countdownStarted"] + $"\n\n0{m}:{s}");

                stopWatch.Stop();
                int delayTime = Convert.ToInt32(stopWatch.ElapsedMilliseconds);
                stopWatch.Reset();

                Thread.Sleep((5 * 1000) - delayTime);
                // Checking if the game was stoped during the sleep 
                if (!isStartedAndReady.ContainsKey(chatId)) {return;}
            }
        }

        client.EditMessageTextAsync(chatId, message.MessageId, botMessages.textMessages["countdownStarted"] + "\n\n00:00");                
        Thread.Sleep(requestDelay);
    }

    // Async method for handling messages from users
    private static async void HandleMessages(ITelegramBotClient client, Update update)
    {
        // Using sleep to not make too many requests
        Thread.Sleep(requestDelay);
        
        // Checking if user has username (It is necessary to have username to participate)
        if (update.Message?.From?.Username == null)
        {
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            await client.SendTextMessageAsync(update.Message.Chat.Id, $"<b>{update.Message?.From?.FirstName}</b>" + botMessages.textMessages["checkUsername"], parseMode: ParseMode.Html);
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        else
        {
            if (update.Message?.Text == "/start" || update.Message?.Text == "/start@ShpigonGameBot")
            {
                if (isStarted.ContainsKey(update.Message.Chat.Id) && isStarted[update.Message.Chat.Id])
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $"@{update.Message?.From?.Username}" + botMessages.textMessages["startAlready"], parseMode: ParseMode.Html);
                
                else   
                    await client.SendTextMessageAsync(update.Message.Chat.Id, botMessages.textMessages["start"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupStart"]);
            }
            
            else if (update.Message?.Text == "/help" || update.Message?.Text == "/help@ShpigonGameBot")
                await client.SendTextMessageAsync(update.Message.Chat.Id, botMessages.textMessages["help"], parseMode: ParseMode.Html);

            else if (update.Message?.Text == "/commands" || update.Message?.Text == "/commands@ShpigonGameBot")
                await client.SendTextMessageAsync(update.Message.Chat.Id, botMessages.textMessages["commands"], parseMode: ParseMode.Html);
            
            else if (update.Message?.Text == "/stop" || update.Message?.Text == "/stop@ShpigonGameBot")
            {
                if (!isStarted.ContainsKey(update.Message.Chat.Id) && !isStartedAndReady.ContainsKey(update.Message.Chat.Id))
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $"@{update.Message?.From?.Username}" + botMessages.textMessages["stopNotStarted"]);

                else if (isStarted.ContainsKey(update.Message.Chat.Id) && isStartedAndReady.ContainsKey(update.Message.Chat.Id) && !isStarted[update.Message.Chat.Id] && !isStartedAndReady[update.Message.Chat.Id])
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $"@{update.Message?.From?.Username}" + botMessages.textMessages["stopNotStarted"]);
                
                else
                    await client.SendTextMessageAsync(update.Message.Chat.Id, $"@{update.Message?.From?.Username}" + botMessages.textMessages["stopConfirmation"], replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupStop"]);
            }
            
            else {}
        }
    }

    // Async method for handling callbacks from users
    private static async void HandleCallbackQuery(ITelegramBotClient client, CallbackQuery callbackQuery)
    {
        // Using sleep to not make too many requests
        Thread.Sleep(requestDelay);

        // Declaring variable for countdowns
        Message lastMessage;

        // Checking if user has username (It is necessary to have username to participate)
        if (callbackQuery.From.Username == null)
        {
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"<b>{callbackQuery.From.FirstName}</b>" + botMessages.textMessages["checkUsername"], parseMode: ParseMode.Html);
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        else
        {
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            // Checking if the game is started
            if (!usersIds.ContainsKey(callbackQuery.Message.Chat.Id) && callbackQuery.Data != "start_game")
                await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["gameNotStarted"], parseMode: ParseMode.Html);
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
            
            else if (callbackQuery.Data == "start_game")
            {
                // Checking if the game is already started
                if (isStarted.ContainsKey(callbackQuery.Message.Chat.Id) && isStarted[callbackQuery.Message.Chat.Id])
                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["startgameAlready"]);
                
                else
                {
                    isStarted[callbackQuery.Message.Chat.Id] = true;

                    // Creating dictionaries for storing information of users from this chat
                    votes[callbackQuery.Message.Chat.Id] = new Dictionary<string, int>();
                    isVoted[callbackQuery.Message.Chat.Id] = new Dictionary<long, bool>();

                    // Creating lists for storing information of users from this chat
                    usersIds[callbackQuery.Message.Chat.Id] = new List<List<string>>();
                    for (int i = 0; i < 2; i++)
                        usersIds[callbackQuery.Message.Chat.Id].Add(new List<string>());

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["startgameAdvice"] + $"<b>{usersIds[callbackQuery.Message.Chat.Id][0].Count()}</b>" + botMessages.textMessages["startgameAdvice2"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupLobby"]);
                }
            }

            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            // Code block for handling voting
            else if (callbackQuery.Data.StartsWith("user"))
            {
                // Checking if user is participating
                if (usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                {
                    // Checking if user have already voted
                    if (isVoted[callbackQuery.Message.Chat.Id].ContainsKey(callbackQuery.From.Id) && isVoted[callbackQuery.Message.Chat.Id][callbackQuery.From.Id] == true)
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["votingAlreadyChose"]);
                    
                    else
                    {
                        string[] parts = callbackQuery.Data.Split("_");
                        string userName = string.Join("_", parts.Skip(1));

                        votes[callbackQuery.Message.Chat.Id][userName]++;

                        isVoted[callbackQuery.Message.Chat.Id][callbackQuery.From.Id] = true;
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["votingChose"] + $"@{userName}");
                        // Using sleep to not make too many requests
                        Thread.Sleep(requestDelay);

                        // Ending the voting if everybody have already voted 
                        if (isVoted[callbackQuery.Message.Chat.Id].Count() == usersIds[callbackQuery.Message.Chat.Id][0].Count())
                        {
                            await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                            // Using sleep to not make too many requests
                            Thread.Sleep(requestDelay);

                            string votesList = CreateVotesList(votes[callbackQuery.Message.Chat.Id]);
                            await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["votingAllVoted"] + votesList + botMessages.textMessages["votingAllVotedAdvice"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupVotingSpyShowUp"]);
                        }
                    }
                }

                else
                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
            }
            #pragma warning restore CS8602 // Dereference of a possibly null reference.

            // Main switch code block for handling callbacks
            else switch (callbackQuery.Data)
            {
                case "save_user":
                    // Checking if the game is already started
                    if (isStarted.ContainsKey(callbackQuery.Message.Chat.Id) && isStarted[callbackQuery.Message.Chat.Id] && isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id) && isStartedAndReady[callbackQuery.Message.Chat.Id])
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["startgameAlready"]);
                        break;
                    }

                    // Checking if user is already saved
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()) && usersIds[callbackQuery.Message.Chat.Id][0].Count() < 10) 
                    {
                        // Saving user ID
                        usersIds[callbackQuery.Message.Chat.Id][0] = usersIds[callbackQuery.Message.Chat.Id][0].Append<string>(callbackQuery.From.Id.ToString()).ToList();

                        // Saving username
                        usersIds[callbackQuery.Message.Chat.Id][1] = usersIds[callbackQuery.Message.Chat.Id][1].Append<string>(callbackQuery.From.Username.ToString()).ToList();
                        
                        // Changing the info about the number of participating players
                        await client.EditMessageTextAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, botMessages.textMessages["startgameAdvice"] + $"<b>{usersIds[callbackQuery.Message.Chat.Id][0].Count()}</b>" + botMessages.textMessages["startgameAdvice2"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupLobby"]);
                    }

                    // Checking the number of players (max. 10)
                    else if (usersIds[callbackQuery.Message.Chat.Id][0].Count() >= 10)
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["saveuserNoMore"]);
                        break;
                    }
                    
                    else
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["saveuserAlready"]);
                    break;
                
                case "ready":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    // Checking if anybody participating
                    if (usersIds[callbackQuery.Message.Chat.Id][0].Count() == 0)
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["readyNobody"]); 
                        break;
                    }

                    // Checking if the game is already started
                    if (isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id) && isStartedAndReady[callbackQuery.Message.Chat.Id])
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["readyStarted"]);
                        break;
                    }

                    Random random = new Random();

                    // Generating random index for spy choosing
                    int index = random.Next(usersIds[callbackQuery.Message.Chat.Id][0].Count());

                    // A variable for storing ID of spy
                    long spyId = long.Parse(usersIds[callbackQuery.Message.Chat.Id][0][index]);
                    spyIds[callbackQuery.Message.Chat.Id] = spyId;
                    
                    // An array for storing IDs of civilians
                    long[] civiliansArray = new long[]{};

                    // Copying IDs of civilians to the new array
                    foreach (string element in usersIds[callbackQuery.Message.Chat.Id][0])
                    {
                        if (element == spyId.ToString())
                            continue;

                        civiliansArray = civiliansArray.Append<long>(long.Parse(element)).ToArray();
                    }

                    // Checking the availability of civilian chats by trying to send dice to their private bot chats
                    foreach (long element in civiliansArray)
                    {
                        try {await client.SendDiceAsync(element);}
                        catch (Telegram.Bot.Exceptions.ApiRequestException exception) 
                        {
                            string userName = $"{usersIds[callbackQuery.Message.Chat.Id][1][usersIds[callbackQuery.Message.Chat.Id][0].IndexOf(element.ToString())]}";
                            
                            // Using sleep to not make too many requests
                            Thread.Sleep(requestDelay);
                            await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{userName}" + botMessages.textMessages["readyBlocked"], parseMode: ParseMode.Html);
                            
                            Console.WriteLine("\n\nException occured.\n");
                            Console.WriteLine(exception.Message);
                            goto case "break_case";
                        }
                        // Using sleep to not make too many requests
                        Thread.Sleep(requestDelay);
                    }

                    // Checking the availability of spy's chat
                    try {await client.SendDiceAsync(spyId);}
                    catch (Telegram.Bot.Exceptions.ApiRequestException exception)
                    {
                        string userName = $"{usersIds[callbackQuery.Message.Chat.Id][1][usersIds[callbackQuery.Message.Chat.Id][0].IndexOf(spyId.ToString())]}";
                        
                        // Using sleep to not make too many requests
                        Thread.Sleep(requestDelay);
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{userName}" + botMessages.textMessages["readyBlocked"], parseMode: ParseMode.Html);
                        
                        Console.WriteLine("\n\nException occured.\n");
                        Console.WriteLine(exception.Message); 
                        goto case "break_case";
                    }

                    // Getting the random topic for game
                    string topic = topics.chooseTopic();
                    
                    // Getting the random location and the list with roles for the game
                    (string location, List<string> rolesForLocation) = topics.chooseLocation(topic); 

                    // Sending text messages to civilians
                    foreach (long element in civiliansArray)
                    {
                        // Getting the role for the game
                        (string role, List<string> rolesForLocationTemp) = topics.chooseRole(rolesForLocation);
                        rolesForLocation = rolesForLocationTemp;

                        await client.SendTextMessageAsync(element, botMessages.textMessages["readyTopic"] + $"{topic}.\n" + botMessages.textMessages["readyLocation"] + $"{location}.\n" + botMessages.textMessages["readyRoleCivilian"] + $"{role}", parseMode: ParseMode.Html);
                        // Using sleep to not make too many requests
                        Thread.Sleep(requestDelay);
                    }

                    // Sending text message to spy
                    await client.SendTextMessageAsync(spyId, botMessages.textMessages["readyTopic"] + $"{topic}.\n" + botMessages.textMessages["readyRoleSpy"], parseMode: ParseMode.Html);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    goto case "game_started";

                case "game_started":
                    isStartedAndReady[callbackQuery.Message.Chat.Id] = true;

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["gamestarted"], parseMode: ParseMode.Html);
                    Thread.Sleep(3000);
                    goto case "round1";

                case "roundFindSpy":
                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["roundFindSpy"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupStartRoundFindSpy"]);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    break;

                case "roundFindSpy_countdown":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    
                    // Starting the timer
                    lastMessage = await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["countdownStarted"]);
                    Timer(minutes: 1, lastMessage, callbackQuery.Message.Chat.Id, client);

                    // Checking if the game was stoped during the sleep 
                    if (!isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id)) {goto case "break_case";}

                    if (roundNow[callbackQuery.Message.Chat.Id] == 5) {await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["roundFindSpyEndNextVoting"], parseMode: ParseMode.Html);}
                    else {await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["roundFindSpyEnd"], parseMode: ParseMode.Html);}
                    Thread.Sleep(5000);

                    if (roundNow[callbackQuery.Message.Chat.Id] == 1) {goto case "round2";}
                    if (roundNow[callbackQuery.Message.Chat.Id] == 2) {goto case "round3";}
                    if (roundNow[callbackQuery.Message.Chat.Id] == 3) {goto case "round4";}
                    if (roundNow[callbackQuery.Message.Chat.Id] == 4) {goto case "round5";}
                    if (roundNow[callbackQuery.Message.Chat.Id] == 5) {goto case "voting";}
                    break;

                case "round1":
                    roundNow[callbackQuery.Message.Chat.Id] = 1;

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["round1Advice"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupStartRound1"]);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    break;

                case "round1_countdown":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);

                    // Starting the timer
                    lastMessage = await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["countdownStarted"]);
                    Timer(minutes: 2, lastMessage, callbackQuery.Message.Chat.Id, client);

                    // Checking if the game was stoped during the sleep 
                    if (!isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id)) {goto case "break_case";}

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["roundEnd"], parseMode: ParseMode.Html);
                    Thread.Sleep(5 * 1000);
                    goto case "roundFindSpy";

                case "round2":
                    roundNow[callbackQuery.Message.Chat.Id]++;

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["round2Advice"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupStartRound2"]);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    break;

                case "round2_countdown":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    
                    // Starting the timer
                    lastMessage = await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["countdownStarted"]);
                    Timer(minutes: 2, lastMessage, callbackQuery.Message.Chat.Id, client);

                    // Checking if the game was stoped during the sleep 
                    if (!isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id)) {goto case "break_case";}

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["roundEnd"], parseMode: ParseMode.Html);
                    Thread.Sleep(5 * 1000);
                    goto case "roundFindSpy";

                case "round3":
                    roundNow[callbackQuery.Message.Chat.Id]++;

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["round3Advice"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupStartRound3"]);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    break;

                case "round3_countdown":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);   
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    
                    // Starting the timer
                    lastMessage = await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["countdownStarted"]);
                    Timer(minutes: 3, lastMessage, callbackQuery.Message.Chat.Id, client);

                    // Checking if the game was stoped during the sleep 
                    if (!isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id)) {goto case "break_case";}

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["roundEnd"], parseMode: ParseMode.Html);
                    Thread.Sleep(5 * 1000);
                    goto case "roundFindSpy";

                case "round4":
                    roundNow[callbackQuery.Message.Chat.Id]++;

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["round4Advice"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupStartRound4"]);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    break;

                case "round4_countdown":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    
                    // Starting the timer
                    lastMessage = await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["countdownStarted"]);
                    Timer(minutes: 3, lastMessage, callbackQuery.Message.Chat.Id, client);

                    // Checking if the game was stoped during the sleep 
                    if (!isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id)) {goto case "break_case";}

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["roundEnd"], parseMode: ParseMode.Html);
                    Thread.Sleep(5 * 1000);
                    goto case "roundFindSpy";

                case "round5":
                    roundNow[callbackQuery.Message.Chat.Id]++;

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["round5Advice"], parseMode: ParseMode.Html, replyMarkup: botMessages.inlineKeyboardMarkups["keyboardInlineMarkupStartRound5"]);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    break;

                case "round5_countdown":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    
                    // Starting the timer
                    lastMessage = await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["countdownStarted"]);
                    Timer(minutes: 2, lastMessage, callbackQuery.Message.Chat.Id, client);

                    // Checking if the game was stoped during the sleep 
                    if (!isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id)) {goto case "break_case";}

                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["roundEnd"], parseMode: ParseMode.Html);
                    Thread.Sleep(5 * 1000);
                    goto case "roundFindSpy";

                case "voting":
                    foreach (string userName in usersIds[callbackQuery.Message.Chat.Id][1])
                        votes[callbackQuery.Message.Chat.Id][userName] = 0;

                    InlineKeyboardMarkup keyboardInlineMarkupStartVoting = CreateInlineKeyboard(usersIds[callbackQuery.Message.Chat.Id][1]);
                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["votingAdvice"], parseMode: ParseMode.Html, replyMarkup: keyboardInlineMarkupStartVoting);
                    break;

                case "spyshowup_accept":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    // Removing all records from chat for stoping the game
                    usersIds.Remove(callbackQuery.Message.Chat.Id);
                    spyIds.Remove(callbackQuery.Message.Chat.Id);
                    isStarted.Remove(callbackQuery.Message.Chat.Id);
                    isStartedAndReady.Remove(callbackQuery.Message.Chat.Id);
                    roundNow.Remove(callbackQuery.Message.Chat.Id);
                    votes.Remove(callbackQuery.Message.Chat.Id);
                    isVoted.Remove(callbackQuery.Message.Chat.Id);
                    isSpyWon.Remove(callbackQuery.Message.Chat.Id);

                    await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["votingSpyShowedUpEnd"], parseMode: ParseMode.Html);
                    break;

                case "spyshowup_deny":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()))
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    // Getting the summary of the game
                    foreach (string userName in votes[callbackQuery.Message.Chat.Id].Keys)
                    {
                        if (userName == usersIds[callbackQuery.Message.Chat.Id][1][usersIds[callbackQuery.Message.Chat.Id][0].IndexOf(spyIds[callbackQuery.Message.Chat.Id].ToString())])
                            continue;

                        if (votes[callbackQuery.Message.Chat.Id][userName] >= votes[callbackQuery.Message.Chat.Id][usersIds[callbackQuery.Message.Chat.Id][1][usersIds[callbackQuery.Message.Chat.Id][0].IndexOf(spyIds[callbackQuery.Message.Chat.Id].ToString())]])
                            isSpyWon[callbackQuery.Message.Chat.Id] = true;

                        else if (votes[callbackQuery.Message.Chat.Id][userName] < votes[callbackQuery.Message.Chat.Id][usersIds[callbackQuery.Message.Chat.Id][1][usersIds[callbackQuery.Message.Chat.Id][0].IndexOf(spyIds[callbackQuery.Message.Chat.Id].ToString())]])
                        {
                            if (isSpyWon.ContainsKey(callbackQuery.Message.Chat.Id) && isSpyWon[callbackQuery.Message.Chat.Id] == true)
                                continue;

                            isSpyWon[callbackQuery.Message.Chat.Id] = false;
                        }
                    
                        else
                            continue;
                    }

                    await client.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);

                    if (isSpyWon.ContainsKey(callbackQuery.Message.Chat.Id) && isSpyWon[callbackQuery.Message.Chat.Id])
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["votingSpyNotShowedUpEnd"] + $"@{usersIds[callbackQuery.Message.Chat.Id][1][usersIds[callbackQuery.Message.Chat.Id][0].IndexOf(spyIds[callbackQuery.Message.Chat.Id].ToString())]}" + botMessages.textMessages["votingSpyWon"], parseMode: ParseMode.Html);
                    else
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, botMessages.textMessages["votingSpyNotShowedUpEnd"] + $"@{usersIds[callbackQuery.Message.Chat.Id][1][usersIds[callbackQuery.Message.Chat.Id][0].IndexOf(spyIds[callbackQuery.Message.Chat.Id].ToString())]}" + botMessages.textMessages["votingSpyLose"], parseMode: ParseMode.Html);

                    // Removing all records from chat for stoping the game
                    usersIds.Remove(callbackQuery.Message.Chat.Id);
                    spyIds.Remove(callbackQuery.Message.Chat.Id);
                    isStarted.Remove(callbackQuery.Message.Chat.Id);
                    isStartedAndReady.Remove(callbackQuery.Message.Chat.Id);
                    roundNow.Remove(callbackQuery.Message.Chat.Id);
                    votes.Remove(callbackQuery.Message.Chat.Id);
                    isVoted.Remove(callbackQuery.Message.Chat.Id);
                    isSpyWon.Remove(callbackQuery.Message.Chat.Id);
                    break;

                case "stop_accept":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()) && isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id) && isStartedAndReady[callbackQuery.Message.Chat.Id])
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    // Removing all records from chat for stoping the game
                    usersIds.Remove(callbackQuery.Message.Chat.Id);
                    spyIds.Remove(callbackQuery.Message.Chat.Id);
                    isStarted.Remove(callbackQuery.Message.Chat.Id);
                    isStartedAndReady.Remove(callbackQuery.Message.Chat.Id);
                    roundNow.Remove(callbackQuery.Message.Chat.Id);
                    votes.Remove(callbackQuery.Message.Chat.Id);
                    isVoted.Remove(callbackQuery.Message.Chat.Id);
                    isSpyWon.Remove(callbackQuery.Message.Chat.Id);

                    await client.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["stopAccept"]);
                    break;

                case "stop_deny":
                    // Checking if user is participating
                    if (!usersIds[callbackQuery.Message.Chat.Id][0].Contains(callbackQuery.From.Id.ToString()) && isStartedAndReady.ContainsKey(callbackQuery.Message.Chat.Id) && isStartedAndReady[callbackQuery.Message.Chat.Id])
                    {
                        await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["notParticipating"]);
                        break;
                    }

                    await client.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
                    // Using sleep to not make too many requests
                    Thread.Sleep(requestDelay);
                    await client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"@{callbackQuery.From.Username}" + botMessages.textMessages["stopDeny"]);
                    break;

                case "break_case":
                    break;
            }
        }
    }
}
