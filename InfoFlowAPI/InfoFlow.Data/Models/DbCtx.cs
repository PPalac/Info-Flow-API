using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbCtx>
    {
        public DbCtx CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DbCtx>();
            var connectionString = "Server=.;Database=Test;Integrated Security=SSPI;";
            builder.UseSqlServer(connectionString);
            return new DbCtx(builder.Options);
        }
    }
}
