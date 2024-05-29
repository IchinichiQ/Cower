using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Attributes;

public class DayOfWeekAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;
        
        if (!Enum.TryParse((string)value, out DayOfWeek day))
            return false;

        return true;
    }
}