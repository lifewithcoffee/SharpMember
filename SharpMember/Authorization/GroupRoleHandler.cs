using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Authorization
{
    public class GroupRoleRequirement : IAuthorizationRequirement
    {
        public string MemberRole { get; private set; }

        public GroupRoleRequirement(string memberRole)
        {
            this.MemberRole = memberRole;
        }
    }

    public class GroupRoleHandler : AuthorizationHandler<GroupRoleRequirement>
    {
        UserManager<ApplicationUser> _userManager;
        IMemberRepository _memberRepo;

        public GroupRoleHandler(UserManager<ApplicationUser> userManager, IMemberRepository memberRepo)
        {
            this._userManager = userManager;
            this._memberRepo = memberRepo;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupRoleRequirement requirement)
        {
            var user = await _userManager.GetUserAsync(context.User);
            string userId = user?.Id;

            string orgRole = this._memberRepo.GetMany(m => m.ApplicationUserId == userId).SingleOrDefault()?.OrganizationRole;
            //return Task.CompletedTask;

            context.Succeed(requirement);
        }
    }
}
