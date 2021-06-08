﻿using System.Collections.Generic;
using System.Threading.Tasks;

using TestOData.Model;

namespace TestOData.Interfaces.DataAccess
{
    public interface IBooksRepository
    {
        Task<Book> GetBook(int key);
        Task<IList<Book>> GetBooks();
    }
}
