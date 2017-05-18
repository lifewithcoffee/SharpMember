using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Data.Models
{
    public class MemberProfileItemEntity
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }    // e.g. date, int, string
        public string ItemValue { get; set; }
    }

    public class MemberProfileItem : MemberProfileItemEntity
    {
        public virtual Member2 Member2 { get; set; }
    }
}
