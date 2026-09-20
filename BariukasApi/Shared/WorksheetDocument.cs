namespace BariukasApi.Shared;

public class WorksheetDocument
{
    public Guid Id { get; set; }

    public ICollection<WorksheetRowDocument> Rows { get; set; } = [];
}