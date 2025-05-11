using System.IO;
using Microsoft.EntityFrameworkCore;
using SiemensInternship.Model;

namespace SiemensInternship.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var projectRoot = Directory.GetParent(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            var dbPath = Path.Combine(projectRoot, "library.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
