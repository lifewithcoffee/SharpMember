using SharpMember.Core.Data.Models.MemberManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.EventManagement
{
    public class ClubMemberRelation
    {
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }

        public int MemberId { get; set; }
        public virtual Member Member { get; set; }
    }
}
