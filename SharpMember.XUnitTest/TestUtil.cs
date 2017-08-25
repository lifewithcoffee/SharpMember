using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using SharpMember.Core.Data.Models.MemberSystem;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SharpMember.Core.Data;
using System.Threading.Tasks;

namespace U
{
    class TestUtil : DependencyEnabled
    {
        public int GetExistingCommunityId()
        {
            var repo = this.serviceProvider.GetService<ICommunityRepository>();
            var org = repo.Add(Guid.NewGuid().ToString());
            repo.Commit();
            return org.Id;
        }

        public int GetNonexistentCommunityId()
        {
            var repo = this.serviceProvider.GetService<ICommunityRepository>();
            var org = repo.GetAll().OrderBy(o => o.Id).LastOrDefault();
            if (null == org)
            {
                return 1;
            }
            else
            {
                return org.Id + 1;
            }
        }

        public int GetExistingMemberId()
        {
            int existingOrgId = this.GetExistingCommunityId();
            var repo = this.serviceProvider.GetService<IMemberRepository>();
            var member = repo.Add(new Member { CommunityId = existingOrgId });
            repo.Commit();
            return member.Id;
        }

        public int GetNonexistentMemberId()
        {
            var memberRepo = this.serviceProvider.GetService<IMemberRepository>();
            var member = memberRepo.GetAll().OrderBy(m => m.Id).LastOrDefault();
            if(null == member)
            {
                return 1;
            }
            else
            {
                return member.Id + 1;
            }
        }

        public async Task<string> GetExistingAppUserId(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var appUser = new ApplicationUser { UserName = Guid.NewGuid().ToString() };
            IdentityResult identityResult = await userManager.CreateAsync(appUser);
            var user = await userManager.FindByNameAsync(appUser.UserName);
            return user.Id;
        }
    }
}
