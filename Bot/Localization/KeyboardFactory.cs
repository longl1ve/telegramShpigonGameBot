using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramShpigonGameBot;

internal sealed class KeyboardFactory(IBotLocalizer localizer)
{
    public InlineKeyboardMarkup CreateLobby() => Rows(
        [Button("Join", "save_user")],
        [Button("Ready", "ready")]);

    public InlineKeyboardMarkup CreateGameStart() =>
        Rows([Button("StartGame", "start_game")]);

    public InlineKeyboardMarkup CreateStopConfirmation() =>
        Rows([Button("Yes", "stop_accept"), Button("No", "stop_deny")]);

    public InlineKeyboardMarkup CreateGuessingStart() =>
        Rows([Button("Begin", "roundFindSpy_countdown")]);

    public InlineKeyboardMarkup CreateRoundStart(int round)
    {
        if (round is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(round), round, "Round must be between 1 and 5.");
        }

        return Rows([Button("Begin", $"round{round}_countdown")]);
    }

    public InlineKeyboardMarkup CreateSpyDecision() => Rows(
        [Button("Yes", "spyshowup_accept")],
        [Button("No", "spyshowup_deny")]);

    private InlineKeyboardButton Button(string labelKey, string callbackData) =>
        InlineKeyboardButton.WithCallbackData(localizer.GetButton(labelKey), callbackData);

    private static InlineKeyboardMarkup Rows(params InlineKeyboardButton[][] rows) => new(rows);
}
