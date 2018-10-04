using AutoMapper;
using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class GroupUpdateVm : GroupEntity
    {
        public List<MemberItemVm> MemberItemVms = new List<MemberItemVm>();

        public Group ConvertToGroup()
        {
            return Mapper.Map<GroupUpdateVm, Group>(this);
        }
    }
}
