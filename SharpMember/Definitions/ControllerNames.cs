using SharpMember.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Definitions
{
    static public class ControllerNames
    {
        static public string Communities = nameof(CommunitiesController).Substring(0, nameof(CommunitiesController).LastIndexOf("Controller"));
        static public string Members = nameof(MembersController).Substring(0, nameof(MembersController).LastIndexOf("Controller"));
        static public string Groups = nameof(GroupsController).Substring(0, nameof(GroupsController).LastIndexOf("Controller"));
    }
}
