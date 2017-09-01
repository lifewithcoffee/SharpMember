using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models.MemberSystem
{
    public class CommunityEntity
    {
        public int Id { get; set; } // will be actually used as a tenant id
        public string Name { get; set; }
    }

    public class Community : CommunityEntity
    {
        public virtual List<Member> Members { get; set; } = new List<Member>();
        public virtual List<Group> Groups { get; set; } = new List<Group>();
        public virtual List<MemberProfileItemTemplate> MemberProfileItemTemplates { get; set; } = new List<MemberProfileItemTemplate>();
    }
}
