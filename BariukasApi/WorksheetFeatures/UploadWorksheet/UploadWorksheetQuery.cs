namespace BariukasApi.WorksheetFeatures.UploadWorksheet;

public sealed record
    UploadWorksheetQuery // TODO: this clashes with the GraphQL query term (e.g. `GetWorksheetQuery`). I should figure out how to rename. Maybe request is enough but I wanna stick to CQRS
{
    public required IFormFile Worksheet { get; set; }
}