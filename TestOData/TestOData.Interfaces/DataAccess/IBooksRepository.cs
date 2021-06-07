using System.Collections.Generic;
using System.Threading.Tasks;

using TestOData.Model;

namespace TestOData.Interfaces.DataAccess
{
    public interface IBooksRepository
    {
        Task<IList<Book>> GetBooks();
    }
}
