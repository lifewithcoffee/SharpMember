using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharpMember.Core.Data.Models.TaskManagement
{
    public class TaskLabelEntity
    {
        [Key]
        public string Name { get; set; }
    }

    public class TaskLabel : TaskLabelEntity
    {
        public virtual List<WorkTaskLabelRelation> WorkTaskLabelRelations { get; set; } = new List<WorkTaskLabelRelation>();
    }
}
