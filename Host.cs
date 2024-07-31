using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace telegramShpigonGameBot
{
    internal class Host
    {
        public TelegramBotClient telegramBotClient;

        // Delegates for pointing to methods in Program.cs
        public Action<ITelegramBotClient, Update>? HandleMessages;
        public Action<ITelegramBotClient, CallbackQuery>? HandleCallbackQuery;

        // Constructor for class
        public Host(string token) 
        {
            telegramBotClient = new TelegramBotClient(token);
        }

        public void Start()
        {
            telegramBotClient.StartReceiving(UpdateHandler, ErrorHandler);
            Console.WriteLine("Bot has been started.");
        }

        // Async task for handling updates, like messages and callbacks from users
        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            Console.WriteLine($"Message is received from id({update.Message?.Chat.Id ?? 0}): {update.Message?.Text ?? "[message is not a text]"}");
            
            // Calling the delegate for handling messages if update type is message
            if (update.Type == UpdateType.Message && update.Message?.Text != null)
                HandleMessages?.Invoke(client, update);

            // Calling the delegate for handling callbacks if update type is callback
            else if (update.Type == UpdateType.CallbackQuery)
            {
                #pragma warning disable CS8604 // Possible null reference argument.
                HandleCallbackQuery?.Invoke(client, update.CallbackQuery);
                #pragma warning restore CS8604 // Possible null reference argument.
            }

            await Task.CompletedTask;
        }

        // Async task for handling occuring errors
        private async Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            // Handling exception with missing or incorrect token for Bot API
            if (exception.GetType().ToString() == "Telegram.Bot.Exceptions.ApiRequestException" && (exception.Message.ToString() == "Not Found" || exception.Message.ToString() == "Unauthorized"))
            {
                Console.WriteLine("Error: Bot API token is incorrect or doesn't exist. Check if you have added the environment variable with the name SHPIGON_TOKEN.\n! A dangerous way ! : You can add Bot API token straight to the program code. This is dangerous for the security of your token data.");
                Console.ReadLine();
            }
            
            Console.WriteLine($"Error: {exception.Message}");
            await Task.CompletedTask;
        }
    }
}
