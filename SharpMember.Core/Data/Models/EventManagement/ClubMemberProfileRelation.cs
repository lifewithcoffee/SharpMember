using SharpMember.Core.Data.Models.MemberManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.EventManagement
{
    public class ClubMemberProfileRelation
    {
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }

        public int MemberProfileId { get; set; }
        public virtual MemberProfile MemberProfile { get; set; }
    }
}
