using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SharpMember.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class

    /// <summary>
    /// Properties inherited from IdentityUser:
    ///     Email
    ///     PhoneNumber
    ///     UserName
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public int MemberNumber { get; set; }
        public bool Renewed { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string Occupation { get; set; }
        public string Mobile { get; set; }
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
}
