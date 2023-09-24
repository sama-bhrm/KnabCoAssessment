using Knab.Cryptocurrency.Core.Common;
using Knab.Cryptocurrency.Infrastructure.Service;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Knab.Cryptocurrency.Infrastructure.Test.Common;
using Knab.Cryptocurrency.Infrastructure.Test.Fixtures;

namespace Knab.Cryptocurrency.Infrastructure.Test;

public class ApiLayerExchangeRatesServiceTest : IClassFixture<GeneralSettingFixture>, IClassFixture<ApiLayerExchangeRatesApiSettingFixture>
{
    private ApiLayerExchangeRatesService? _sut;
    private readonly GeneralSettingFixture _generalSettingFixture;
    private readonly ApiLayerExchangeRatesApiSettingFixture _apiSettingFixture;

    private GeneralSetting Setting => _generalSettingFixture.CurrentValue;
    private ApiLayerExchangeRatesApiSetting ApiSetting => _apiSettingFixture.CurrentValue;

    public ApiLayerExchangeRatesServiceTest(GeneralSettingFixture generalSettingFixture, ApiLayerExchangeRatesApiSettingFixture apiLayerExchangeRatesApiSettingFixture)
    {
        _generalSettingFixture = generalSettingFixture;
        _apiSettingFixture = apiLayerExchangeRatesApiSettingFixture;
    }

    [Fact]
    public async Task GetCurrencyExchangesPerMajorCurrencyAsync_Returns_ValidCachedResult_When_CacheIsFull()
    {
        //Arrange       
        _sut = new ApiLayerExchangeRatesService(ApiLayerHttpClientFactoryMock.GetHttpClientFactoryWithNoClient(),
                                                _apiSettingFixture,
                                                _generalSettingFixture,
                                                CacheServiceMock.GetFullCache(Setting));

        //Act
        var result = await _sut.GetCurrencyExchangesPerMajorCurrencyAsync(_generalSettingFixture.CurrentValue.MajorCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Success, result.StatusCode);
    }

    [Fact]
    public async Task GetCurrencyExchangesPerMajorCurrencyAsync_Returns_ApiResult_When_CacheIsEmpty()
    {
        //Arrange       
        _sut = new ApiLayerExchangeRatesService(ApiLayerHttpClientFactoryMock.GetHttpClientFactoryWithValidResponseContent(Setting, ApiSetting),
                                                _apiSettingFixture,
                                                _generalSettingFixture,
                                                CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCurrencyExchangesPerMajorCurrencyAsync(_generalSettingFixture.CurrentValue.MajorCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Success, result.StatusCode);
    }

    [Fact]
    public async Task GetCurrencyExchangesPerMajorCurrencyAsync_Returns_UnSuccess_When_InputIsNotValid()
    {
        //Arrange       
        _sut = new ApiLayerExchangeRatesService(ApiLayerHttpClientFactoryMock.GetHttpClientFactoryWithNoClient(),
                                                _apiSettingFixture,
                                                _generalSettingFixture,
                                                CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCurrencyExchangesPerMajorCurrencyAsync(string.Empty);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.InvalidInput, result.StatusCode);
        Assert.Equal(410, result.ResponseCode);
    }

    [Fact]
    public async Task GetCurrencyExchangesPerMajorCurrencyAsync_Returns_UnSuccess_When_ApiCallHasError()
    {
        //Arrange       
        _sut = new ApiLayerExchangeRatesService(ApiLayerHttpClientFactoryMock.GetHttpClientFactoryWithError(ApiSetting),
                                                _apiSettingFixture,
                                                _generalSettingFixture,
                                                CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCurrencyExchangesPerMajorCurrencyAsync(Setting.MajorCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(411, result.ResponseCode);
    }

    [Fact]
    public async Task GetCurrencyExchangesPerMajorCurrencyAsync_Returns_UnSuccess_When_ApiResponseIsNotSuccess()
    {
        //Arrange       
        _sut = new ApiLayerExchangeRatesService(ApiLayerHttpClientFactoryMock.GetHttpClientFactoryWithUnsuccessfulResult(ApiSetting),
                                                _apiSettingFixture,
                                                _generalSettingFixture,
                                                CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCurrencyExchangesPerMajorCurrencyAsync(Setting.MajorCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(412, result.ResponseCode);
    }

    [Fact]
    public async Task GetCurrencyExchangesPerMajorCurrencyAsync_Returns_UnSuccess_When_CurrencySymbolIsNotValid()
    {
        //Arrange       
        _sut = new ApiLayerExchangeRatesService(ApiLayerHttpClientFactoryMock.GetHttpClientFactoryForInvalidCurrency(ApiSetting),
                                                _apiSettingFixture,
                                                _generalSettingFixture,
                                                CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCurrencyExchangesPerMajorCurrencyAsync(Setting.MajorCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(414, result.ResponseCode);
    }
    [Fact]
    public async Task GetCurrencyExchangesPerMajorCurrencyAsync_Returns_UnSuccess_When_AnExceptionRaised()
    {
        //Arrange       
        _sut = new ApiLayerExchangeRatesService(ApiLayerHttpClientFactoryMock.GetHttpClientFactoryWithNoClient(),
                                                _apiSettingFixture,
                                                _generalSettingFixture,
                                                CacheServiceMock.GetEmptyCache());

        //Act
        var result = await _sut.GetCurrencyExchangesPerMajorCurrencyAsync(Setting.MajorCurrencySymbol);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCode.Failed, result.StatusCode);
        Assert.Equal(510, result.ResponseCode);
    }
}