using System.Collections.Generic;
using System.Threading.Tasks;

using TestOData.Model;

namespace TestOData.Interfaces.Service
{
    public interface IBooksService
    {
        Task<IList<Book>> GetBooks();
    }
}
