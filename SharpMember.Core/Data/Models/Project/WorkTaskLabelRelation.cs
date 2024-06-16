using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.Project
{
    public class WorkTaskLabelRelation
    {
        public int WorkTaskId { get; set; }
        public virtual TaskItem WorkTask { get; set; }

        public int TaskLabelId { get; set; }
        public virtual TaskLabel TaskLabel { get;set;}
    }
}
