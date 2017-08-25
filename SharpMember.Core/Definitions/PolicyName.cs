using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Definitions
{
    public class PolicyName
    {
        public static string RequireRoleOf_CommunityOwner = nameof(RequireRoleOf_CommunityOwner);
        public static string RequireRoleOf_CommunityManager = nameof(RequireRoleOf_CommunityManager);

        public static string RequireRoleOf_GroupOwner = nameof(RequireRoleOf_GroupOwner);
        public static string RequireRoleOf_GroupManager = nameof(RequireRoleOf_GroupManager);
    }
}
