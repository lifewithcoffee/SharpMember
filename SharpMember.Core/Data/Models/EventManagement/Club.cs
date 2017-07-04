using SharpMember.Core.Data.Models.MemberManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.EventManagement
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
