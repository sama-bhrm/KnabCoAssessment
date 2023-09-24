using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Knab.Cryptocurrency.Infrastructure.Service.Dtos;

public class CoinMarketCapResponseDto
{
    public Status Status { get; set; }
    public Dictionary<string, DataItem[]>? Data { get; set; }
}

public class Status
{
    [JsonPropertyName("error_code")]
    public int ErrorCode  { get; set; }
    [JsonPropertyName("error_message")]
    public object ErrorMessage { get; set; }
}

public class DataItem
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public Dictionary<string, Quote> Quote { get; set; }
}

public class Quote
{
    public decimal Price { get; set; }
}