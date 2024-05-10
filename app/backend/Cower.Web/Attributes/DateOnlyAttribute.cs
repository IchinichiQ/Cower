using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Cower.Web.Attributes;

public class DateOnlyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;
        
        if (!DateOnly.TryParse((string)value, out DateOnly dateTime))
            return false;

        return true;
    }
}