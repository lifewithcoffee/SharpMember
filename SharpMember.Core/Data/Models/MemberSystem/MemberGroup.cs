using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class MemberGroupEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MemberGroup : MemberGroupEntity
    {
        public Organization Organization { get; set; }
        public virtual List<Member> Members { get; set; } = new List<Member>();
    }
}
