using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Definitions;
using System.Data.SqlClient;
using SharpMember.Utils;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Models.ActivitySystem;
using SharpMember.Core.Data.Models.TaskSystem;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SharpMember.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<GlobalSettings> GlobalSettings { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; }
        public DbSet<MemberProfileItem> MemberProfileItems { get; set; }
        public DbSet<Community> Communities { get; set; }

        /** The following constructor will cause mvc scaffolding to throw out an exception of
         *      Unable to resolve service for type 'System.String'
         * when choose "MVC Controller with read/write actions"
         */
        //public ApplicationDbContext(string connectionString) : base(GetOptionsFromConnectionString(connectionString)) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            if (GlobalConfigs.DatabaseType == eDatabaseType.Sqlite)
                Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ClubMemberRelation>().HasKey(c => new { c.ClubId, c.MemberId });
            //builder.Entity<WorkTaskLabelRelation>().HasKey(w => new { w.TaskLabelId, w.WorkTaskId });
            builder.Entity<GroupMemberRelation>().HasKey(m => new { m.MemberId, m.GroupId });
            builder.Entity<GroupMemberRelation>().HasKey(m => new { m.MemberId, m.GroupId });

            /**
             * Disable cascade deletion to avoid 2 cascade deletion pathes, for:
             * 
             *     Community -> Groups -> GroupMemberRelation
             *     Community -> Member -> GroupMemberRelation
             *     
             * and:
             * 
             *      Community -> Member -> MemberProfileItem
             *      Community -> MemberProfileItemTemplate -> MemberProfileItem
             *      
             * which will cause an exception on update-database in the DB migration.
             */
            builder.Entity<Community>().HasMany(c => c.Members).WithOne(m => m.Community).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
