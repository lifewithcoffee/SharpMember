using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SharpMember.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Authorization
{
    public class AuthPolicy
    {
        public string RequireRoleOf_OrganizationOwner = nameof(RequireRoleOf_OrganizationOwner);
        public string RequireRoleOf_OrganizationManager = nameof(RequireRoleOf_OrganizationManager);

        public string RequireRoleOf_GroupOwner = nameof(RequireRoleOf_GroupOwner);
        public string RequireRoleOf_GroupManager = nameof(RequireRoleOf_GroupManager);
    }

    public class RoleName
    {
        public string OrganizationOwner = nameof(OrganizationOwner);
        public string OrganizationManager = nameof(OrganizationManager);

        public string GroupOwner = nameof(GroupOwner);
        public string GroupManager = nameof(GroupManager);
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
