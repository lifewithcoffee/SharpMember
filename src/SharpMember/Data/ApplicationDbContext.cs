using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpMember.Data.Models;

namespace SharpMember.Data
{

    public class BaseDbContext : IdentityDbContext<ApplicationUser>
    {
        public BaseDbContext() { }
        public BaseDbContext(DbContextOptions options) : base(options) { }

        public DbSet<GlobalSettings> GlobalSettings { get; set; }
        public DbSet<Member> Members { get; set; }
    }

    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }

    public class SqliteDbContext : BaseDbContext
    {
        public SqliteDbContext() { }
        public SqliteDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Members.sqlitedb");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
