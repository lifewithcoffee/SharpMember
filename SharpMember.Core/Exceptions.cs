using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core
{
    public class MemberNameExistException : Exception
    {
        public MemberNameExistException(string message) : base(message) { }
    }

    public class OrganizationNotExistException : Exception
    {
        public OrganizationNotExistException(string message) : base(message) { }
    }
}
