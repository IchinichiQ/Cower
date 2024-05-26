namespace Cower.Web.Models;

public class ErrorDto
{
    public Error Error { get; set; }

    public ErrorDto(string code, IReadOnlyCollection<string> details)
    {
        Error = new Error(code, details);
    }
    
    public ErrorDto(string code, string details)
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