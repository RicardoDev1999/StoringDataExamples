using Microsoft.EntityFrameworkCore;
using StoringDataExamples.Data.Models;

namespace StoringDataExamples.Business.SQLStorage.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
    }
}
