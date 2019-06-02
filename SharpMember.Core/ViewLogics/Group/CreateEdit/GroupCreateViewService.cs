using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Utils.Mappers;
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
        GroupUpdateVm GetAsync(int commId);
        Task<int> Post(GroupUpdateVm data);
    }

    public class GroupCreateViewService : IGroupCreateViewService
    {
        IRepositoryBase<Group> _groupRepository;

        public GroupCreateViewService(IRepositoryBase<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public GroupUpdateVm GetAsync(int commId)
        {
            return new GroupUpdateVm { CommunityId = commId };
        }

        public async Task<int> Post(GroupUpdateVm data)
        {
            var newGroup = _groupRepository.Add(data.ConvertToGroup());
            await _groupRepository.CommitAsync();
            return newGroup.Id;
        }
    }
}
