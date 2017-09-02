using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Mappers;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharpMember.Utils;

namespace SharpMember.Core.Views.ViewServices
{
    public interface IMemberCreateViewService
    {
        Task<MemberUpdateVM> GetAsync(int orgId, string appUserId);
        Task<int> Post(MemberUpdateVM data);
    }

    public class MemberCreateViewService : IMemberCreateViewService
    {
        IMemberRepository _memberRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public MemberCreateViewService(
            IMemberRepository memberRepo,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        ){
            _memberRepository = memberRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public async Task<MemberUpdateVM> GetAsync(int commId, string appUserId)
        {
            var member = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(commId, appUserId);
            var result = MemberMapper<Member,MemberUpdateVM>.Cast(member);
            result.ProfileItemViewModels = await ConvertTo.MemberProfileItemVMList(member.MemberProfileItems, _memberProfileItemTemplateRepository);
            return result;
        }

        public async Task<int> Post(MemberUpdateVM data)
        {
            Member member = MemberMapper<MemberUpdateVM,Member>.Cast(data);
            member.MemberProfileItems = await ConvertTo.MemberProfileItemList(data.ProfileItemViewModels, _memberProfileItemTemplateRepository);

            _memberRepository.Add(member);
            await _memberRepository.CommitAsync();

            return member.Id;
        }
    }

    public interface IMemberEditViewService
    {
        Task<MemberUpdateVM> GetAsync(int id);
        Task PostAsync(MemberUpdateVM data);
    }

    public class MemberEditViewService : IMemberEditViewService
    {
        IMemberRepository _memberRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public MemberEditViewService(
            IMemberRepository memberRepo,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        ){
            _memberRepository = memberRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public async Task<MemberUpdateVM> GetAsync(int id)
        {
            MemberUpdateVM result = null;

            var member = await _memberRepository.GetMany(m => m.Id == id).Include(m => m.MemberProfileItems).SingleOrDefaultAsync();
            if (member != null)
            {
                result = MemberMapper<Member, MemberUpdateVM>.Cast(member);
                result.ProfileItemViewModels = await ConvertTo.MemberProfileItemVMList(member.MemberProfileItems, _memberProfileItemTemplateRepository);
            }

            return result;
        }

        public async Task PostAsync(MemberUpdateVM data)
        {
            Ensure.IsTrue(data.Id > 0, $"Invalid value: MemberUpdateVM.Id = {data.Id}");

            Member member = MemberMapper<MemberUpdateVM,Member>.Cast(data);
            member.MemberProfileItems = await ConvertTo.MemberProfileItemList(data.ProfileItemViewModels, _memberProfileItemTemplateRepository);
            _memberRepository.Update(member);
            await _memberRepository.CommitAsync();
        }
    }
}
