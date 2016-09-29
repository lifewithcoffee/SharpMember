using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SharpMember.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual UserAdditionalInfo UserAdditionalInfo { get; set; }
    }

    public class UserAdditionalInfo
    {
        public int Id { get; set; }
        public int MemberNumber { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
