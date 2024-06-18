using System.ComponentModel.DataAnnotations;

namespace Cower.Web.Attributes;

public class RequiredEnum : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }

        var type = value.GetType();
        if (!(type.IsEnum && Enum.IsDefined(type, value)))
        {
            return false;
        }

        return true;
    }
}