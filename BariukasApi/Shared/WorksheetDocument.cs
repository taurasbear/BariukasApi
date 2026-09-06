namespace BariukasApi.Shared;

public class WorksheetDocument
{
    public string Customer { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public int SoldQuantity { get; set; }

    public int ScannedQuantity { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal SupplierPrice { get; set; }
}