using BariukasApi.Shared;

namespace BariukasApi.WorksheetFeatures.GetWorksheet;

[QueryType]
public partial class GetWorksheetResolver(GetWorksheetHandler handler)
{
    public async Task<WorksheetDocument> GetWorksheet(Guid id, CancellationToken ct)
    {
        return await handler.HandleAsync(id, ct);
    }
}