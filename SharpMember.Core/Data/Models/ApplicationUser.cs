using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SharpMember.Core.Data.Models.MemberManagement;
using SharpMember.Core.Data.Models.TaskManagement;

namespace SharpMember.Core.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual List<MemberProfile> MemberProfiles { get; set; } = new List<MemberProfile>();
        public virtual List<WorkTask> WorkTasks { get; set; } = new List<TaskManagement.WorkTask>();   // private tasks
    }
}
