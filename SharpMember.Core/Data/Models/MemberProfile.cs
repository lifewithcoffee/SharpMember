using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Models
{
    public class MemberProfileEntity
    {
        public int Id { get; set; } // some members may not have been assigned a member number, so an Id field is still required
        public int MemberNumber { get; set; }
        public bool Renewed { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? CeaseDate { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
    }

    public class MemberProfile : MemberProfileEntity
    {
        public virtual Branch Branch { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual List<MemberProfileItem> MemberProfileItems { get; set; } = new List<MemberProfileItem>();
    }
}
