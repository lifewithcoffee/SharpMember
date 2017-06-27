using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models
{
    public class WorkTaskEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public bool Done { get; set; }
        public bool Pinned { get; set; }
        public string Status { get; set; }  // user input or select from a drop down menu
        public string Comments { get; set; }
    }

    public class WorkTask : WorkTaskEntity
    {
        public virtual List<CheckListItem> CheckListItems { get; set; } = new List<CheckListItem>();
        public virtual MemberProfile WorkTaskCreator { get; set; }
        public virtual MemberProfile WorkTaskOwner { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }    // for user's private tasks
    }
}
