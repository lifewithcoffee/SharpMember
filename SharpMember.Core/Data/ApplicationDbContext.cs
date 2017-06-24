using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Models;
using SharpMember.Global;
using System.Data.SqlClient;
using SharpMember.Utils;
using SharpMember.Core.Global;

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

    //public class SqlServerDbContext : BaseDbContext // renamed from the auto-generated ApplicationDbContext
    //{
    //    public SqlServerDbContext(string connectionString)
    //    : this(new DbContextOptionsBuilder<SqlServerDbContext>().UseSqlServer(new SqlConnection(connectionString)).Options)
    //    { }

    //    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options) { }
    //}

    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext()
        {
            Ensure.IsTrue(GlobalConfigs.DatabaseType == eDatabaseType.Sqlite);  // if this constructor is called, the database type must be sqlite
        }

        //public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public ApplicationDbContext(string connectionString): this(
            new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer( new SqlConnection(connectionString) )
            .Options )
        {
            Ensure.IsTrue(GlobalConfigs.DatabaseType == eDatabaseType.SqlServer);    // if this constructor is called, the database type must be sqlserver
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(GlobalConfigs.DatabaseType == eDatabaseType.Sqlite)
            {
                optionsBuilder.UseSqlite($"Filename={GlobalConsts.SqliteDbFileName}");
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
