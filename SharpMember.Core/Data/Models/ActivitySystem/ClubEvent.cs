using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.ActivitySystem
{
    public class ClubEventEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DateTime { get; set; }
        public string Address { get; set; }
    }

    public class ClubEvent : ClubEventEntity
    {
        public virtual Organization Organization { get; set; }
        public virtual MemberGroup MemberGroup { get; set; }
        public virtual Club Club { get; set; }
    }
}
