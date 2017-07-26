using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core
{
    public class MemberNameExistsException : Exception
    {
        public MemberNameExistsException(string message) : base(message) { }
    }

    public class OrganizationNotExistsException : Exception
    {
        public OrganizationNotExistsException(int invalidOrgId):base($"The organization with Id {invalidOrgId} does not exist.") { }
    }
}
