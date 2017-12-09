using Interfaces.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        protected Context()
        {
        }

        public DbSet<TextFile> Files { get; set; }
    }
}
