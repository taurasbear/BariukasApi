using System.Data;

namespace BariukasApi.Shared.Extensions;

public static class DataRowExtensions
{
    extension(DataRow row)
    {
        public bool IsEmpty()
        {
            return row.ItemArray.All(cell => string.IsNullOrWhiteSpace(cell?.ToString()));
        }
    }
}