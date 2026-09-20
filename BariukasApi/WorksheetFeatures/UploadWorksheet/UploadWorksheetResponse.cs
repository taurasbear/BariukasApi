namespace BariukasApi.WorksheetFeatures.UploadWorksheet;

public sealed record UploadWorksheetResponse
{
    public Guid WorksheetId { get; set; }

    public List<FluentResults.IError> Errors { get; set; } = [];
}