using BariukasApi.Shared;
using BariukasApi.Shared.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Error = FluentResults.Error;

namespace BariukasApi.WorksheetFeatures.IncrementScannedCount;

public class IncrementScannedCountHandler(IMongoCollection<WorksheetDocument> worksheetCollection)
{
    public async Task<FluentResults.Result<WorksheetRowDocument>> HandleAsync(
        Guid worksheetId,
        Guid rowId,
        int incrementCount,
        CancellationToken ct
    )
    {
        var filter = Builders<WorksheetDocument>.Filter.Eq(worksheet => worksheet.Id, worksheetId) &
                     Builders<WorksheetDocument>.Filter.ElemMatch(worksheet => worksheet.Rows, row => row.Id == rowId);

        var increment =
            Builders<WorksheetDocument>.Update.Inc(
                worksheet => worksheet.Rows.FirstMatchingElement().ScannedQuantity, incrementCount);

        var result = await worksheetCollection.UpdateOneAsync(filter, increment, null, ct);

        if (result.ModifiedCount <= 0)
        {
            return new Error("Worksheet not found")
                .WithMetadata("Id", worksheetId)
                .WithErrorCode(Shared.Constants.ErrorCodes.WORKSHEET_NOT_FOUND);
        }

        var worksheetRow = await worksheetCollection
            .Find(worksheet => worksheet.Id == worksheetId)
            .Project(worksheet => worksheet.Rows.First(row => row.Id == rowId))
            .FirstOrDefaultAsync(ct);

        return FluentResults.Result.Ok(worksheetRow);
    }
}