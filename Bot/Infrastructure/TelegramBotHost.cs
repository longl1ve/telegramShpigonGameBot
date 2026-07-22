using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramShpigonGameBot;

internal sealed class TelegramBotHost(
    string token,
    Func<ITelegramBotClient, Update, CancellationToken, Task> handleMessage,
    Func<ITelegramBotClient, CallbackQuery, CancellationToken, Task> handleCallback)
{
    private readonly TelegramBotClient telegramBotClient = new(token);

    public void Start()
    {
        telegramBotClient.StartReceiving(UpdateHandlerAsync, HandleErrorAsync);
        Console.WriteLine("Bot has been started.");
    }

    private async Task UpdateHandlerAsync(
        ITelegramBotClient client,
        Update update,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Update received from chat {update.Message?.Chat.Id ?? 0}: " +
            $"{update.Message?.Text ?? "[non-text update]"}");

        if (update.Type == UpdateType.Message && update.Message?.Text is not null)
        {
            await handleMessage(client, update, cancellationToken);
        }
        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery is not null)
        {
            await handleCallback(client, update.CallbackQuery, cancellationToken);
        }
    }

    private static Task HandleErrorAsync(
        ITelegramBotClient client,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ApiRequestException { Message: "Not Found" or "Unauthorized" })
        {
            Console.Error.WriteLine(
                "Telegram rejected the bot token. Check the SHPIGON_TOKEN environment variable.");
        }
        else
        {
            Console.Error.WriteLine($"Telegram receiver error: {exception.Message}");
        }

        return Task.CompletedTask;
    }
}
