using BariukasApi.Shared;

namespace BariukasApi.WorksheetFeatures.GetWorksheet;

[QueryType]
public partial class GetWorksheetResolver(GetWorksheetHandler getWorksheetHandler)
{
    public async Task<WorksheetDocument> GetWorksheet(Guid id, CancellationToken ct)
    {
        return await getWorksheetHandler.HandleAsync(id, ct);
    }
}