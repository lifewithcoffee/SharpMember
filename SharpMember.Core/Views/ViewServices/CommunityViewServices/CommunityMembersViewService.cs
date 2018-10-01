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
    public interface ICommunityMembersViewService
    {
        CommunityMembersVM Get(int commId);
        Task PostToDeleteSelected(CommunityMembersVM data);
    }

    public class CommunityMembersViewService : ICommunityMembersViewService
    {
        IMemberRepository _memberRepo;

        public CommunityMembersViewService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public CommunityMembersVM Get(int commId)
        {
            var items = _memberRepo
                .GetMany(m => m.CommunityId == commId)
                .Select(m => new CommunityMemberItemVM { Id = m.Id, Name = string.IsNullOrWhiteSpace(m.Name) ? "(No name)" : m.Name, MemberNumber = m.MemberNumber, Renewed = m.Renewed })
                .ToList();

            return new CommunityMembersVM {  CommunityId = commId, ItemViewModels = items };
        }

        public async Task PostToDeleteSelected(CommunityMembersVM data)
        {
            _memberRepo.DeleteRange(data.ItemViewModels.Where(x => x.Selected).Select(x => new Member { Id = x.Id }));
            await _memberRepo.CommitAsync();
        }
    }
}
