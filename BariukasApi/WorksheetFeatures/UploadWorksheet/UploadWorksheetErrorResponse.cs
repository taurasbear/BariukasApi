using BariukasApi.Shared;

namespace BariukasApi.WorksheetFeatures.UploadWorksheet;

public class UploadWorksheetErrorResponse : IErrorResponse
{
    public string Reason { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public object? Params { get; set; }
}