using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models
{
    public class ClubEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
    }

    public class Club: ClubEntity
    {
        public virtual Branch Branch { get; set; }
        public virtual List<MemberProfile> SubscribedMembers { get; set; }
    }
}
