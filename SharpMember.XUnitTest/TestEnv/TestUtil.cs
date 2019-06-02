using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using SharpMember.Core.Data.Models.MemberSystem;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data;
using System.Threading.Tasks;
using NetCoreUtils.String;

namespace U.TestEnv
{
    public class TestUtil
    {
        IServiceProvider _serviceProvider;

        public TestUtil(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public int GetExistingCommunityId()
        {
            var repo = _serviceProvider.GetService<ICommunityRepository>();
            var community = repo.Add(ShortGuid.NewGuid());
            repo.Repo.Commit();
            return community.Id;
        }

        public int GetNonexistentCommunityId()
        {
            var repo = _serviceProvider.GetService<ICommunityRepository>();
            var community = repo.Repo.GetAll().OrderBy(o => o.Id).LastOrDefault();
            if (null == community)
                return 1;
            else
                return community.Id + 100;
        }

        public int GetExistingMemberId(int? existingCommunityId = null)
        {
            if(existingCommunityId == null)
                existingCommunityId = this.GetExistingCommunityId();

            var repo = _serviceProvider.GetService<IMemberRepository>();
            var member = repo.Add(new Member { CommunityId = existingCommunityId.Value });
            repo.Repo.Commit();
            return member.Id;
        }

        public int GetNonexistentMemberId()
        {
            var memberRepo = _serviceProvider.GetService<IMemberRepository>();
            var member = memberRepo.Repo.GetAll().OrderBy(m => m.Id).LastOrDefault();
            if(null == member)
                return 1;
            else
                return member.Id + 1;
        }

        public async Task<string> GetExistingAppUserId()
        {
            UserManager<ApplicationUser> userManager = _serviceProvider.GetService<UserManager<ApplicationUser>>();

            var appUser = new ApplicationUser { UserName = Guid.NewGuid().ToString() };
            IdentityResult identityResult = await userManager.CreateAsync(appUser);
            var user = await userManager.FindByNameAsync(appUser.UserName);
            return user.Id;
        }
    }
}
