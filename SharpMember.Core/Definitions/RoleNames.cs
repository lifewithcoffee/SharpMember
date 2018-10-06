using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Definitions
{
    public class InternalRoleNames
    {
        protected const string AuthRoleCommunityManagerOnly = "CommunityManager";
    }

    public class RoleNames : InternalRoleNames
    {
        public static string CommunityOwner = nameof(CommunityOwner);
        public static string CommunityManager = nameof(CommunityManager);

        public static string GroupOwner = nameof(GroupOwner);
        public static string GroupManager = nameof(GroupManager);


        public const string AuthRoleAdmin = "Admin";
        public const string AuthRoleMemberOnly = "MemberOnly";
        public const string AuthRoleCommunityManager = AuthRoleAdmin + "," + AuthRoleCommunityManagerOnly;
    }
}
