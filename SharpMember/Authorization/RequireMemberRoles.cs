using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Authorization
{
    public enum AuthPolicies
    {
        RequireRoleOf_OrganizationOwner,
        RequireRoleOf_GroupManager,
        RequireRoleOf_GroupAssistantManager
    }

    public class MemberRolesRequirement : IAuthorizationRequirement
    {
        public string MemberRole { get; private set; }

        public MemberRolesRequirement(string memberRole)
        {
            MemberRole = memberRole;
        }
    }

    public class MemberRolesHandler : AuthorizationHandler<MemberRolesRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberRolesHandler(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MemberRolesRequirement requirement)
        {
            var user = await _userManager.GetUserAsync(context.User);
            string userId = user?.Id;

            //return Task.CompletedTask;

            context.Succeed(requirement);
        }
    }
}
