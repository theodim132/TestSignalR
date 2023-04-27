using Microsoft.EntityFrameworkCore;
using TestSignalR.Models;

namespace TestSignalR.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
    }
}
