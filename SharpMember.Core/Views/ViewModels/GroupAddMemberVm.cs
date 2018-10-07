using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class GroupAddMemberVm
    {
        public int GroupId { get; set; }
        public List<MemberItemVm> MemberItemVms { get; set; } = new List<MemberItemVm>();
    }
}
