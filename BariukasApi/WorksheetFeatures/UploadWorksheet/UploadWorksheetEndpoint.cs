using BariukasApi.Shared;
using BariukasApi.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BariukasApi.WorksheetFeatures.UploadWorksheet;

public class UploadWorksheetEndpoint(ILogger<UploadWorksheetEndpoint> logger)
    : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/worksheet",
            async ([FromForm] UploadWorksheetCommand request, UploadWorksheetCommandHandler handler,
                CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(request, ct);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }

                var error = result.Errors.First();

                return Results.BadRequest(new UploadWorksheetErrorResponse
                {
                    Message = error.Message,
                    Reason = error.GetErrorCode()
                });
            });
    }
}