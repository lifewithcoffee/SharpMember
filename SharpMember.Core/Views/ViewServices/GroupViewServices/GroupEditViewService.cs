using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Mappers;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.GroupViewServices
{
    public interface IGroupEditViewService
    {
        GroupUpdateVM GetAsync(int id);
        void PostAsync(GroupUpdateVM data);
    }

    public class GroupEditViewService : IGroupEditViewService
    {
        IGroupRepository _groupRepository;

        public GroupEditViewService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public GroupUpdateVM GetAsync(int id)
        {
            return _groupRepository.GetById(id).ConvertToGroupUpdateVM();
        }

        public void PostAsync(GroupUpdateVM data)
        {
            Ensure.IsTrue(data.Id > 0);
            _groupRepository.Update(data.ConvertToGroup());
        }
    }
}
