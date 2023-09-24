using Knab.Cryptocurrency.Core.Common;
using Knab.Cryptocurrency.Core.DomainServices;
using Knab.Cryptocurrency.Core.Entities;
using Knab.Cryptocurrency.Core.Services;
using Moq;

namespace Knab.Cryptocurrency.Test.Core;

public class DomainServiceTest
{
    private CryptoCurrencyExchangeDomainService? _sut;
    private string cryptoCurrencySymbol = "BTC";

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatesPerCurrencies_Returns_ValidList_When_AllServicesReturnSuccessfulResult()
    {
        //Arrange       
        var cryptoCurrencyExchangeService = new Mock<ICryptoCurrencyExchangeService>();

        cryptoCurrencyExchangeService
            .Setup(r => r.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(cryptoCurrencySymbol))
            .ReturnsAsync(() => new Response<CryptoCurrencyExchangeRate>
            {
                StatusCode = StatusCode.Success,
                Data = new CryptoCurrencyExchangeRate
                {
                    ExchangeRate = 2000.2000M,
                    CryptoCurrencyName = "BTC",
                    CryptoCurrencySymbol = "BitCoin",
                    MajorCurrencyName = "USD"
                }
            });

        var currencyExchangeService = new Mock<ICurrencyExchangeService>();

        currencyExchangeService
            .Setup(r => r.GetCurrencyExchangesPerMajorCurrencyAsync("USD"))
            .ReturnsAsync(() => new Response<List<CurrencyExchangeRate>>
            {
                StatusCode = StatusCode.Success,
                Data = new List<CurrencyExchangeRate>
                {
                    new() { MinorCurrency = "GBP", Rate = 2M },
                    new() { MinorCurrency = "EUR", Rate = 1.1M }
                }
            });

        _sut = new CryptoCurrencyExchangeDomainService(currencyExchangeService.Object,
                                                       cryptoCurrencyExchangeService.Object);

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatesPerCurrenciesAsync(cryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Success, result.StatusCode);
        Assert.Equal(3, result.Data.Quotas.Count);
        Assert.Equal(2000.2000M, result.Data.Quotas["USD"]);
        Assert.Equal(4000.4000M, result.Data.Quotas["GBP"]);
        Assert.Equal(2200.2200M, result.Data.Quotas["EUR"]);
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatesPerCurrencies_Returns_ErrorResult_When_CryptoCurrencyExchangeServiceReturnsError()
    {
        //Arrange       
        var cryptoCurrencyExchangeService = new Mock<ICryptoCurrencyExchangeService>();

        cryptoCurrencyExchangeService
            .Setup(r => r.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(cryptoCurrencySymbol))
            .ReturnsAsync(() => new Response<CryptoCurrencyExchangeRate>
            {
                StatusCode = StatusCode.Failed,
                ResponseCode = 411,
                ResponseDescription = ResponseCodes.GetDescription(411)
            });

        var currencyExchangeService = new Mock<ICurrencyExchangeService>();

        currencyExchangeService
            .Setup(r => r.GetCurrencyExchangesPerMajorCurrencyAsync(It.IsAny<string>()))
            .ReturnsAsync(() => new Response<List<CurrencyExchangeRate>>
            {
                StatusCode = StatusCode.Success,
                Data = new List<CurrencyExchangeRate>
                                           {
                                               new() { MinorCurrency = "GBP", Rate = 2M },
                                               new() { MinorCurrency = "EUR", Rate = 1.1M }
                                           }
            });

        _sut = new CryptoCurrencyExchangeDomainService(currencyExchangeService.Object,
                                                       cryptoCurrencyExchangeService.Object);

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatesPerCurrenciesAsync(cryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(411, result.ResponseCode);
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatesPerCurrencies_Returns_ErrorResult_When_CurrencyExchangeServiceReturnsError()
    {
        //Arrange       
        var cryptoCurrencyExchangeService = new Mock<ICryptoCurrencyExchangeService>();

        cryptoCurrencyExchangeService
            .Setup(r => r.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(cryptoCurrencySymbol))
            .ReturnsAsync(() => new Response<CryptoCurrencyExchangeRate>
            {
                StatusCode = StatusCode.Success,
                Data = new CryptoCurrencyExchangeRate
                {
                    ExchangeRate = 2000.2000M,
                    CryptoCurrencyName = "BTC",
                    CryptoCurrencySymbol = "BitCoin",
                    MajorCurrencyName = "USD"
                }
            });

        var currencyExchangeService = new Mock<ICurrencyExchangeService>();

        currencyExchangeService
            .Setup(r => r.GetCurrencyExchangesPerMajorCurrencyAsync("USD"))
            .ReturnsAsync(() => new Response<List<CurrencyExchangeRate>>
            {
                StatusCode = StatusCode.Failed,
                ResponseCode = 411
            });

        _sut = new CryptoCurrencyExchangeDomainService(currencyExchangeService.Object,
                                                       cryptoCurrencyExchangeService.Object);

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatesPerCurrenciesAsync(cryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(411, result.ResponseCode);
    }
}