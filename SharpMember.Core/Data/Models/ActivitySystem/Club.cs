using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.ActivitySystem
{
    public class ClubEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Announcement { get; set; }
    }

    public class Club: ClubEntity
    {
        public virtual Branch Branch { get; set; }
        public virtual List<ClubMemberRelation> ClubMemberRelations { get; set; } = new List<ClubMemberRelation>();
    }
}
