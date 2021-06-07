using Microsoft.EntityFrameworkCore;
using RSM.Core.Logging.Extensions.Adapters;
using System.Collections.Generic;
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
    }
}
