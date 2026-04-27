using Microsoft.EntityFrameworkCore;

namespace Portfolio.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContactViewModel> Contacts { get; set; }

    }
}
