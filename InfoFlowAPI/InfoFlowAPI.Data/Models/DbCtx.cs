﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfoFlowAPI.Data.Models
{
    public class DbCtx : IdentityDbContext
    {
        public DbCtx(DbContextOptions options) : base(options)
        {
        }

        new DbSet<User> Users { get; set; }
    }
}
