using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityEditViewService
    {
        CommunityUpdateVM Get(int commId);
        Task PostAsync(CommunityUpdateVM data);
    }

    public class CommunityEditViewService : ICommunityEditViewService
    {
        ICommunityRepository _communityRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public CommunityEditViewService(
            ICommunityRepository orgRepo,
            IMemberRepository memberRepository,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        )
        {
            _communityRepository = orgRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public CommunityUpdateVM Get(int commId)
        {
            var org = _communityRepository.GetMany(o => o.Id == commId).Include(o => o.MemberProfileItemTemplates).Single();

            CommunityUpdateVM result = new CommunityUpdateVM { Id = org.Id, Name = org.Name };
            result.MemberProfileItemTemplates = org.MemberProfileItemTemplates;

            return result;
        }

        public async Task PostAsync(CommunityUpdateVM data)
        {
            var community = _communityRepository.GetById(data.Id);
            community.Name = data.Name;
            await _communityRepository.CommitAsync();

            _memberProfileItemTemplateRepository.AddOrUpdateItemTemplates(data.Id, data.MemberProfileItemTemplates);
            await _communityRepository.CommitAsync();
        }
    }
}
