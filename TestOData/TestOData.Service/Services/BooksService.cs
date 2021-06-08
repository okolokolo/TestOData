using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestOData.Interfaces.DataAccess;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;

namespace TestOData.Service.Services
{
    internal class BooksService : IBooksService
    {
        private readonly ILoggerAdapter<BooksService> _logger;
        private readonly IBooksRepository _booksRepository;

        public BooksService
            (
                ILoggerAdapter<BooksService> logger,
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
                _logger.LogError("No book data found.", new LogItem<BooksService>());
                throw new NotFoundException("No book data found.");
            }

            return books;
        }

        public async Task<Book> GetBook(int key)
        {
            var book = await _booksRepository.GetBook(key).ConfigureAwait(false);
            
            if (book == null)
            {
                _logger.LogError($"No book data with {key} found.", new LogItem<BooksService>());

                throw new NotFoundException($"No book data with {key} found.");
            }

            return book;
        }

    }
}
