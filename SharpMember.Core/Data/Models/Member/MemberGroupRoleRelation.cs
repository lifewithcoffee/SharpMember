using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharpMember.Core.Data.Models.Member
{
    public class GroupMemberRelation
    {
        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; }
        public int MemberId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }
        public int GroupId { get; set; }

        public string GroupRole { get; set; }
    }
}
