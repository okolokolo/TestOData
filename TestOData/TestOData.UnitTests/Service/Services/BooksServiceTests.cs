using Moq;
using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestOData.Interfaces.DataAccess;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;
using TestOData.Service.Services;
using Xunit;

namespace TestOData.UnitTests.Service.Services
{
    public sealed class BooksServiceTests : IDisposable
    {
        private readonly MockRepository moq;
        private readonly Mock<IBooksRepository> _mockBooksRepository;
        private readonly Mock<ILoggerAdapter<BooksService>> _mockLogger;
        private readonly IBooksService sut;

        public BooksServiceTests()
        {
            moq = new MockRepository(MockBehavior.Strict);
            _mockBooksRepository = moq.Create<IBooksRepository>();
            _mockLogger = moq.Create<ILoggerAdapter<BooksService>>();

            sut = new BooksService(_mockLogger.Object, _mockBooksRepository.Object);
        }

        [Fact]
        public void GetBook_WhenBookDataNotFound_ThrowsNotFoundResult()
        {
            // Arrange
            _mockBooksRepository.Setup(r => r.GetBooks())
                .ReturnsAsync(new List<Book>());
            _mockLogger.Setup(l => l.LogError("No book data found.", It.IsAny<LogItem<BooksService>>()));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => sut.GetBooks());
        }

        [Fact]
        public async Task GetWeeklyForecast_WhenWeatherDataFound_ReturnsWeeklyForecast()
        {
            // Arrange
            var bookData = DataSource.GetBooks();
            _mockBooksRepository.Setup(r => r.GetBooks())
                .ReturnsAsync(bookData);

            // Act
            var result = await sut.GetBooks().ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        public void Dispose()
        {
            moq.VerifyAll();
        }
    }
}
