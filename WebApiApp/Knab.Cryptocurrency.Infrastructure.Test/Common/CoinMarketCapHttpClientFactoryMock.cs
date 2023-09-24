using Knab.Cryptocurrency.Infrastructure.Helper;
using Knab.Cryptocurrency.Infrastructure.Service.Dtos;
using Knab.Cryptocurrency.Infrastructure.Settings;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace Knab.Cryptocurrency.Infrastructure.Test.Common;

public static class CoinMarketCapHttpClientFactoryMock
{
    public static IHttpClientFactory GetHttpClientFactoryWithNoClient() => new Mock<IHttpClientFactory>().Object;

    public static IHttpClientFactory GetHttpClientFactoryWithValidResponseContent(CoinMarketCapSetting apiSetting)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(GetResponseContent()))
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

        mockHttpClientFactory.Setup(_ => _.CreateClient(HttpClientConstants.CoinMarketCapHttpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory.Object;
    }

    public static IHttpClientFactory GetHttpClientFactoryWithUnsuccessfulResult(CoinMarketCapSetting apiSetting)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new CoinMarketCapResponseDto
                { Status = new Status { ErrorCode = 1 } }))
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

        mockHttpClientFactory.Setup(_ => _.CreateClient(HttpClientConstants.CoinMarketCapHttpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory.Object;
    }

    public static IHttpClientFactory GetHttpClientFactoryWithError(CoinMarketCapSetting apiSetting)
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

        mockHttpClientFactory.Setup(_ => _.CreateClient(HttpClientConstants.CoinMarketCapHttpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory.Object;
    }

    public static IHttpClientFactory GetHttpClientFactoryForInvalidCryptoCurrency(CoinMarketCapSetting apiSetting)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(GetResponseContentWithNoQuota()))
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

        mockHttpClientFactory.Setup(_ => _.CreateClient(HttpClientConstants.CoinMarketCapHttpClientName))
            .Returns(httpClient);

        return mockHttpClientFactory.Object;
    }

    private static CoinMarketCapResponseDto GetResponseContent() => new()
    {
        Status = new Status
        {
            ErrorCode = 0
        },
        Data = new Dictionary<string, DataItem[]>
        {
            {
                "BTC", new DataItem[]
                {
                    new()
                    {
                        Quote = new Dictionary<string, Quote>
                            { { "USD", new Quote { Price = 2_000.1M } } },
                        Symbol = "BTC",
                        Name = "BitCoin"
                    }
                }
            }
        }
    };

    private static CoinMarketCapResponseDto GetResponseContentWithNoQuota() => new()
    {
        Status = new Status
        {
            ErrorCode = 0
        },
        Data = new Dictionary<string, DataItem[]>
        {
            {
                "BTC", null

            }
        }
    };
}