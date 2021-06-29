using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestOData.Api.Controllers.v1;
using TestOData.Interfaces.Service;
using TestOData.Model;
using Xunit;

namespace TestOData.UnitTests.Api.Controllers.v1
{
    public sealed class BooksControllerTests : IDisposable
    {
        private readonly MockRepository _moq;
        private readonly Mock<ILogger<BooksController>> _mockLogger;
        private readonly Mock<IBooksService> _mockBookService;
        private readonly BooksController sut;

        public BooksControllerTests()
        {
            _moq = new MockRepository(MockBehavior.Strict);
            _mockLogger = _moq.Create<ILogger<BooksController>>();
            _mockBookService = _moq.Create<IBooksService>();

            sut = new BooksController(_mockLogger.Object, _mockBookService.Object);
        }

        [Fact]
        public async Task Get_WhenBookDataFound_ReturnsOKResponse()
        {
            // Arrange
            // _mockLogger.Setup(l => l.LogDebug("Received request.", It.IsAny<LogItem<BooksController>>()));

            var books = new List<Book> {
                new(){
                    Title = "The Laws of Success",
                    Author = "Napolean Hill"
                }
            };

            _mockBookService.Setup(s => s.GetBooks())
                .ReturnsAsync(books);

            // Act
            var result = await sut.Get().ConfigureAwait(false);

            // Assert
            Assert.Equal(typeof(OkObjectResult), result.GetType());
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);

            var data = (IList<Book>)((OkObjectResult)result).Value;

            var book = books.First();

            Assert.True(data.Any(d => d.Title == book.Title), "The book should exist");
        }

        public void Dispose()
        {
            _moq.VerifyAll();
        }
    }
}
