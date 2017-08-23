using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Definitions
{
    public class PolicyName
    {
        public static string RequireRoleOf_OrganizationOwner = nameof(RequireRoleOf_OrganizationOwner);
        public static string RequireRoleOf_OrganizationManager = nameof(RequireRoleOf_OrganizationManager);

        public static string RequireRoleOf_GroupOwner = nameof(RequireRoleOf_GroupOwner);
        public static string RequireRoleOf_GroupManager = nameof(RequireRoleOf_GroupManager);
    }
}
