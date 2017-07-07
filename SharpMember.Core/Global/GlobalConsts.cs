using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Global
{
    static public class GlobalConsts
    {
        public const string SqliteDbFileName = "Members.sqlite";

        private const string AuthRoleOrganizationManagerOnly = "OrganizationManager";

        public const string AuthRoleAdmin = "Admin";
        public const string AuthRoleMemberOnly = "MemberOnly";
        public const string AuthRoleOrganizationManager = AuthRoleAdmin + "," + AuthRoleOrganizationManagerOnly;
    }
}
