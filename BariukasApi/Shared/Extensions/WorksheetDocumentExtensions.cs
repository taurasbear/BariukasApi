using System.Data;
using Error = FluentResults.Error;

namespace BariukasApi.Shared.Extensions;

public static class WorksheetDocumentExtensions
{
    extension(DataRow row)
    {
        public FluentResults.Result<WorksheetRowDocument> MapToDocument()
        {
            // TODO: read this from in-memory instead
            const int customerColumnIndex = 0;
            const int productCodeColumnIndex = 1;
            const int soldQuantityColumnIndex = 2;

            var customer = row[customerColumnIndex].ToString();
            var productCode = row[productCodeColumnIndex].ToString();
            var soldQuantityRaw = row[soldQuantityColumnIndex].ToString();

            if (string.IsNullOrWhiteSpace(customer))
                return
                    new Error(
                        "Customer cannot be empty"); // TODO: these messages aren't specific enough. Row and column numbers would be beneficial

            if (string.IsNullOrWhiteSpace(productCode))
                return new Error("Product code cannot be empty");

            if (!int.TryParse(soldQuantityRaw, out var soldQuantity))
                return new Error("Sold quantity is not a number");

            return new WorksheetRowDocument
            {
                Customer = customer,
                ProductCode = productCode,
                SoldQuantity = soldQuantity,
                ScannedQuantity = 0
            };
        }
    }
}