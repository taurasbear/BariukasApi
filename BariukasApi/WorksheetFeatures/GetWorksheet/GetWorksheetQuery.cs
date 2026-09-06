using BariukasApi.Shared;

namespace BariukasApi.WorksheetFeatures.GetWorksheet;

[QueryType]
public partial class GetWorksheetQuery
{
    public static WorksheetDocument GetWorksheet(int id) => new WorksheetDocument
    {
        Customer = id.ToString(), 
        ProductCode = id.ToString(),
        SoldQuantity = 2,
        ScannedQuantity = 1,
        SellingPrice = 5.5m,
        SupplierPrice = 5.5m
    };
}