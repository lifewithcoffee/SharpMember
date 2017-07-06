using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.TaskSystem
{
    public class WorkTaskLabelRelation
    {
        public int WorkTaskId { get; set; }
        public virtual WorkTask WorkTask { get; set; }

        public int TaskLabelId { get; set; }
        public virtual TaskLabel TaskLabel { get;set;}
    }
}
