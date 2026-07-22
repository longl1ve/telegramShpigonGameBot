using System.Text.Json;
using System.Text.Json.Serialization;

namespace TelegramShpigonGameBot;

internal sealed class JsonGameResources : IBotLocalizer, IGameContentProvider
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    private readonly LocalizationDocument document;

    public JsonGameResources(string language)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Localization", $"{language}.json");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Localization file was not found: {path}", path);
        }

        var json = File.ReadAllText(path);
        document = JsonSerializer.Deserialize<LocalizationDocument>(json, SerializerOptions)
            ?? throw new InvalidDataException($"Localization file is empty or invalid: {path}");

        if (document.Messages.Count == 0 || document.Buttons.Count == 0 || document.Topics.Count == 0)
        {
            throw new InvalidDataException($"Game resource file has no messages, buttons, or topics: {path}");
        }

        if (document.Topics.Any(topic => topic.Locations.Count == 0 || topic.Locations.Any(location => location.Roles.Count == 0)))
        {
            throw new InvalidDataException($"Game resource file contains an empty topic, location, or role list: {path}");
        }

        if (HasDuplicates(document.Topics.Select(topic => topic.Name)) ||
            HasDuplicates(document.Topics.SelectMany(topic => topic.Locations).Select(location => location.Name)) ||
            document.Topics.SelectMany(topic => topic.Locations).Any(location => HasDuplicates(location.Roles)))
        {
            throw new InvalidDataException($"Game resource file contains duplicate topics, locations, or roles: {path}");
        }
    }

    public string GetText(string key) => GetRequired(document.Messages, key, "message");

    public string GetButton(string key) => GetRequired(document.Buttons, key, "button");

    public string ChooseTopic() => document.Topics[Random.Shared.Next(document.Topics.Count)].Name;

    public (string Location, List<string> Roles) ChooseLocation(string topic)
    {
        var topicResource = document.Topics.SingleOrDefault(item => item.Name == topic)
            ?? throw new KeyNotFoundException($"Unknown topic '{topic}'.");
        var location = topicResource.Locations[Random.Shared.Next(topicResource.Locations.Count)];
        return (location.Name, new List<string>(location.Roles));
    }

    public (string Role, List<string> RemainingRoles) ChooseRole(List<string> rolesForLocation)
    {
        if (rolesForLocation.Count == 0)
        {
            throw new InvalidOperationException("No roles remain for this location.");
        }

        var index = Random.Shared.Next(rolesForLocation.Count);
        var role = rolesForLocation[index];
        rolesForLocation.RemoveAt(index);
        return (role, rolesForLocation);
    }

    internal IReadOnlyCollection<string> MessageKeys => document.Messages.Keys;

    internal IReadOnlyCollection<string> ButtonKeys => document.Buttons.Keys;

    internal IReadOnlyList<int> LocationCounts => document.Topics.Select(topic => topic.Locations.Count).ToArray();

    internal IReadOnlyList<int> RoleCounts => document.Topics.SelectMany(topic => topic.Locations).Select(location => location.Roles.Count).ToArray();

    private static string GetRequired(Dictionary<string, string> values, string key, string kind) =>
        values.TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException($"Missing localization {kind} key '{key}'.");

    private static bool HasDuplicates(IEnumerable<string> values)
    {
        var uniqueValues = new HashSet<string>(StringComparer.Ordinal);
        return values.Any(value => !uniqueValues.Add(value));
    }

    private sealed record LocalizationDocument(
        Dictionary<string, string> Messages,
        Dictionary<string, string> Buttons,
        List<TopicResource> Topics);

    private sealed record TopicResource(string Name, List<LocationResource> Locations);

    private sealed record LocationResource(string Name, List<string> Roles);
}
