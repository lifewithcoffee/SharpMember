using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Mappers;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.MemberViewServices
{
    public interface IMemberEditViewService
    {
        Task<MemberUpdateVm> GetAsync(int id);
        Task PostAsync(MemberUpdateVm data);
    }

    public class MemberEditViewService : IMemberEditViewService
    {
        IMemberRepository _memberRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public MemberEditViewService(
            IMemberRepository memberRepo,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        )
        {
            _memberRepository = memberRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public async Task<MemberUpdateVm> GetAsync(int id)
        {
            MemberUpdateVm result = null;

            // get existing items
            var member = await _memberRepository.GetMany(m => m.Id == id).Include(m => m.MemberProfileItems).SingleOrDefaultAsync();
            if (member != null)
            {
                result = MemberMapper<Member, MemberUpdateVm>.Cast(member);
                result.ProfileItemViewModels = await ConvertTo.MemberProfileItemVMList(member.MemberProfileItems, _memberProfileItemTemplateRepository);
            }

            // get the possible new items from the community item templates
            var templateIds = member.MemberProfileItems.Select(i => i.MemberProfileItemTemplateId).ToList();
            var newItems = _memberProfileItemTemplateRepository
                .GetMany(t => t.CommunityId == member.CommunityId)
                .Where(t => !templateIds.Contains(t.Id))
                .Select(t => new MemberProfileItem { MemberId = member.Id, MemberProfileItemTemplateId = t.Id })
                .ToList();

            var newItemVms = await ConvertTo.MemberProfileItemVMList(newItems, _memberProfileItemTemplateRepository);
            result.ProfileItemViewModels.AddRange(newItemVms);

            return result;
        }

        public async Task PostAsync(MemberUpdateVm data)
        {
            Ensure.IsTrue(data.Id > 0, $"Invalid value: MemberUpdateVM.Id = {data.Id}");

            Member member = MemberMapper<MemberUpdateVm, Member>.Cast(data);
            member.MemberProfileItems = await ConvertTo.MemberProfileItemList(data.ProfileItemViewModels, _memberProfileItemTemplateRepository);
            _memberRepository.Update(member);
            await _memberRepository.CommitAsync();
        }
    }
}
