using System;
using backend_template.Model;
using Microsoft.EntityFrameworkCore;

namespace backend_template.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
    }
}

