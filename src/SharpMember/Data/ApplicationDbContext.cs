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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<GlobalSettings>().HasKey(g => g.Name).HasName("PrimaryKey_Name");
        }
    }

    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // configure unique constraint for UserAdditionalInfo.MemberNumber
            // from: http://ef.readthedocs.io/en/latest/modeling/relational/unique-constraints.html
            builder.Entity<ApplicationUser>().HasAlternateKey(i => i.MemberNumber).HasName("AlternateKey_MemberNumber");
        }
    }

    public class SqliteDbContext : BaseDbContext
    {
        public SqliteDbContext() { }
        public SqliteDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Members.sqlitedb");
        }
    }
}
