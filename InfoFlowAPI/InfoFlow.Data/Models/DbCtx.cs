using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfoFlow.Data.Models
{
    public class DbCtx : IdentityDbContext
    {
        public DbCtx(DbContextOptions options) : base(options)
        {
        }

        public new DbSet<User> Users { get; set; }
        public DbSet<RegisterLinkParameter> RegisterLinkParams { get; set; }
    }
}
