using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models
{
    public class CommunityEventEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DateTime { get; set; }
        public string Address { get; set; }
    }

    public class CommunityEvent : CommunityEventEntity
    {
        public virtual Organization Organization { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual Club Club { get; set; }
    }
}
