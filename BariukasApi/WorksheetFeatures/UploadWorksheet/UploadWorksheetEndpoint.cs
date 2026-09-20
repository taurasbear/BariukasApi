using System.Data;
using BariukasApi.Shared;
using BariukasApi.Shared.Extensions;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BariukasApi.WorksheetFeatures.UploadWorksheet;

public class UploadWorksheetEndpoint(
    IMongoCollection<WorksheetDocument> worksheetCollection,
    ILogger<UploadWorksheetEndpoint> logger) : IEndpoint
{
    private const string ERROR_REASON = "parsingFailure";

    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/worksheet",
            async ([FromForm] UploadWorksheetQuery request, CancellationToken ct) =>
            {
                var result = await Handle(request, ct);

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

    private async Task<FluentResults.Result<UploadWorksheetResponse>> Handle(UploadWorksheetQuery request,
        CancellationToken ct)
    {
        try
        {
            var errors = new List<FluentResults.IError>();
            var worksheetRowDocuments = new List<WorksheetRowDocument>();
            await using (var stream = request.Worksheet.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // TODO: figure out how to ignore header (top row) when creating reader
                    var result = reader.AsDataSet();

                    foreach (DataTable sheet in result.Tables)
                    foreach (DataRow row in sheet.Rows)
                    {
                        if (row.IsEmpty())
                        {
                            continue;
                        }

                        var worksheetRowResult = row.MapToDocument();
                        if (worksheetRowResult.IsFailed)
                        {
                            errors.AddRange(worksheetRowResult.Errors);
                        }
                        else
                        {
                            worksheetRowDocuments.Add(worksheetRowResult.Value);
                        }
                    }
                }
            }

            if (worksheetRowDocuments.Count == 0)
            {
                return FluentResults.Result.Fail("Uploaded worksheet had no parsable rows");
            }

            var worksheetDocument = new WorksheetDocument { Id = Guid.NewGuid(), Rows = worksheetRowDocuments };
            await worksheetCollection.InsertOneAsync(worksheetDocument, null, ct);

            var response = new UploadWorksheetResponse
            {
                WorksheetId = worksheetDocument.Id,
                Errors = errors
            };
            return FluentResults.Result.Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                ex.Message); // TODO: this will cause a 500.
            // But it's likely a problem with the request, not the server so maybe we should return an error "Uploaded file is not parsable - likely wrong format" or something like that?
            throw;
        }
    }
}