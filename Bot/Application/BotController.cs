using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramShpigonGameBot;

internal sealed class BotController(
    IBotLocalizer messages,
    KeyboardFactory keyboards,
    IGameContentProvider topics,
    GameSessionStore sessions)
{
    private const int MaximumPlayers = 10;
    private static readonly TimeSpan RequestDelay = TimeSpan.FromMilliseconds(500);
    private static readonly TimeSpan GameStartDelay = TimeSpan.FromSeconds(3);
    private static readonly TimeSpan RoundTransitionDelay = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan GuessingDuration = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan CountdownUpdateInterval = TimeSpan.FromSeconds(5);

    public async Task HandleMessageAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message?.From is null || message.Text is null)
        {
            return;
        }

        if (!HasUsername(message.From))
        {
            await SendMissingUsernameAsync(client, message.Chat.Id, message.From, cancellationToken);
            return;
        }

        var command = message.Text.Split('@', 2)[0];
        switch (command)
        {
            case "/start":
                if (sessions.TryGet(message.Chat.Id, out _))
                {
                    await client.SendTextMessageAsync(message.Chat.Id, $"@{message.From.Username}{Text("startAlready")}", parseMode: ParseMode.Html, cancellationToken: cancellationToken);
                }
                else
                {
                    await client.SendTextMessageAsync(message.Chat.Id, Text("start"), parseMode: ParseMode.Html, replyMarkup: keyboards.CreateGameStart(), cancellationToken: cancellationToken);
                }

                break;
            case "/help":
                await client.SendTextMessageAsync(message.Chat.Id, Text("help"), parseMode: ParseMode.Html, cancellationToken: cancellationToken);
                break;
            case "/commands":
                await client.SendTextMessageAsync(message.Chat.Id, Text("commands"), parseMode: ParseMode.Html, cancellationToken: cancellationToken);
                break;
            case "/stop":
                if (!sessions.TryGet(message.Chat.Id, out _))
                {
                    await client.SendTextMessageAsync(message.Chat.Id, $"@{message.From.Username}{Text("stopNotStarted")}", cancellationToken: cancellationToken);
                }
                else
                {
                    await client.SendTextMessageAsync(message.Chat.Id, $"@{message.From.Username}{Text("stopConfirmation")}", replyMarkup: keyboards.CreateStopConfirmation(), cancellationToken: cancellationToken);
                }

                break;
        }
    }

    public async Task HandleCallbackAsync(ITelegramBotClient client, CallbackQuery callback, CancellationToken cancellationToken)
    {
        await client.AnswerCallbackQueryAsync(callback.Id, cancellationToken: cancellationToken);
        if (callback.Message is null)
        {
            return;
        }

        if (!HasUsername(callback.From))
        {
            await SendMissingUsernameAsync(client, callback.Message.Chat.Id, callback.From, cancellationToken);
            return;
        }

        var data = callback.Data ?? string.Empty;
        if (data == "start_game")
        {
            await StartLobbyAsync(client, callback, cancellationToken);
            return;
        }

        if (!sessions.TryGet(callback.Message.Chat.Id, out var session) || session is null)
        {
            await client.SendTextMessageAsync(callback.Message.Chat.Id, $"@{callback.From.Username}{Text("gameNotStarted")}", parseMode: ParseMode.Html, cancellationToken: cancellationToken);
            return;
        }

        if (data.StartsWith("vote:", StringComparison.Ordinal))
        {
            await VoteAsync(client, callback, session, data, cancellationToken);
        }
        else if (data == "save_user")
        {
            await JoinAsync(client, callback, session, cancellationToken);
        }
        else if (data == "ready")
        {
            await StartGameAsync(client, callback, session, cancellationToken);
        }
        else if (data == "roundFindSpy_countdown")
        {
            await RunGuessingAsync(client, callback, session, cancellationToken);
        }
        else if (TryGetRound(data, out var round))
        {
            await RunRoundAsync(client, callback, session, round, cancellationToken);
        }
        else if (data == "spyshowup_accept")
        {
            await FinishSpyRevealAsync(client, callback, session, cancellationToken);
        }
        else if (data == "spyshowup_deny")
        {
            await FinishVotingAsync(client, callback, session, cancellationToken);
        }
        else if (data == "stop_accept")
        {
            await StopAsync(client, callback, session, cancellationToken);
        }
        else if (data == "stop_deny")
        {
            await DenyStopAsync(client, callback, session, cancellationToken);
        }
    }

    private async Task StartLobbyAsync(ITelegramBotClient client, CallbackQuery callback, CancellationToken token)
    {
        var chatId = callback.Message!.Chat.Id;
        if (sessions.TryGet(chatId, out _))
        {
            await client.SendTextMessageAsync(chatId, $"@{callback.From.Username}{Text("startgameAlready")}", cancellationToken: token);
            return;
        }

        sessions.GetOrCreate(chatId);
        await client.SendTextMessageAsync(chatId, LobbyText(0), parseMode: ParseMode.Html, replyMarkup: keyboards.CreateLobby(), cancellationToken: token);
    }

    private async Task JoinAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, CancellationToken token)
    {
        await session.Gate.WaitAsync(token);
        try
        {
            if (session.Phase != GamePhase.Lobby)
            {
                await client.SendTextMessageAsync(session.ChatId, $"@{callback.From.Username}{Text("startgameAlready")}", cancellationToken: token);
            }
            else if (session.HasPlayer(callback.From.Id))
            {
                await client.SendTextMessageAsync(session.ChatId, $"@{callback.From.Username}{Text("saveuserAlready")}", cancellationToken: token);
            }
            else if (session.Players.Count >= MaximumPlayers)
            {
                await client.SendTextMessageAsync(session.ChatId, $"@{callback.From.Username}{Text("saveuserNoMore")}", cancellationToken: token);
            }
            else
            {
                session.Players.Add(new Player(callback.From.Id, callback.From.Username!));
                await client.EditMessageTextAsync(session.ChatId, callback.Message!.MessageId, LobbyText(session.Players.Count), parseMode: ParseMode.Html, replyMarkup: keyboards.CreateLobby(), cancellationToken: token);
            }
        }
        finally { session.Gate.Release(); }
    }

    private async Task StartGameAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, CancellationToken token)
    {
        await session.Gate.WaitAsync(token);
        try
        {
            if (!session.HasPlayer(callback.From.Id))
            {
                await SendNotParticipatingAsync(client, callback, token);
                return;
            }
            if (session.Phase != GamePhase.Lobby)
            {
                await client.SendTextMessageAsync(session.ChatId, $"@{callback.From.Username}{Text("readyStarted")}", cancellationToken: token);
                return;
            }
            if (session.Players.Count == 0)
            {
                return;
            }

            var spy = session.Players[Random.Shared.Next(session.Players.Count)];
            session.SpyId = spy.Id;

            if (!await CanMessageAllPlayersAsync(client, session, token))
            {
                return;
            }

            var topic = topics.ChooseTopic();
            var (location, roles) = topics.ChooseLocation(topic);
            foreach (var player in session.Players.Where(player => player.Id != spy.Id))
            {
                var (role, remainingRoles) = topics.ChooseRole(roles);
                roles = remainingRoles;
                await client.SendTextMessageAsync(player.Id, $"{Text("readyTopic")}{topic}.\n{Text("readyLocation")}{location}.\n{Text("readyRoleCivilian")}{role}", parseMode: ParseMode.Html, cancellationToken: token);
                await Task.Delay(RequestDelay, token);
            }
            await client.SendTextMessageAsync(spy.Id, $"{Text("readyTopic")}{topic}.\n{Text("readyRoleSpy")}", parseMode: ParseMode.Html, cancellationToken: token);

            session.CurrentRound = 1;
            session.Phase = GamePhase.Round;
        }
        finally { session.Gate.Release(); }

        if (session.Phase == GamePhase.Round)
        {
            await client.SendTextMessageAsync(session.ChatId, Text("gamestarted"), parseMode: ParseMode.Html, cancellationToken: token);
            await Task.Delay(GameStartDelay, token);
            await SendRoundPromptAsync(client, session, RoundDefinition.Get(1), token);
        }
    }

    private async Task<bool> CanMessageAllPlayersAsync(ITelegramBotClient client, GameSession session, CancellationToken token)
    {
        foreach (var player in session.Players)
        {
            try { await client.SendDiceAsync(player.Id, cancellationToken: token); }
            catch (ApiRequestException exception)
            {
                await client.SendTextMessageAsync(session.ChatId, $"@{player.Username}{Text("readyBlocked")}", parseMode: ParseMode.Html, cancellationToken: token);
                Console.Error.WriteLine(exception.Message);
                return false;
            }
            await Task.Delay(RequestDelay, token);
        }
        return true;
    }

    private async Task RunRoundAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, RoundDefinition round, CancellationToken token)
    {
        if (!await TryBeginPhaseAsync(session, callback.From.Id, GamePhase.Round, GamePhase.RoundCountdown, round.Number, token))
        {
            await SendNotParticipatingOrStaleAsync(client, callback, session, token);
            return;
        }

        await client.EditMessageReplyMarkupAsync(session.ChatId, callback.Message!.MessageId, cancellationToken: token);
        if (!await RunCountdownAsync(client, session, round.Duration, token))
        {
            return;
        }

        session.Phase = GamePhase.Guessing;
        await client.SendTextMessageAsync(session.ChatId, Text("roundEnd"), parseMode: ParseMode.Html, cancellationToken: token);
        await Task.Delay(RoundTransitionDelay, token);
        await client.SendTextMessageAsync(session.ChatId, Text("roundFindSpy"), parseMode: ParseMode.Html, replyMarkup: keyboards.CreateGuessingStart(), cancellationToken: token);
    }

    private async Task RunGuessingAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, CancellationToken token)
    {
        if (!await TryBeginPhaseAsync(session, callback.From.Id, GamePhase.Guessing, GamePhase.GuessingCountdown, session.CurrentRound, token))
        {
            await SendNotParticipatingOrStaleAsync(client, callback, session, token);
            return;
        }

        await client.EditMessageReplyMarkupAsync(session.ChatId, callback.Message!.MessageId, cancellationToken: token);
        if (!await RunCountdownAsync(client, session, GuessingDuration, token))
        {
            return;
        }

        if (session.CurrentRound == RoundDefinition.All.Count)
        {
            session.Phase = GamePhase.Voting;
            await client.SendTextMessageAsync(session.ChatId, Text("roundFindSpyEndNextVoting"), parseMode: ParseMode.Html, cancellationToken: token);
            await Task.Delay(RoundTransitionDelay, token);
            await SendVotingPromptAsync(client, session, token);
        }
        else
        {
            await client.SendTextMessageAsync(session.ChatId, Text("roundFindSpyEnd"), parseMode: ParseMode.Html, cancellationToken: token);
            await Task.Delay(RoundTransitionDelay, token);
            session.CurrentRound++;
            await SendRoundPromptAsync(client, session, RoundDefinition.Get(session.CurrentRound), token);
        }
    }

    private static async Task<bool> TryBeginPhaseAsync(GameSession session, long playerId, GamePhase expected, GamePhase next, int round, CancellationToken token)
    {
        await session.Gate.WaitAsync(token);
        try
        {
            if (!session.HasPlayer(playerId) || session.Phase != expected || session.CurrentRound != round)
            {
                return false;
            }

            session.Phase = next;
            return true;
        }
        finally { session.Gate.Release(); }
    }

    private async Task<bool> RunCountdownAsync(ITelegramBotClient client, GameSession session, TimeSpan duration, CancellationToken applicationToken)
    {
        session.StopCountdown();
        session.CountdownCancellation = CancellationTokenSource.CreateLinkedTokenSource(applicationToken);
        var token = session.CountdownCancellation.Token;
        try
        {
            var message = await client.SendTextMessageAsync(session.ChatId, Text("countdownStarted"), cancellationToken: token);
            var remaining = duration;
            while (remaining > TimeSpan.Zero)
            {
                await client.EditMessageTextAsync(session.ChatId, message.MessageId, $"{Text("countdownStarted")}\n\n{remaining:mm\\:ss}", cancellationToken: token);
                await Task.Delay(CountdownUpdateInterval, token);
                remaining -= CountdownUpdateInterval;
            }
            await client.EditMessageTextAsync(session.ChatId, message.MessageId, $"{Text("countdownStarted")}\n\n00:00", cancellationToken: token);
            return true;
        }
        catch (OperationCanceledException) { return false; }
        finally
        {
            session.CountdownCancellation?.Dispose();
            session.CountdownCancellation = null;
        }
    }

    private async Task VoteAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, string data, CancellationToken token)
    {
        if (!long.TryParse(data.AsSpan("vote:".Length), out var candidateId) || !session.HasPlayer(candidateId))
        {
            return;
        }

        await session.Gate.WaitAsync(token);
        try
        {
            if (!session.HasPlayer(callback.From.Id))
            {
                await SendNotParticipatingAsync(client, callback, token);
                return;
            }
            if (session.Phase != GamePhase.Voting || session.VotesByPlayer.ContainsKey(callback.From.Id))
            {
                await client.SendTextMessageAsync(session.ChatId, $"@{callback.From.Username}{Text("votingAlreadyChose")}", cancellationToken: token);
                return;
            }

            session.VotesByPlayer[callback.From.Id] = candidateId;
            var candidate = session.FindPlayer(candidateId)!;
            await client.SendTextMessageAsync(session.ChatId, $"@{callback.From.Username}{Text("votingChose")}@{candidate.Username}", cancellationToken: token);
            if (session.VotesByPlayer.Count != session.Players.Count)
            {
                return;
            }

            session.Phase = GamePhase.SpyDecision;
            await client.EditMessageReplyMarkupAsync(session.ChatId, callback.Message!.MessageId, cancellationToken: token);
            await client.SendTextMessageAsync(session.ChatId, Text("votingAllVoted") + CreateVotesList(session) + Text("votingAllVotedAdvice"), parseMode: ParseMode.Html, replyMarkup: keyboards.CreateSpyDecision(), cancellationToken: token);
        }
        finally { session.Gate.Release(); }
    }

    private async Task FinishVotingAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, CancellationToken token)
    {
        if (!session.HasPlayer(callback.From.Id) || session.SpyId is null)
        {
            await SendNotParticipatingAsync(client, callback, token);
            return;
        }
        var spyWon = VotingRules.SpyWins(session);
        var spy = session.FindPlayer(session.SpyId.Value)!;
        await client.EditMessageReplyMarkupAsync(session.ChatId, callback.Message!.MessageId, cancellationToken: token);
        await client.SendTextMessageAsync(session.ChatId, $"{Text("votingSpyNotShowedUpEnd")}@{spy.Username}{Text(spyWon ? "votingSpyWon" : "votingSpyLose")}", parseMode: ParseMode.Html, cancellationToken: token);
        sessions.Remove(session.ChatId);
    }

    private async Task FinishSpyRevealAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, CancellationToken token)
    {
        if (!session.HasPlayer(callback.From.Id))
        {
            await SendNotParticipatingAsync(client, callback, token);
            return;
        }
        await client.EditMessageReplyMarkupAsync(session.ChatId, callback.Message!.MessageId, cancellationToken: token);
        await client.SendTextMessageAsync(session.ChatId, Text("votingSpyShowedUpEnd"), parseMode: ParseMode.Html, cancellationToken: token);
        sessions.Remove(session.ChatId);
    }

    private async Task StopAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, CancellationToken token)
    {
        if (session.Phase != GamePhase.Lobby && !session.HasPlayer(callback.From.Id))
        {
            await SendNotParticipatingAsync(client, callback, token);
            return;
        }
        sessions.Remove(session.ChatId);
        await client.DeleteMessageAsync(session.ChatId, callback.Message!.MessageId, cancellationToken: token);
        await client.SendTextMessageAsync(session.ChatId, $"@{callback.From.Username}{Text("stopAccept")}", cancellationToken: token);
    }

    private async Task DenyStopAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, CancellationToken token)
    {
        if (session.Phase != GamePhase.Lobby && !session.HasPlayer(callback.From.Id))
        {
            await SendNotParticipatingAsync(client, callback, token);
            return;
        }
        await client.DeleteMessageAsync(session.ChatId, callback.Message!.MessageId, cancellationToken: token);
        await client.SendTextMessageAsync(session.ChatId, $"@{callback.From.Username}{Text("stopDeny")}", cancellationToken: token);
    }

    private async Task SendRoundPromptAsync(ITelegramBotClient client, GameSession session, RoundDefinition round, CancellationToken token) =>
        await client.SendTextMessageAsync(session.ChatId, Text(round.MessageKey), parseMode: ParseMode.Html, replyMarkup: keyboards.CreateRoundStart(round.Number), cancellationToken: token);

    private async Task SendVotingPromptAsync(ITelegramBotClient client, GameSession session, CancellationToken token)
    {
        var rows = session.Players.Select(player => new[] { InlineKeyboardButton.WithCallbackData($"@{player.Username}", $"vote:{player.Id}") });
        await client.SendTextMessageAsync(session.ChatId, Text("votingAdvice"), parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(rows), cancellationToken: token);
    }

    private static string CreateVotesList(GameSession session) => string.Concat(
        session.Players.Select(player => $"\n@{player.Username} - {session.VotesByPlayer.Values.Count(id => id == player.Id)}"));

    private async Task SendNotParticipatingOrStaleAsync(ITelegramBotClient client, CallbackQuery callback, GameSession session, CancellationToken token)
    {
        if (!session.HasPlayer(callback.From.Id))
        {
            await SendNotParticipatingAsync(client, callback, token);
        }
    }

    private async Task SendNotParticipatingAsync(ITelegramBotClient client, CallbackQuery callback, CancellationToken token) =>
        await client.SendTextMessageAsync(callback.Message!.Chat.Id, $"@{callback.From.Username}{Text("notParticipating")}", cancellationToken: token);

    private async Task SendMissingUsernameAsync(ITelegramBotClient client, long chatId, User user, CancellationToken token) =>
        await client.SendTextMessageAsync(chatId, $"<b>{user.FirstName}</b>{Text("checkUsername")}", parseMode: ParseMode.Html, cancellationToken: token);

    private string LobbyText(int count) => $"{Text("startgameAdvice")}<b>{count}</b>{Text("startgameAdvice2")}";
    private string Text(string key) => messages.GetText(key);
    private static bool HasUsername(User user) => !string.IsNullOrWhiteSpace(user.Username);
    private static bool TryGetRound(string data, out RoundDefinition round)
    {
        round = RoundDefinition.All.FirstOrDefault(item => item.CallbackData == data)!;
        return round is not null;
    }
}
