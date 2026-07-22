using System.Collections.Concurrent;

namespace TelegramShpigonGameBot;

internal sealed class GameSessionStore
{
    private readonly ConcurrentDictionary<long, GameSession> sessions = new();

    public bool TryGet(long chatId, out GameSession? session) => sessions.TryGetValue(chatId, out session);

    public GameSession GetOrCreate(long chatId) => sessions.GetOrAdd(chatId, static id => new GameSession(id));

    public bool Remove(long chatId)
    {
        if (!sessions.TryRemove(chatId, out var session))
        {
            return false;
        }

        session.StopCountdown();
        return true;
    }
}
