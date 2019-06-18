using NetCoreUtils.Database;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.DataServices.MemberSystem;
using SharpMember.Core.Definitions;
using SharpMember.Core.Views.ViewModels;
using SharpMember.Core.Views.ViewModels.CommunityVms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Views.ViewServices.CommunityViewServices
{
    public interface ICommunityCreateHandler
    {
        CommunityUpdateVm Get();
        Task<int> PostAsync(string appUserId, CommunityUpdateVm data);
    }

    public class CommunityCreateHandler : ICommunityCreateHandler
    {
        IRepository<Community> _communityRepository;
        IRepository<Member> _memberRepo;
        IRepository<MemberProfileItemTemplate> _memberProfileItemTemplateRepo;
        IMemberRepository _memberRepository;
        IMemberProfileItemTemplateService _memberProfileItemTemplateRepository;

        public CommunityCreateHandler(
            IRepository<Community> orgRepo,
            IRepository<Member> memberRepo,
            IRepository<MemberProfileItemTemplate> memberProfileItemTemplateRepo,
            IMemberRepository memberRepository,
            IMemberProfileItemTemplateService memberProfileItemTemplateRepository
        )
        {
            _communityRepository = orgRepo;
            _memberRepo = memberRepo;
            _memberProfileItemTemplateRepo = memberProfileItemTemplateRepo;
            _memberRepository = memberRepository;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public CommunityUpdateVm Get()
        {
            CommunityUpdateVm model = new CommunityUpdateVm
            {
                ItemTemplateVMs = Enumerable.Range(0, 5).Select(i => new MemberProfileItemTemplateVm()).ToList()
            };

            return model;
        }

        public async Task<int> PostAsync(string appUserId, CommunityUpdateVm data)
        {
            Community community = new Community { Name = data.Name };
            _communityRepository.Add(community);
            await _communityRepository.CommitAsync();

            Member newMember = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(community.Id, appUserId);
            newMember.CommunityRole = RoleNames.CommunityOwner;
            await _memberRepo.CommitAsync();

            var required = data.ItemTemplateVMs.Where(p => p.ItemTemplate.IsRequired == true).Select(p => p.ItemTemplate.ItemName);
            await _memberProfileItemTemplateRepository.AddTemplatesAsync(community.Id, required, true);

            var optional = data.ItemTemplateVMs.Where(p => p.ItemTemplate.IsRequired == false).Select(p => p.ItemTemplate.ItemName);
            await _memberProfileItemTemplateRepository.AddTemplatesAsync(community.Id, optional, false);

            await _memberProfileItemTemplateRepo.CommitAsync();

            return community.Id;
        }
    }
}
