using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Core.Data.Models.Member;
using SharpMember.Core.Data.Models.Project;
using Microsoft.AspNetCore.Identity;

namespace SharpMember.Core.Data.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }
    }
}
