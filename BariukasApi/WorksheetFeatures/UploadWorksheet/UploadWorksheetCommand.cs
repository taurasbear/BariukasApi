namespace BariukasApi.WorksheetFeatures.UploadWorksheet;

public sealed record
    UploadWorksheetCommand
{
    public required IFormFile Worksheet { get; set; }
}