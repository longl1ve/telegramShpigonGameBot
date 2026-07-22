using Telegram.Bot;
using TelegramShpigonGameBot;

internal static class Program
{
    private static async Task Main()
    {
        var token = Environment.GetEnvironmentVariable("SHPIGON_TOKEN");
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("Set the SHPIGON_TOKEN environment variable before starting the bot.");
        }

        var language = Environment.GetEnvironmentVariable("SHPIGON_LANGUAGE")?.ToLowerInvariant() ?? "en";
        if (language is not ("en" or "uk" or "ua"))
        {
            throw new InvalidOperationException("SHPIGON_LANGUAGE must be 'en' or 'uk'.");
        }

        var resources = new JsonGameResources(language == "ua" ? "uk" : language);
        var controller = new BotController(resources, new KeyboardFactory(resources), resources, new GameSessionStore());
        var host = new TelegramBotHost(
            token,
            controller.HandleMessageAsync,
            controller.HandleCallbackAsync);

        host.Start();
        await Task.Delay(Timeout.Infinite);
    }
}
