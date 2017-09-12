using AutoMapper;
using SharpMember.Core.Data.Models.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class GroupUpdateVM : GroupEntity
    {
        public Group ConvertToGroup()
        {
            return Mapper.Map<GroupUpdateVM, Group>(this);
        }
    }
}
