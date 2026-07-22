namespace TelegramShpigonGameBot;

internal sealed record RoundDefinition(int Number, string MessageKey, TimeSpan Duration, string CallbackData)
{
    public static readonly IReadOnlyList<RoundDefinition> All =
    [
        new(1, "round1Advice", TimeSpan.FromMinutes(2), "round1_countdown"),
        new(2, "round2Advice", TimeSpan.FromMinutes(2), "round2_countdown"),
        new(3, "round3Advice", TimeSpan.FromMinutes(3), "round3_countdown"),
        new(4, "round4Advice", TimeSpan.FromMinutes(3), "round4_countdown"),
        new(5, "round5Advice", TimeSpan.FromMinutes(2), "round5_countdown")
    ];

    public static RoundDefinition Get(int number) => All.Single(round => round.Number == number);
}
