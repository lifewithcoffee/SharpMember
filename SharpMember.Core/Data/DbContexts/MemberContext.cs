using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.DbContexts;
public class MemberContext : DbContext
{
    public DbSet<Member> Members { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; }
    public DbSet<MemberProfileItem> MemberProfileItems { get; set; }
    public DbSet<Community> Communities { get; set; }

    public MemberContext(DbContextOptions<MemberContext> options) : base(options)
    {
        if (GlobalConfigs.DatabaseType == eDatabaseType.Sqlite)
            Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("Member");

        //builder.Entity<ClubMemberRelation>().HasKey(c => new { c.ClubId, c.MemberId });
        //builder.Entity<WorkTaskLabelRelation>().HasKey(w => new { w.TaskLabelId, w.WorkTaskId });
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
