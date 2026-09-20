using BariukasApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BariukasApi.WorksheetFeatures.UploadWorksheet;

public class UploadWorksheetEndpoint(UploadWorksheetCommandHandler handler, ILogger<UploadWorksheetEndpoint> logger)
    : IEndpoint
{
    private const string ERROR_REASON = "parsingFailure";

    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/worksheet",
            async ([FromForm] UploadWorksheetCommand request, CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(request, ct);

                return result.IsFailed
                    ? Results.BadRequest(new UploadWorksheetErrorResponse
                    {
                        Message = result.Errors.FirstOrDefault()?.Message ??
                                  "Failed to parse worksheet", // TODO: should I write these in Lithuanian?
                        Reason = ERROR_REASON
                    })
                    : Results.Ok(result.Value);
            });
    }
}