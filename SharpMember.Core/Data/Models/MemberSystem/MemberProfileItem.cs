using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class MemberProfileItemEntity
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
        public bool IsRequired { get; set; } = false;
    }

    public class MemberProfileItemWithFK : MemberProfileItemEntity
    { 
        public int MemberId { get; set; }
    }

    public class MemberProfileItem : MemberProfileItemWithFK
    {
        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; }
    }
}
