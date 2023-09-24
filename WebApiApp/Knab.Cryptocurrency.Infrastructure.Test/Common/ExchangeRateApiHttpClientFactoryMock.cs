using Knab.Cryptocurrency.Infrastructure.Helper;
using Knab.Cryptocurrency.Infrastructure.Service.Dtos;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace Knab.Cryptocurrency.Infrastructure.Test.Common;

public static class ExchangeRateApiHttpClientFactoryMock
{
    public static IHttpClientFactory GetHttpClientFactoryWithNoClient() => new Mock<IHttpClientFactory>().Object;

    public static IHttpClientFactory GetHttpClientFactoryWithValidResponseContent(GeneralSetting setting,
        ExchangeRatesApiSetting apiSetting)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new ExchangeRateResponseDto
            {
                Success = true,
                Rates = GetResponseContent(setting)
            }))
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(apiSetting.BaseAddress)
        };
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory.Setup(_ => _.CreateClient(HttpClientConstants.ExchangeRateApisHttpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory.Object;
    }

    public static IHttpClientFactory GetHttpClientFactoryWithUnsuccessfulResult(ExchangeRatesApiSetting apiSetting)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new ExchangeRateResponseDto { Success = false }))
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(apiSetting.BaseAddress)
        };
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory.Setup(_ => _.CreateClient(HttpClientConstants.ExchangeRateApisHttpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory.Object;
    }

    public static IHttpClientFactory GetHttpClientFactoryWithError(ExchangeRatesApiSetting apiSetting)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(apiSetting.BaseAddress)
        };
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory.Setup(_ => _.CreateClient(HttpClientConstants.ExchangeRateApisHttpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory.Object;
    }

    public static IHttpClientFactory GetHttpClientFactoryForInvalidCurrency(ExchangeRatesApiSetting apiSetting)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new ExchangeRateResponseDto
            {
                Success = true,
                Rates = new Dictionary<string, decimal>()
            }))
        };
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri(apiSetting.BaseAddress)
        };
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory.Setup(_ => _.CreateClient(HttpClientConstants.ExchangeRateApisHttpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory.Object;
    }

    private static Dictionary<string, decimal> GetResponseContent(GeneralSetting setting)
    {

        var minorCurrencies = setting.MinorCurrencySymbols.Split(',').ToList();

        var resultDictionary = new Dictionary<string, decimal>();

        minorCurrencies.ForEach(c => { resultDictionary.Add(c, 2.2M); });

        return resultDictionary;
    }
}