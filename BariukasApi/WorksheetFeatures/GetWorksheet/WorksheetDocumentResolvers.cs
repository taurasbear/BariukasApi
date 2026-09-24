using System.Text.RegularExpressions;
using BariukasApi.Shared;
using BariukasApi.Shared.Constants;

namespace BariukasApi.WorksheetFeatures.GetWorksheet;

[ExtendObjectType<WorksheetDocument>]
public class WorksheetDocumentResolvers(ILogger<WorksheetDocumentResolvers> logger)
{
    public async Task<ICollection<WorksheetRowDocument>> GetRows(
        [Parent] WorksheetDocument worksheetDocument,
        GetRowsFilter filter)
    {
        var normalizedSubstrings = GetNormalizedCodeSubstrings(filter.Codes);

        var rows = worksheetDocument.Rows
            .Where(row => normalizedSubstrings.Contains(row.NormalizedProductCode))
            .ToList();

        logger.LogDebug("Codes matched rows: {rows}", rows);

        return rows;
    }

    // TODO: PaddleOCR returns separate lines/blocks, so we should get all substrings of each line, get rid of whitespaces
    // TODO: noticed that sometimes it mixes up 0 with O and around, so maybe I should do something about that
    private List<string> GetNormalizedCodeSubstrings(List<string> codes)
    {
        var normalizedCodes = codes
            .Select(code =>
                Regex.Replace(code, RegexPatterns.NON_LETTERS_AND_NUMBERS, "")
                    .ToUpper())
            .ToList();

        var substrings = new List<string>();
        foreach (var code in normalizedCodes)
        {
            for (var i = 1; i <= code.Length; i++)
            {
                for (var j = 0; j < code.Length - i + 1; j++)
                {
                    var substring = code.Substring(j, i);
                    substrings.Add(substring);
                }
            }
        }

        // TODO: remove this after validating method works
        logger.LogDebug("Got substrings: {substrings} from {codes}", substrings, codes);

        return substrings;
    }
}