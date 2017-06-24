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
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<GlobalSettings> GlobalSettings { get; set; }
        public DbSet<Member> Members { get; set; }  // TODO: need to be removed when member2 gets stabled
        public DbSet<MemberProfile> MemberProfiles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Branch> Branches { get; set; }

        private static DbContextOptions<ApplicationDbContext> GetOptionsFromConnectionString(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            if (GlobalConfigs.DatabaseType == eDatabaseType.Sqlite)
            {
                optionsBuilder.UseSqlite(connectionString);
            }
            else if (GlobalConfigs.DatabaseType == eDatabaseType.SqlServer)
            {
                optionsBuilder.UseSqlServer(new SqlConnection(connectionString));
            }

            return optionsBuilder.Options;
        }

        public ApplicationDbContext(string connectionString) : base(GetOptionsFromConnectionString(connectionString)) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
