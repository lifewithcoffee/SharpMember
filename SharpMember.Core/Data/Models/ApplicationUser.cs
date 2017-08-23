using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Models.TaskSystem;
using Microsoft.AspNetCore.Identity;

namespace SharpMember.Core.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual List<Member> Members { get; set; } = new List<Member>();
        //public virtual List<WorkTask> WorkTasks { get; set; } = new List<TaskSystem.WorkTask>();   // private tasks
    }
}
