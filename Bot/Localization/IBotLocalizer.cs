namespace TelegramShpigonGameBot;

internal interface IBotLocalizer
{
    string GetText(string key);
    string GetButton(string key);
}
