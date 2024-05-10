using System.ComponentModel.DataAnnotations;
using Cower.Web.Models;

namespace Cower.Web.Helpers;

public static class ValidationHelper
{
    public static ErrorDTO? Validate(object? obj)
    {
        if (obj == null)
        {
            return new ErrorDTO(
                ErrorCodes.INVALID_REQUEST_DATA,
                "Некорректное тело запроса");
        }
        
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(obj, null, null);
        Validator.TryValidateObject(obj, validationContext, validationResults, true);

        var errors = new List<string>();
        foreach (var validationResult in validationResults)
        {
            if (validationResult.ErrorMessage != null)
            {
                errors.Add(validationResult.ErrorMessage);
            }
        }


        ErrorDTO? dto = null;
        if (errors.Any())
        {
            dto = new ErrorDTO(
                ErrorCodes.INVALID_REQUEST_DATA,
                errors);
        }

        return dto;
    }
}