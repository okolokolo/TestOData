using Moq;
using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Get Books
        [Fact]
        public void GetBooks_WhenBookDataNotFound_ThrowsNotFoundResult()
        {
            // Arrange
            _mockBooksRepository.Setup(r => r.GetBooks())
                .ReturnsAsync(new List<Book>());
            _mockLogger.Setup(l => l.LogError("No book data found.", It.IsAny<LogItem<BooksService>>()));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => sut.GetBooks());
        }

        [Fact]
        public async Task GetBooks_WhenBookDataFound_ReturnsBookData()
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
        #endregion

        #region Get Book
        //TODO figure this out later
        //[Fact]
        //public void GetBook_WhenBookDataNotFound_ThrowsNotFoundResult()
        //{
        //    // Arrange
        //    int key = 0;

        //    _mockBooksRepository.Setup(r => r.GetBook(key))
        //        .ReturnsAsync(new Book());
        //    _mockLogger.Setup(l => l.LogError($"No book data with {key} found.", It.IsAny<LogItem<BooksService>>()));

        //    // Act & Assert
        //    Assert.ThrowsAsync<NotFoundException>(() => sut.GetBook(key));
        //}

        [Fact]
        public async Task GetBook_WhenBookDataFound_ReturnsBookData()
        {
            // Arrange
            int key = 1;

            var bookData = DataSource.GetBooks();
            //_mockBooksRepository.Setup(r => r.GetBooks())
            //    .ReturnsAsync(bookData);

            _mockBooksRepository.Setup(r => r.GetBook(1))
              .ReturnsAsync(bookData.First());

            // Act
            var result = await sut.GetBook(key).ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);

        }
        #endregion

        public void Dispose()
        {
            moq.VerifyAll();
        }
    }
}
