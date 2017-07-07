using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class MemberGroupRelation
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int MemberGroupId { get; set; }
        public MemberGroup MemberGroup { get; set; }
    }
}
