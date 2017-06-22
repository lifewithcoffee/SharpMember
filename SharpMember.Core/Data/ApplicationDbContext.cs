using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Models;
using SharpMember.Global;

namespace SharpMember.Core.Data
{

    public class BaseDbContext : IdentityDbContext<ApplicationUser>
    {
        public BaseDbContext() { }
        public BaseDbContext(DbContextOptions options) : base(options) { }

        public DbSet<GlobalSettings> GlobalSettings { get; set; }
        public DbSet<Member> Members { get; set; }  // TODO: need to be removed when member2 gets stabled
        public DbSet<MemberProfile> MemberProfiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Branch> Branches { get; set; }
    }

    public class SqlServerDbContext : BaseDbContext // renamed from the auto-generated ApplicationDbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options) { }
    }

    public class SqliteDbContext : BaseDbContext
    {
        public SqliteDbContext() { }
        public SqliteDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={GlobalConsts.SqliteDbFileName}");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
