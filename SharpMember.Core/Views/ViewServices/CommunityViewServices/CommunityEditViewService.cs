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
        CommunityUpdateVm Get(int commId, int addMore);
        Task PostAsync(CommunityUpdateVm data);
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

        public CommunityUpdateVm Get(int commId, int addMore)
        {
            var community = _communityRepository.GetMany(c => c.Id == commId).Include(c => c.MemberProfileItemTemplates).Single();

            CommunityUpdateVm result = community.ConvertToCommunityUpdateVM();
            result.ItemTemplateVMs = community.MemberProfileItemTemplates.Select(x => new MemberProfileItemTemplateVm { ItemTemplate = x}).ToList();

            for(int i = 0; i < addMore; i++)
            {
                result.ItemTemplateVMs.Add(new MemberProfileItemTemplateVm());
            }

            return result;
        }

        /// <summary>
        /// Unit test: <see cref="U.ViewServices.CommunityViewServiceTests.Community_edit_view_service"/>
        /// </summary>
        public async Task PostAsync(CommunityUpdateVm data)
        {
            _communityRepository.Update(data.ConvertToCommunityWithoutNavProp());
            await _communityRepository.CommitAsync();

            _memberProfileItemTemplateRepository.DeleteRange(data.ItemTemplateVMs.Where(x => x.Delete).Select(x => x.ItemTemplate).ToList());
            _memberProfileItemTemplateRepository.AddOrUpdateItemTemplates(data.Id, data.ItemTemplateVMs.Where(x => !x.Delete && !string.IsNullOrWhiteSpace(x.ItemTemplate.ItemName)).Select(x => x.ItemTemplate).ToList());

            await _communityRepository.CommitAsync();
        }
    }
}
