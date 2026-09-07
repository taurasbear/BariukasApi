using BariukasApi.Shared;
using MongoDB.Driver;

namespace BariukasApi.WorksheetFeatures.GetWorksheet;

public class GetWorksheetHandler(
    IMongoCollection<WorksheetDocument> worksheetCollection,
    ILogger<GetWorksheetHandler> logger)
{
    public async Task<WorksheetDocument> HandleAsync(Guid id, CancellationToken ct)
    {
        logger.LogInformation("Get Worksheet Document {id}", id);
        try
        {
            var doc = await worksheetCollection.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
            logger.LogInformation("GetWorksheetHandler: Worksheet {id} successfully retrieved: {doc}", id, doc);
            return doc;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }

        return null;
        // This overfetches the DB but GraphQL will only return the things asked for. In our case, it's not worth optimising this
    }
}