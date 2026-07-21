namespace TelegramShpigonGameBot;

internal static class VotingRules
{
    public static bool SpyWins(GameSession session)
    {
        if (session.SpyId is null)
        {
            throw new InvalidOperationException("A spy must be assigned before voting is evaluated.");
        }

        var counts = session.VotesByPlayer.Values
            .GroupBy(candidateId => candidateId)
            .ToDictionary(group => group.Key, group => group.Count());
        var spyVotes = counts.GetValueOrDefault(session.SpyId.Value);
        var highestOther = session.Players
            .Where(player => player.Id != session.SpyId)
            .Select(player => counts.GetValueOrDefault(player.Id))
            .DefaultIfEmpty(0)
            .Max();

        return highestOther >= spyVotes;
    }
}
