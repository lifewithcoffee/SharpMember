using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models
{
    public class MemberProfileItemEntity
    {
        [Key]
        public string ItemName { get; set; }
        public string ItemType { get; set; }    // e.g. date, int, string
        public string ItemValue { get; set; }
    }

    public class MemberProfileItem : MemberProfileItemEntity
    {
        public virtual MemberProfile Member2 { get; set; }
    }
}
