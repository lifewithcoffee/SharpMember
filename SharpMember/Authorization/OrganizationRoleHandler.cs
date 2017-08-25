using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Authorization
{
    public class CommunityRoleRequirement : IAuthorizationRequirement
    {
        public string CommunityRole { get; set; }

        public CommunityRoleRequirement(string communityRole)
        {
            this.CommunityRole = communityRole;
        }
    }

    public class CommunityRoleHandler : AuthorizationHandler<CommunityRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CommunityRoleRequirement requirement)
        {
            throw new NotImplementedException();
        }
    }
}
