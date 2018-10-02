using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Mappers;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var group = _groupRepository.GetMany(x => x.Id == id).Include(x => x.GroupMemberRelations).ThenInclude(x => x.Member).Single();
            var result = group.ConvertToGroupUpdateVM();
            result.ItemViewModels = group.GroupMemberRelations.Select(x =>
                new CommunityMemberItemVM
                {
                    Id = x.Member.Id,
                    MemberNumber = x.Member.MemberNumber,
                    Name = x.Member.Name,
                    Renewed = x.Member.Renewed,
                    Selected = false
                }).ToList();
            return result;
        }

        public async Task PostAsync(GroupUpdateVM data)
        {
            Ensure.IsTrue(data.Id > 0);
            _groupRepository.Update(data.ConvertToGroup());
            await _groupRepository.CommitAsync();
        }
    }
}
