namespace BariukasApi.Shared;

public class WorksheetRowDocument
{
    public string Customer { get; set; } = null!;

    public string ProductCode { get; set; } = null!;

    public int SoldQuantity { get; set; }

    public int ScannedQuantity { get; set; }
}