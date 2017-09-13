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
        GroupUpdateVM Get(int id);
        Task PostAsync(GroupUpdateVM data);
    }

    public class GroupEditViewService : IGroupEditViewService
    {
        IGroupRepository _groupRepository;

        public GroupEditViewService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public GroupUpdateVM Get(int id)
        {
            return _groupRepository.GetById(id).ConvertToGroupUpdateVM();
        }

        public async Task PostAsync(GroupUpdateVM data)
        {
            Ensure.IsTrue(data.Id > 0);
            _groupRepository.Update(data.ConvertToGroup());
            await _groupRepository.CommitAsync();
        }
    }
}
