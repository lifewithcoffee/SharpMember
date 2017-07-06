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
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Models.ActivitySystem;
using SharpMember.Core.Data.Models.TaskSystem;

namespace SharpMember.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<GlobalSettings> GlobalSettings { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberProfileItem> MemberProfileItems { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<ClubEvent> CommunityEvents { get; set; }
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ClubMemberRelation>().HasKey(c => new { c.ClubId, c.MemberId });
            builder.Entity<WorkTaskLabelRelation>().HasKey(w => new { w.TaskLabelId, w.WorkTaskId });

            base.OnModelCreating(builder);
        }
    }
}
