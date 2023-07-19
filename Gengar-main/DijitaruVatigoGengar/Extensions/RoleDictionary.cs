using DijitaruVatigoGengar.Enums;

namespace DijitaruVatigoGengar.Extensions;

public static class RoleDictionary
{
    private static Dictionary<string, Role> map = new Dictionary<string, Role>
    {
        {"Normal", Role.Normal},
        {"Approver", Role.Approver},
        {"Admin", Role.Admin}
    };
    public static string RoleToString(this Role role)
    {
        return map.First(c => c.Value == role).Key;
    }

    public static Role RoleToValue(this string text)
    {
        return map.First(c => c.Key == text).Value;
    }
}
