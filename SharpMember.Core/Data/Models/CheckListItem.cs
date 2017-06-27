using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models
{
    public class CheckListItemEntity
    {
        public int Id { get; set; }
        public bool Done { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
    }

    public class CheckListItem : CheckListItemEntity
    {
        public virtual WorkTask WorkTask { get; set; }
    }
}
