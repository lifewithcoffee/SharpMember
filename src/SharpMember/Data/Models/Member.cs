using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Data.Models
{
    /// <summary>
    /// If there are multiple items for Phone, Address etc., separate them by ';'.
    /// </summary>
    public class MemberEntity
    {
        public int Id { get; set; } // some members may not have been assigned a member number, so an Id field is still required
        public int MemberNumber { get; set; }
        public bool Renewed { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string Organization { get; set; }
        public string Occupation { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Degree { get; set; }
        public int GraduationYear { get; set; }
        public string HomeTown { get; set; }
        public string Wechat { get; set; }
        public string QQ { get; set; }
        public string Skype { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? CeaseDate { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Remarks { get; set; }
    }

    public class Member : MemberEntity
    {
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
