using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class MemberMemberGroupRelation
    {
        [ForeignKey(nameof(MemberId))]
        public Member Member { get; set; }
        public int MemberId { get; set; }

        [ForeignKey(nameof(MemberGroupId))]
        public MemberGroup MemberGroup { get; set; }
        public int MemberGroupId { get; set; }
    }
}
