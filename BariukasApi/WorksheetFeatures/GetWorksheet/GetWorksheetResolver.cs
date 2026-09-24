using BariukasApi.Shared;
using BariukasApi.Shared.Extensions;

namespace BariukasApi.WorksheetFeatures.GetWorksheet;

[QueryType]
public partial class GetWorksheetResolver(GetWorksheetHandler handler)
{
    public async Task<WorksheetDocument> GetWorksheet(Guid id, CancellationToken ct)
    {
        var result = await handler.HandleAsync(id, ct);
        if (result.IsSuccess)
        {
            return result.Value;
        }

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