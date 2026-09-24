namespace BariukasApi.Shared.Extensions;

public static class ErrorExtensions
{
    private const string CodeKey = "code";

    extension(FluentResults.Error error)
    {
        public FluentResults.Error WithErrorCode(string code)
        {
            return error.WithMetadata(CodeKey, code);
        }
    }

    extension(FluentResults.IError error)
    {
        public string GetErrorCode()
        {
            return error.Metadata
                .TryGetValue(CodeKey, out var errorCode)
                ? errorCode?.ToString() ?? Constants.ErrorCodes.UNKNOWN_ERROR
                : Constants.ErrorCodes.UNKNOWN_ERROR;
        }
    }
}