namespace Knab.Cryptocurrency.Core.Common;

public static class ResponseCodes
{
    public static int SuccessCode = 20;

    public static Dictionary<int, string?> ResponseDictionary = new()
    {
        { SuccessCode, null },
        { 410, "Invalid Input." },
        { 411, "Api call had an error." },
        { 412, "Api Response was not successful." },
        { 413, "No exchange rate data was found for expected CryptoCurrency." },
        { 414, "No exchange rate data was found for expected Currency." },
        { 415, "Api Key Missing." },
        { 416, "Invalid Api Key." },
        { 510, "An exception raised during process." },
        { 511, "An HttpRequestException raised during process." },
        { 512, "Result status has not been implemented" }
    };

    public static string GetDescription(int key, string? additionalInfo = null)
        => additionalInfo is null
            ? $"{ResponseDictionary.FirstOrDefault(x => x.Key == key).Value}"
            : $"{ResponseDictionary.FirstOrDefault(x => x.Key == key).Value} AdditionalInfo: {additionalInfo}";
}