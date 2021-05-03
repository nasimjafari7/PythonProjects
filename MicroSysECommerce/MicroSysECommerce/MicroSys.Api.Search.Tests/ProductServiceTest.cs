using System;
using Xunit;
using Moq;
using System.Net.Http;
using Moq.Protected;
using System.Threading.Tasks;
using System.Net;
using MicroSys.Api.Search.Services;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace MicroSys.Api.Search.Tests
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task SearchAsyncTest()
        {
            //Arrange
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'Id' : 1, 'Name' : 'Keyboard'}]"),
                });
            

            var client = new HttpClient(mockHttpMessageHandler.Object);
            client.BaseAddress = new Uri("http://localhost:47538/api/products");
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);            

            var productService = new ProductService(mockFactory.Object, null);

            //Act
            var result = await productService.GetProductsAsync();

            //Assert
            Assert.True(result.IsSuccess); ;
            Assert.True(result.Products.Any());
            Assert.Null(result.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(ValidateTestData))]
        public async Task SearchAsyncWithDifferentInputTest(string content, bool expectedIsSuccess, int expectedProductsCount, string expectedErrorMessage)
        {
            //Arrange
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content),
                });


            var client = new HttpClient(mockHttpMessageHandler.Object);
            client.BaseAddress = new Uri("http://localhost:47538/api/products");
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);


            var productService = new ProductService(mockFactory.Object, null);

            //Act
            var result = await productService.GetProductsAsync();

            //Assert
            Assert.Equal(expectedIsSuccess, result.IsSuccess);
            Assert.Equal(expectedProductsCount, result.Products.Count());
            Assert.Equal(expectedErrorMessage, result.ErrorMessage);
        }

        public static IEnumerable<object[]> ValidateTestData =>
        new List<object[]>
        {
            new object[] { "[{'Id' : 1, 'Name' : 'Keyboard'}]", true, 1, null },
            new object[] { "[]", true, 0, null },
        };
    }
}
