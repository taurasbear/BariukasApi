namespace BariukasApi.Shared;

public record WorksheetDocument
{
    public Guid Id { get; set; }

    public ICollection<WorksheetRowDocument> Rows { get; set; } = [];
}