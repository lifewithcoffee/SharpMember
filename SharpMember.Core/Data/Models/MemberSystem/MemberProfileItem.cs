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
        [Key]
        public string ItemName { get; set; }
        public string ItemType { get; set; }    // for validation, e.g. date, int, string
        public string ItemValue { get; set; }
    }

    public class MemberProfileItem : MemberProfileItemEntity
    {
        public int MemberId { get; set; }

        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; }
    }
}
