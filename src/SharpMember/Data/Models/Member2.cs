using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Data.Models
{
    public class MemberEntity2
    {
        public int Id { get; set; } // some members may not have been assigned a member number, so an Id field is still required
        public int MemberNumber { get; set; }
        public bool Renewed { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? CeaseDate { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
    }

    public class Member2 : MemberEntity2
    {
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual List<MemberProfileItem> MemberProfileItems { get; set; }
    }
}
