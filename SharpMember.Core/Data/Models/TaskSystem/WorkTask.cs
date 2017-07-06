using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.TaskSystem
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public bool Done { get; set; }  // display as "Open" and "Closed"
        public bool Pinned { get; set; }
        public string Status { get; set; }  // user input or select from a drop down menu
        public DateTime CreationTime { get; set; }
    }

    public class WorkTask : TaskEntity
    {
        public virtual List<CheckListItem> CheckListItems { get; set; } = new List<CheckListItem>();
        public virtual List<TaskComment> Comments { get; set; } = new List<TaskComment>();

        public virtual List<WorkTaskLabelRelation> WorkTaskLabelRelations { get; set; } = new List<WorkTaskLabelRelation>();

        public virtual Member WorkTaskCreator { get; set; }
        public virtual Member WorkTaskOwner { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }    // for user's private tasks

        public virtual Milestone Milestone { get; set; }
        public virtual Project Project { get; set; }
    }
}
