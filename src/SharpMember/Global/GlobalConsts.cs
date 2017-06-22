using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Global
{
    static public class GlobalConsts
    {
        public const string SqliteDbFileName = "Members.sqlite";

        public const string AuthRoleAdmin = "Admin";
        public const string AuthRoleOrganizationManager = "OrganizationManager";
        public const string AuthRoleBranchManager = "BranchManager";
    }
}
