using System.Data;
using System.Text.RegularExpressions;
using BariukasApi.Shared.Constants;
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
            {
                return
                    new Error(
                        "Customer cannot be empty"); // TODO: these messages aren't specific enough. Row and column numbers would be beneficial. Or maybe the code calling this method should add info about row and column
            }

            if (string.IsNullOrWhiteSpace(productCode))
            {
                return new Error("Product code cannot be empty");
            }

            if (!int.TryParse(soldQuantityRaw, out var soldQuantity))
            {
                return new Error("Sold quantity is not a number");
            }

            var normalizedProductCode =
                Regex.Replace(productCode, RegexPatterns.NON_LETTERS_AND_NUMBERS, "").ToUpper();

            if (string.IsNullOrWhiteSpace(normalizedProductCode))
            {
                return new Error("Product code must contain letters or numbers");
            }

            return new WorksheetRowDocument
            {
                Customer = customer,
                ProductCode = productCode,
                NormalizedProductCode = normalizedProductCode,
                SoldQuantity = soldQuantity,
                ScannedQuantity = 0
            };
        }
    }
}