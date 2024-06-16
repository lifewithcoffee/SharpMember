﻿using Microsoft.EntityFrameworkCore;
using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.Community;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Utils.Mappers;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.MemberViewServices
{
    public interface IMemberEditHandler
    {
        Task<MemberUpdateVm> GetAsync(int id);
        Task PostAsync(MemberUpdateVm data);
    }

    public class MemberEditHandler : IMemberEditHandler
    {
        readonly IRepository<Member> _memberRepository;
        readonly IRepository<MemberProfileItemTemplate> _memberProfileItemTemplateRepository;

        readonly IRepository<GroupMemberRelation> _groupMemberRelationRepository;

        public MemberEditHandler(
            IRepository<Member> memberRepo,
            IRepository<MemberProfileItemTemplate> memberProfileItemTemplateRepository,
            IRepository<GroupMemberRelation> groupMemberRelationRepository
        )
        {
            _memberRepository = memberRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
            _groupMemberRelationRepository = groupMemberRelationRepository;
        }

        public async Task<MemberUpdateVm> GetAsync(int id)
        {
            MemberUpdateVm result = null;

            // get existing items
            var member = await _memberRepository.Query(m => m.Id == id).Include(m => m.MemberProfileItems).SingleOrDefaultAsync();
            if (member != null)
            {
                result.CopyFrom(member);
                result.ProfileItemViewModels = await ConvertTo.MemberProfileItemVMList(member.MemberProfileItems, _memberProfileItemTemplateRepository);
            }

            // get the possible new items from the community item templates
            var templateIds = member.MemberProfileItems.Select(i => i.MemberProfileItemTemplateId).ToList();
            var newItems = _memberProfileItemTemplateRepository
                .Query(t => t.CommunityId == member.CommunityId)
                .Where(t => !templateIds.Contains(t.Id))
                .Select(t => new MemberProfileItem { MemberId = member.Id, MemberProfileItemTemplateId = t.Id })
                .ToList();

            var newItemVms = await ConvertTo.MemberProfileItemVMList(newItems, _memberProfileItemTemplateRepository);
            result.ProfileItemViewModels.AddRange(newItemVms);
            result.GroupList = GetGroups(id);

            return result;
        }

        public async Task PostAsync(MemberUpdateVm data)
        {
            Ensure.IsTrue(data.Id > 0, $"Invalid value: MemberUpdateVM.Id = {data.Id}");

            Member member = new Member().CopyFrom(data);
            member.MemberProfileItems = await ConvertTo.MemberProfileItemList(data.ProfileItemViewModels, _memberProfileItemTemplateRepository);
            _memberRepository.Update(member);
            await _memberRepository.CommitAsync();
        }

        private List<CommunityGroupItemVm> GetGroups(int memberId)
        {
            return _groupMemberRelationRepository.Query(x => x.MemberId == memberId)
                                                 .Include(x => x.Group)
                                                 .Select(x => new CommunityGroupItemVm { Id = x.GroupId, Name = x.Group.Name, Introduction = x.Group.Description })
                                                 .ToList();
        }
    }
}
