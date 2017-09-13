using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityGroupsViewService
    {
        CommunityGroupsVM Get(int commId);
        void PostToDeleteSelected(CommunityGroupsVM data);
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
                    string trim = g.Introduction.Trim();
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

        public void PostToDeleteSelected(CommunityGroupsVM data)
        {
            throw new NotImplementedException();
        }
    }
}
