using Knab.Cryptocurrency.Core.Common;
using Knab.Cryptocurrency.Infrastructure.Service;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Knab.Cryptocurrency.Infrastructure.Test.Common;
using Knab.Cryptocurrency.Infrastructure.Test.Fixtures;

namespace Knab.Cryptocurrency.Infrastructure.Test;

public class CoinMarketCapServiceTest : IClassFixture<GeneralSettingFixture>, IClassFixture<CoinMarketCapSettingFixture>
{
    private CoinMarketCapService? _sut;
    private readonly GeneralSettingFixture _generalSettingFixture;
    private readonly CoinMarketCapSettingFixture _apiSettingFixture;

    private GeneralSetting Setting => _generalSettingFixture.CurrentValue;
    private CoinMarketCapSetting ApiSetting => _apiSettingFixture.CurrentValue;
    private string CryptoCurrencySymbol = "BTC";

    public CoinMarketCapServiceTest(GeneralSettingFixture generalSettingFixture, CoinMarketCapSettingFixture apiSettingFixture)
    {
        _generalSettingFixture = generalSettingFixture;
        _apiSettingFixture = apiSettingFixture;
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync_Returns_ValidCachedResult_When_CacheIsFull()
    {
        //Arrange       

        _sut = new CoinMarketCapService(CoinMarketCapHttpClientFactoryMock.GetHttpClientFactoryWithNoClient(),
                                        _apiSettingFixture,
                                        _generalSettingFixture,
                                        CacheServiceMock.GetFullCache(Setting, CryptoCurrencySymbol));

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(CryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Success, result.StatusCode);
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync_Returns_ApiResult_When_CacheIsEmpty()
    {
        //Arrange       
        _sut = new CoinMarketCapService(CoinMarketCapHttpClientFactoryMock.GetHttpClientFactoryWithValidResponseContent(ApiSetting),
                                        _apiSettingFixture,
                                        _generalSettingFixture,
                                        CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(CryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Success, result.StatusCode);
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync_Returns_UnSuccess_When_InputIsNotValid()
    {
        //Arrange       
        _sut = new CoinMarketCapService(CoinMarketCapHttpClientFactoryMock.GetHttpClientFactoryWithNoClient(),
                                        _apiSettingFixture,
                                        _generalSettingFixture,
                                        CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(string.Empty);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.InvalidInput, result.StatusCode);
        Assert.Equal(410, result.ResponseCode);
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync_Returns_UnSuccess_When_ApiCallHasError()
    {
        //Arrange       
        _sut = new CoinMarketCapService(CoinMarketCapHttpClientFactoryMock.GetHttpClientFactoryWithError(ApiSetting),
                                        _apiSettingFixture,
                                        _generalSettingFixture,
                                        CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(CryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(411, result.ResponseCode);
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync_Returns_UnSuccess_When_ApiResponseIsNotSuccess()
    {
        //Arrange       
        _sut = new CoinMarketCapService(CoinMarketCapHttpClientFactoryMock.GetHttpClientFactoryWithUnsuccessfulResult(ApiSetting),
                                        _apiSettingFixture,
                                        _generalSettingFixture,
                                        CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(CryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(412, result.ResponseCode);
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync_Returns_UnSuccess_When_CurrencySymbolIsNotValid()
    {
        //Arrange       
        _sut = new CoinMarketCapService(CoinMarketCapHttpClientFactoryMock.GetHttpClientFactoryForInvalidCryptoCurrency(ApiSetting),
                                        _apiSettingFixture,
                                        _generalSettingFixture,
                                        CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(CryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(413, result.ResponseCode);
    }

    [Fact]
    public async Task GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync_Returns_UnSuccess_When_AnExceptionRaised()
    {
        //Arrange       
        _sut = new CoinMarketCapService(CoinMarketCapHttpClientFactoryMock.GetHttpClientFactoryWithNoClient(),
                                        _apiSettingFixture,
                                        _generalSettingFixture,
                                        CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCryptoCurrencyExchangeRatePerMajorCurrencyAsync(CryptoCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(510, result.ResponseCode);
    }
}