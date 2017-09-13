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
    public interface IGroupCreateViewService
    {
        GroupUpdateVM GetAsync(int commId);
        Task<int> Post(GroupUpdateVM data);
    }

    public class GroupCreateViewService : IGroupCreateViewService
    {
        IGroupRepository _groupRepository;

        public GroupCreateViewService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public GroupUpdateVM GetAsync(int commId)
        {
            return new GroupUpdateVM { CommunityId = commId };
        }

        public async Task<int> Post(GroupUpdateVM data)
        {
            var newGroup = _groupRepository.Add(data.ConvertToGroup());
            await _groupRepository.CommitAsync();
            return newGroup.Id;
        }
    }
}
