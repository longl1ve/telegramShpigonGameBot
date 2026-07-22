namespace TelegramShpigonGameBot;

internal interface IGameContentProvider
{
    string ChooseTopic();
    (string Location, List<string> Roles) ChooseLocation(string topic);
    (string Role, List<string> RemainingRoles) ChooseRole(List<string> rolesForLocation);
}
