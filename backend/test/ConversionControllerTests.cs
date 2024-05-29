using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using service;
using api.Controllers;

/*namespace test
{
    public class ConversionControllerTests
    {
        [Fact]
        public void ConvertCurrency_ShouldReturnOk_WhenConversionSucceeds()
        {
            // Arrange
            var request = new DataRecordController.ConversionRequest
            {
                Amount = 100,
                FromCurrency = "USD",
                ToCurrency = "EUR"
            };

            var mockService = new Mock<PlantService>();
            mockService.Setup(service => service.ConvertCurrency(request.Amount, request.FromCurrency, request.ToCurrency))
                       .Returns(93); 

            var controller = new DataRecordController(mockService.Object);

            // Act
            var result = controller.ConvertCurrency(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(100, ((dynamic)okResult.Value).Amount);
            Assert.Equal("USD", ((dynamic)okResult.Value).FromCurrency);
            Assert.Equal(93, ((dynamic)okResult.Value).ConvertedAmount);
            Assert.Equal("EUR", ((dynamic)okResult.Value).ToCurrency);
        }

        [Fact]
        public void ConvertCurrency_ShouldReturnBadRequest_WhenConversionFails()
        {
            // Arrange
            var request = new DataRecordController.ConversionRequest
            {
                Amount = 100,
                FromCurrency = "USD",
                ToCurrency = "EUR"
            };

            var mockService = new Mock<PlantService>();
            mockService.Setup(service => service.ConvertCurrency(request.Amount, request.FromCurrency, request.ToCurrency))
                       .Throws(new ArgumentException("Unsupported currency")); // Simula una excepción

            var controller = new DataRecordController(mockService.Object);

            // Act
            var result = controller.ConvertCurrency(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetConversionHistory_ShouldReturnOk_WhenHistoryIsAvailable()
        {
            // Arrange
            var mockService = new Mock<PlantService>();
            var conversionRecords = new List<infrastructure.ConversionRecord> // Corregir aquí
            {
                new() { Id = 1, FromCurrency = "USD", ToCurrency = "EUR", Amount = 100, ConvertedAmount = 93, Date = DateTime.Now },
                new() { Id = 2, FromCurrency = "EUR", ToCurrency = "USD", Amount = 93, ConvertedAmount = 100, Date = DateTime.Now }
                
            };
            mockService.Setup(service => service.GetConversionHistory())
                .Returns(conversionRecords);

            var controller = new DataRecordController(mockService.Object);

            // Act
            var result = controller.GetConversionHistory();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(conversionRecords, okResult.Value);
        }


        [Fact]
        public void GetConversionHistory_ShouldReturnInternalServerError_WhenHistoryFetchFails()
        {
            // Arrange
            var mockService = new Mock<PlantService>();
            mockService.Setup(service => service.GetConversionHistory())
                       .Throws(new Exception("Test exception"));

            var controller = new DataRecordController(mockService.Object);

            // Act
            var result = controller.GetConversionHistory();

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.NotNull(statusCodeResult);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
*/