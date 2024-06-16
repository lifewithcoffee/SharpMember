using SharpMember.Core.Data.Models.Member;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class GroupUpdateVm : GroupEntity
    {
        public List<MemberItemVm> MemberItemVms { get; set; } = new List<MemberItemVm>();

        public Group ConvertToGroup()
        {
            return new Group().CopyFrom(this);
        }
    }
}
