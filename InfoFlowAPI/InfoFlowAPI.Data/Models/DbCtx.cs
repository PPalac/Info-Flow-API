using Microsoft.EntityFrameworkCore;

namespace InfoFlowAPI.Data.Models
{
    public class DbCtx : DbContext
    {
        public DbCtx(DbContextOptions options) : base(options)
        {
        }
    }
}
