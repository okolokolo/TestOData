using System.Collections.Generic;
using System.Threading.Tasks;

using TestOData.Model;

namespace TestOData.Interfaces.Service
{
    public interface IBooksService
    {
        Task<Book> CreateBook(Book book);
        Task<Book> GetBook(int key);
        Task<IList<Book>> GetBooks();
    }
}
