namespace BariukasApi.Shared;

public interface IErrorResponse
{
    public string Reason { get; set; }

    public string Message { get; set; }

    public object? Params { get; set; } // TODO: think whether this could be more specific
}