using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.TaskSystem
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Project : ProjectEntity
    {
        public virtual List<Milestone> Milestones { get; set; }
        public virtual List<WorkTask> WorkTasks { get; set; }
    }
}
