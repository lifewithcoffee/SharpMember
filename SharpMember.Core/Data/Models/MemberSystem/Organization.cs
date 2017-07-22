using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class OrganizationEntity
    {
        public int Id { get; set; } // will be actually used as a tenant id
        public string Name { get; set; }
    }

    public class Organization : OrganizationEntity
    {
        public virtual List<Member> Members { get; set; } = new List<Member>();
        public virtual List<MemberGroup> MemberGroups { get; set; } = new List<MemberGroup>();
        public virtual List<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; } = new List<MemberProfileItemTemplate>();
    }
}
