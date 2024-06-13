using SharpMember.Core.Definitions;
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

        // FKs
        public int CommunityId { get; set; }
        public int MemberId { get; set; }
        public string ApplicationUserId { get; set; }   // AspNetUser.Id
    }

    public class Project : ProjectEntity
    {
        public virtual List<Milestone> Milestones { get; set; }
        public virtual List<TaskItem> WorkTasks { get; set; }
    }
}
