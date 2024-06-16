﻿using SharpMember.Core.Data.DataServices.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using SharpMember.Core.Data.Models.Member;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data;
using System.Threading.Tasks;
using NetCoreUtils.String;
using NetCoreUtils.Database;

namespace U.TestEnv
{
    public class TestUtil
    {
        readonly IServiceProvider _serviceProvider;

        public TestUtil(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private IServiceProvider GetNewProvider()
        {
            return _serviceProvider.CreateScope().ServiceProvider;
        }

        public int GetExistingCommunityId()
        {
            var repo = GetNewProvider().GetService<IRepository<Community>>();
            var community = repo.Add(new Community { Name = ShortGuid.NewGuid() });
            repo.Commit();
            return community.Id;
        }

        public int GetNonexistentCommunityId()
        {
            var repo = GetNewProvider().GetService<IRepository<Community>>();
            var community = repo.QueryAll().OrderBy(o => o.Id).LastOrDefault();
            if (null == community)
                return 1;
            else
                return community.Id + 100;
        }

        public int GetExistingMemberId(int? existingCommunityId = null)
        {
            if(existingCommunityId == null)
                existingCommunityId = this.GetExistingCommunityId();

            var repo = GetNewProvider().GetService<IMemberService>();
            var member = repo.Add(new Member { CommunityId = existingCommunityId.Value });
            repo.Repo.Commit();
            return member.Id;
        }

        public int GetNonexistentMemberId()
        {
            var memberRepo = GetNewProvider().GetService<IMemberService>();
            var member = memberRepo.Repo.QueryAll().OrderBy(m => m.Id).LastOrDefault();
            if(null == member)
                return 1;
            else
                return member.Id + 100;
        }

        public async Task<string> GetExistingAppUserId()
        {
            UserManager<AppUser> userManager = GetNewProvider().GetService<UserManager<AppUser>>();

            var appUser = new AppUser { UserName = Guid.NewGuid().ToString() };
            await userManager.CreateAsync(appUser);
            var user = await userManager.FindByNameAsync(appUser.UserName);
            return user.Id.ToString();
        }
    }
}
