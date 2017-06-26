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
        public DbSet<MemberProfile> MemberProfiles { get; set; }
        public DbSet<MemberProfileItem> MemberProfileItems { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CommunityEvent> CommunityEvents { get; set; }
        public DbSet<Club> Clubs { get; set; }

        private static DbContextOptions<ApplicationDbContext> GetOptionsFromConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) connectionString = $"Filename={GlobalConsts.SqliteDbFileName}";

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

        /**
         * The following constructor will cause mvc scaffolding to throw out exception of "Unable to resolve service for type 'System.String'"
         * when choose "MVC Controller with read/write actions"
         */
        //public ApplicationDbContext(string connectionString) : base(GetOptionsFromConnectionString(connectionString)) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
