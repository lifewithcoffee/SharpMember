using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityGroupsViewService
    {
        CommunityGroupsVM Get(int commId);
        Task PostToDeleteSelected(CommunityGroupsVM data);
    }

    public class CommunityGroupsViewService : ICommunityGroupsViewService
    {
        IGroupRepository _groupRepository;

        public CommunityGroupsViewService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public CommunityGroupsVM Get(int commId)
        {
            var items = _groupRepository
                .GetMany(g => g.CommunityId == commId)
                .ToList()
                .Select(g => {
                    string trim = g.Introduction == null ? "" : g.Introduction.Trim();
                    string intro;
                    if (trim.Length > 150)
                    {
                        intro = trim + " ...";
                    }
                    else
                    {
                        intro = trim;
                    }

                    return new CommunityGroupItemVM { Id = g.Id, Name = g.Name, Introduction = intro };
                }).ToList();

            return new CommunityGroupsVM { CommunityId = commId, ItemViewModels = items };
        }

        public async Task PostToDeleteSelected(CommunityGroupsVM data)
        {
            _groupRepository.DeleteRange(data.ItemViewModels.Where(x => x.Selected).Select(x => new Group { Id = x.Id }));
            await _groupRepository.CommitAsync();
        }
    }
}
