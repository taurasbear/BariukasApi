using BariukasApi.Shared;
using BariukasApi.Shared.Extensions;

namespace BariukasApi.WorksheetFeatures.IncrementScannedCount;

[MutationType]
public class IncrementScannedCountResolver(IncrementScannedCountHandler handler)
{
    public async Task<WorksheetRowDocument> IncrementScannedCount(
        Guid worksheetId,
        Guid rowId,
        int incrementCount,
        CancellationToken ct
    )
    {
        var result = await handler.HandleAsync(worksheetId, rowId, incrementCount, ct);

        if (result.IsSuccess)
        {
            return result.Value;
        }

        // TODO: make this reusable
        var error = result.Errors.First();
        throw new GraphQLException(
            ErrorBuilder
                .New()
                .SetMessage(error.Message)
                .SetCode(error.GetErrorCode())
                .SetExtension("metadata", error.Metadata)
                .Build()
        );
    }
}