namespace Knab.Cryptocurrency.Infrastructure.Service.Dtos;

public class ExchangeRateResponseDto
{ 
    public bool Success { get; set; }
    public string Base { get; set; }
    public DateOnly Date { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }
}