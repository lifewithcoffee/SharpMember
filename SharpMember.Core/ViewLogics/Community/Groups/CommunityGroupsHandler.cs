using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityGroupsHandler
    {
        CommunityGroupsVm Get(int commId);
        Task PostToDeleteSelected(CommunityGroupsVm data);
    }

    public class CommunityGroupsHandler : ICommunityGroupsHandler
    {
        IRepository<Group> _groupRepository;

        public CommunityGroupsHandler(IRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public CommunityGroupsVm Get(int commId)
        {
            var items = _groupRepository
                .Query(g => g.CommunityId == commId)
                .ToList()
                .Select(g => {
                    string trim = g.Description == null ? "" : g.Description.Trim();
                    string intro;
                    if (trim.Length > 150)
                    {
                        intro = trim + " ...";
                    }
                    else
                    {
                        intro = trim;
                    }

                    return new CommunityGroupItemVm { Id = g.Id, Name = string.IsNullOrWhiteSpace(g.Name)? "(No name)" : g.Name, Introduction = intro };
                }).ToList();

            return new CommunityGroupsVm { CommunityId = commId, ItemViewModels = items };
        }

        public async Task PostToDeleteSelected(CommunityGroupsVm data)
        {
            _groupRepository.RemoveRange(data.ItemViewModels.Where(x => x.Selected).Select(x => new Group { Id = x.Id }));
            await _groupRepository.CommitAsync();
        }
    }
}
