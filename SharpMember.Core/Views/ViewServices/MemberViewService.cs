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
        IMemberRepository _memberRepo;

        public MemberCreateViewService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public async Task<MemberUpdateVM> GetAsync(int commId, string appUserId)
        {
            var member = await _memberRepo.GenerateNewMemberWithProfileItemsAsync(commId, appUserId);
            var result = MemberMapper<Member,MemberUpdateVM>.Cast(member);
            result.MemberProfileItems = member.MemberProfileItems.Select(i => MemberProfileItemMapper<MemberProfileItem, MemberProfileItemEntity>.Cast(i) ).ToList();
            result.CommunityId = commId;
            result.ApplicationUserId = appUserId;
            return result;
        }

        public async Task<int> Post(MemberUpdateVM data)
        {
            Member member = MemberMapper<MemberUpdateVM,Member>.Cast(data);
            member.MemberProfileItems = data.MemberProfileItems.Select(i => MemberProfileItemMapper<MemberProfileItemEntity, MemberProfileItem>.Cast(i)).ToList();
            _memberRepo.Add(member);
            await _memberRepo.CommitAsync();
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
        IMemberRepository _memberRepo;

        public MemberEditViewService(IMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public async Task<MemberUpdateVM> GetAsync(int id)
        {
            MemberUpdateVM result = null;

            var member = await _memberRepo.GetMany(m => m.Id == id).Include(m => m.MemberProfileItems).SingleOrDefaultAsync();
            if(member != null)
            {
                MemberUpdateVM model = MemberMapper<Member, MemberUpdateVM>.Cast(member);
                model.MemberProfileItems = member.MemberProfileItems.Select( i => MemberProfileItemMapper<MemberProfileItem, MemberProfileItemEntity>.Cast(i) ).ToList();

                result = model;
            }

            return result;
        }

        public async Task PostAsync(MemberUpdateVM data)
        {
            Ensure.IsTrue(data.Id > 0, $"Invalid value: MemberUpdateVM.Id = {data.Id}");

            Member member = MemberMapper<MemberUpdateVM,Member>.Cast(data);
            _memberRepo.Update(member);
            await _memberRepo.CommitAsync();
            //return member.Id;
        }
    }
}
