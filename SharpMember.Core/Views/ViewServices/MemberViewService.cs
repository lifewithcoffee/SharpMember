using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Mappers;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices
{
    public interface IMemberIndexViewService
    {
        MemberIndexVM Get(int orgId);
        void Post(MemberIndexVM data);
    }

    public class MemberIndexViewService : IMemberIndexViewService
    {
        IMemberRepository _memberRepo;

        public MemberIndexViewService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public MemberIndexVM Get(int orgId)
        {
            var items = _memberRepo
                .GetMany(m => m.OrganizationId == orgId)
                .Select(m => new MemberIndexItemVM { Id = m.Id, Name = m.Name, MemberNumber = m.MemberNumber, Renewed = m.Renewed})
                .ToList();

            return new MemberIndexVM { ItemViewModels = items };
        }

        public void Post(MemberIndexVM data)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMemberCreateViewService
    {
        Task<MemberUpdateVM> GetAsync(int orgId, string appUserId);
        Task<int> Post(MemberUpdateVM data);
    }

    public class MemberCreateViewService : IMemberCreateViewService
    {
        IMemberRepository _memberRepo;

        public MemberCreateViewService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public async Task<MemberUpdateVM> GetAsync(int orgId, string appUserId)
        {
            var member = await _memberRepo.GenerateNewMemberWithProfileItemsAsync(orgId, appUserId);
            return MemberMapper<Member,MemberUpdateVM>.Cast(member);
        }

        public async Task<int> Post(MemberUpdateVM data)
        {
            Member member = MemberMapper<MemberUpdateVM,Member>.Cast(data);
            _memberRepo.Add(member);
            await _memberRepo.CommitAsync();
            return member.Id;
        }
    }

    public interface IMemberEditViewService
    {
        MemberUpdateVM Get();
        void Post(MemberUpdateVM data);
    }

    public class MemberEditViewService : IMemberEditViewService
    {
        public MemberUpdateVM Get()
        {
            throw new NotImplementedException();
        }

        public void Post(MemberUpdateVM data)
        {
            throw new NotImplementedException();
        }
    }
}
