using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Attributes;

public class TimeOnlyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;
        
        if (!TimeOnly.TryParse((string)value, out TimeOnly timeOnly))
            return false;

        return true;
    }
}