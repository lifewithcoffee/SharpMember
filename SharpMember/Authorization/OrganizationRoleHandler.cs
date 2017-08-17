using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Authorization
{
    public class OrganizationRoleRequirement : IAuthorizationRequirement
    {
        public string OrganizationRole { get; set; }

        public OrganizationRoleRequirement(string organizationRole)
        {
            this.OrganizationRole = organizationRole;
        }
    }

    public class OrganizationRoleHandler : AuthorizationHandler<OrganizationRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrganizationRoleRequirement requirement)
        {
            throw new NotImplementedException();
        }
    }
}
