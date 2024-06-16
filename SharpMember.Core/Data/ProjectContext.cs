using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Models.ProjectSystem;
using SharpMember.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMember.Core.Data;
public class ProjectContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<CheckListItem> CheckListItems { get; set; }
    public DbSet<TaskComment> TaskComments { get; set; }
    public DbSet<TaskLabel> TaskLabels { get; set; }
    public DbSet<TaskItem> WorkTasks { get; set; }
    public DbSet<WorkTaskLabelRelation> WorkTaskLabelRelations { get; set; }

    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
    {
        if (GlobalConfigs.DatabaseType == eDatabaseType.Sqlite)
            Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("task");

        builder.Entity<WorkTaskLabelRelation>().HasKey(m => new { m.WorkTaskId, m.TaskLabelId });
    }
}
