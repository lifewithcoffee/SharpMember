using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Authorization
{
    public class RoleName
    {
        public static string OrganizationOwner = nameof(OrganizationOwner);
        public static string OrganizationManager = nameof(OrganizationManager);

        public static string GroupOwner = nameof(GroupOwner);
        public static string GroupManager = nameof(GroupManager);
    }
}
