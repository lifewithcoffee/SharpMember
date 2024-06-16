using SharpMember.Core.Data.Models.Member;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.ActivitySystem
{
    public class GatheringEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DateTime { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
    }

    public class Gathering : GatheringEntity
    {
    }
}
