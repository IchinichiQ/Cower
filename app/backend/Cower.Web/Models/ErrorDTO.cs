namespace Cower.Web.Models;

public class ErrorDTO
{
    public Error Error { get; set; }

    public ErrorDTO(string code, IReadOnlyCollection<string> details)
    {
        Error = new Error(code, details);
    }
    
    public ErrorDTO(string code, string details)
    {
        Error = new Error(code, new [] { details } );
    }
}

public class Error
{
    public string Code { get; set; }
    public IReadOnlyCollection<string> Details { get; set; }

    public Error(string code, IReadOnlyCollection<string> details)
    {
        Code = code;
        Details = details;
    }
}