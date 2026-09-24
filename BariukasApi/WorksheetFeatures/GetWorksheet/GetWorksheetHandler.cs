using BariukasApi.Shared;
using BariukasApi.Shared.Extensions;
using MongoDB.Driver;

namespace BariukasApi.WorksheetFeatures.GetWorksheet;

public class GetWorksheetHandler(
    IMongoCollection<WorksheetDocument> worksheetCollection,
    ILogger<GetWorksheetHandler> logger)
{
    public async Task<FluentResults.Result<WorksheetDocument>> HandleAsync(Guid id, CancellationToken ct)
    {
        logger.LogInformation("Get Worksheet with Id: {id}", id);
        var doc = await worksheetCollection.Find(x => x.Id == id).FirstOrDefaultAsync(ct);
        if (doc == null)
        {
            logger.LogInformation("Worksheet with Id: {id} not found", id);
            return new FluentResults.Error("Worksheet not found")
                .WithMetadata("Id", id)
                .WithErrorCode(Shared.Constants.ErrorCodes.WORKSHEET_NOT_FOUND);
        }

        logger.LogInformation("Worksheet {id} successfully retrieved: {doc}", id, doc);
        return doc;
    }
}