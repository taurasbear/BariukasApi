namespace BariukasApi.Shared;

public record WorksheetDocument
{
    public Guid Id { get; set; }

    [GraphQLIgnore] public ICollection<WorksheetRowDocument> Rows { get; set; } = [];
}