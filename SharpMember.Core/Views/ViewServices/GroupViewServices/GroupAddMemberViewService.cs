using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewServices.GroupViewServices
{
    public interface IGroupAddMemberViewService
    {
        GroupAddMemberVm GetAsync(int groupId);
        void Post(GroupAddMemberVm vm);
    }

    public class GroupAddMemberViewService : IGroupAddMemberViewService
    {
        public GroupAddMemberVm GetAsync(int groupId)
        {
            throw new NotImplementedException();
        }

        public void Post(GroupAddMemberVm vm)
        {
            throw new NotImplementedException();
        }
    }
}
