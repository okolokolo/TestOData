using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestOData.Interfaces.DataAccess;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;

namespace TestOData.Service.Services
{
    internal class BooksService : IBooksService
    {
        private readonly ILogger<BooksService> _logger;
        private readonly IBooksRepository _booksRepository;

        public BooksService
            (
                ILogger<BooksService> logger,
                IBooksRepository weatherRepository
            )
        {
            _logger = logger;
            _booksRepository = weatherRepository;
        }

        public async Task<IList<Book>> GetBooks()
        {
            var books = await _booksRepository.GetBooks().ConfigureAwait(false);

            if (books.Count == 0)
            {
                var ex = new NotFoundException("No book data found.");
                _logger.LogError(ex.Message);
                throw ex;
            }

            return books;
        }

        public async Task<Book> GetBook(int key)
        {
            var book = await _booksRepository.GetBook(key).ConfigureAwait(false);
            
            if (book == null)
            {
                var ex = new NotFoundException($"No book data with {key} found.");
                _logger.LogError(ex.Message);
                throw ex;
            }

            return book;
        }

        public async Task<Book> CreateBook(Book book)
        {
            return await _booksRepository.CreateBook(book);
        }

    }
}
