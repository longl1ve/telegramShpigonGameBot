namespace TelegramShpigonGameBot;

internal sealed class GameSession(long chatId)
{
    public long ChatId { get; } = chatId;
    public GamePhase Phase { get; set; } = GamePhase.Lobby;
    public List<Player> Players { get; } = [];
    public long? SpyId { get; set; }
    public int CurrentRound { get; set; }
    public Dictionary<long, long> VotesByPlayer { get; } = [];
    public CancellationTokenSource? CountdownCancellation { get; set; }
    public SemaphoreSlim Gate { get; } = new(1, 1);

    public bool HasPlayer(long playerId) => Players.Any(player => player.Id == playerId);

    public Player? FindPlayer(long playerId) => Players.FirstOrDefault(player => player.Id == playerId);

    public void StopCountdown()
    {
        CountdownCancellation?.Cancel();
        CountdownCancellation?.Dispose();
        CountdownCancellation = null;
    }
}
