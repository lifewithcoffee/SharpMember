using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.ActivitySystem
{
    public class ClubMemberRelation
    {
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }

        public int MemberId { get; set; }
        public virtual Member Member { get; set; }
    }
}
