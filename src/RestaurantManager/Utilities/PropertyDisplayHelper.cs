using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Utilities;

public static class PropertyDisplayHelper
{
    public static string GetDisplayName(Enum value)
    {
        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var displayAttr = member?.GetCustomAttributes(typeof(DisplayAttribute), false)
        .Cast<DisplayAttribute>()
        .FirstOrDefault();
        return displayAttr?.Name ?? value.ToString();
    }
}
