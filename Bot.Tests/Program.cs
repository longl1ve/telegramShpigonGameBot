using TelegramShpigonGameBot;

var tests = new (string Name, Action Run)[]
{
    ("A session keeps player identity together", PlayerIdentity),
    ("The session store returns one session per chat", SessionStoreIdentity),
    ("Removing a session cancels its countdown", SessionRemovalCancelsCountdown),
    ("All five rounds have unique callbacks", RoundDefinitionsAreValid),
    ("The spy wins a tied vote", SpyWinsTie),
    ("The spy loses with the unique highest vote count", SpyLosesHighestVote),
    ("Choosing roles does not mutate shared content", RolesAreCopied),
    ("Language resources contain matching structures", LocalizationKeysMatch),
    ("Every static keyboard can be created", KeyboardsAreValid)
};

foreach (var test in tests)
{
    test.Run();
    Console.WriteLine($"PASS: {test.Name}");
}

static void PlayerIdentity()
{
    var session = new GameSession(10);
    session.Players.Add(new Player(20, "alice"));
    Assert(session.HasPlayer(20));
    Assert(session.FindPlayer(20)?.Username == "alice");
}

static void SessionStoreIdentity()
{
    var store = new GameSessionStore();
    Assert(ReferenceEquals(store.GetOrCreate(10), store.GetOrCreate(10)));
}

static void SessionRemovalCancelsCountdown()
{
    var store = new GameSessionStore();
    var session = store.GetOrCreate(10);
    session.CountdownCancellation = new CancellationTokenSource();
    var token = session.CountdownCancellation.Token;
    Assert(store.Remove(10));
    Assert(token.IsCancellationRequested);
    Assert(!store.TryGet(10, out _));
}

static void RoundDefinitionsAreValid()
{
    Assert(RoundDefinition.All.Count == 5);
    Assert(RoundDefinition.All.Select(round => round.CallbackData).Distinct().Count() == 5);
    Assert(RoundDefinition.All.Select(round => round.Number).SequenceEqual([1, 2, 3, 4, 5]));
}

static void SpyWinsTie()
{
    var session = VotingSession();
    session.VotesByPlayer[1] = 1;
    session.VotesByPlayer[2] = 2;
    session.VotesByPlayer[3] = 3;
    Assert(VotingRules.SpyWins(session));
}

static void SpyLosesHighestVote()
{
    var session = VotingSession();
    session.VotesByPlayer[1] = 1;
    session.VotesByPlayer[2] = 1;
    session.VotesByPlayer[3] = 2;
    Assert(!VotingRules.SpyWins(session));
}

static GameSession VotingSession()
{
    var session = new GameSession(10) { SpyId = 1 };
    session.Players.AddRange([new Player(1, "spy"), new Player(2, "two"), new Player(3, "three")]);
    return session;
}

static void RolesAreCopied()
{
    var content = new JsonGameResources("en");
    var seen = new Dictionary<string, int>();
    for (var attempt = 0; attempt < 200; attempt++)
    {
        var (location, roles) = content.ChooseLocation("Airport");
        if (seen.TryGetValue(location, out var expectedCount))
        {
            Assert(roles.Count == expectedCount);
        }
        else
        {
            seen[location] = roles.Count;
        }

        roles.Clear();
    }
}

static void LocalizationKeysMatch()
{
    var english = new JsonGameResources("en");
    var ukrainian = new JsonGameResources("uk");
    Assert(english.MessageKeys.ToHashSet().SetEquals(ukrainian.MessageKeys));
    Assert(english.ButtonKeys.ToHashSet().SetEquals(ukrainian.ButtonKeys));
    Assert(english.MessageKeys.Count == 42);
    Assert(english.ButtonKeys.Count == 6);
    Assert(english.LocationCounts.SequenceEqual(ukrainian.LocationCounts));
    Assert(english.RoleCounts.SequenceEqual(ukrainian.RoleCounts));
    Assert(english.LocationCounts.Sum() == 50);
    Assert(english.RoleCounts.Sum() == 500);
}

static void KeyboardsAreValid()
{
    var factory = new KeyboardFactory(new JsonGameResources("en"));
    var keyboards = new[]
    {
        factory.CreateLobby(),
        factory.CreateGameStart(),
        factory.CreateStopConfirmation(),
        factory.CreateGuessingStart(),
        factory.CreateRoundStart(1),
        factory.CreateRoundStart(2),
        factory.CreateRoundStart(3),
        factory.CreateRoundStart(4),
        factory.CreateRoundStart(5),
        factory.CreateSpyDecision()
    };
    Assert(keyboards.All(keyboard => keyboard.InlineKeyboard.Any()));
}

static void Assert(bool condition)
{
    if (!condition)
    {
        throw new InvalidOperationException("Assertion failed.");
    }
}
