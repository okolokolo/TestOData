using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestOData.Model;

namespace TestOData.DataAccess
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
             : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Press> Presses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().OwnsOne(c => c.Location);
        }

        public void SeedBooksIfEmpty()
        {
            if (Books.Any() == false)
            {
                foreach (var b in DataSource.GetBooks())
                {
                    Books.Add(b);
                    Presses.Add(b.Press);
                }
                SaveChanges();
            }
        }

    }
}
