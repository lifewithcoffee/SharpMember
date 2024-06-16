using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Models.ProjectSystem;
using Microsoft.AspNetCore.Identity;

namespace SharpMember.Core.Data.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }
    }
}
