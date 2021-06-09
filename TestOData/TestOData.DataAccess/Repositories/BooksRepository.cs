using Microsoft.EntityFrameworkCore;
using RSM.Core.Logging.Extensions.Adapters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestOData.Interfaces.DataAccess;
using TestOData.Model;

namespace TestOData.DataAccess.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly ILoggerAdapter<BooksRepository> _logger;
        private readonly BookStoreContext _context;

        public BooksRepository
            (
                ILoggerAdapter<BooksRepository> logger,
                BookStoreContext context
            )
        {

            context.SeedBooksIfEmpty();
            _logger = logger;
            _context = context;
        }

        public async Task<IList<Book>> GetBooks()
        {
            return await _context.Books.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Book> GetBook(int key)
        {
            var books = await GetBooks().ConfigureAwait(false);

            return books.Single(b => b.Id == key);
        }

        public async Task<Book> CreateBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
