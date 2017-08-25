using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Definitions;
using Microsoft.EntityFrameworkCore;
using SharpMember.Core.Data.RepositoryBase;
using SharpMember.Core.Data;

namespace SharpMember.Core.Views.ViewServices
{
    public interface ICommunityIndexViewService
    {
        CommunityIndexVM Get();
        void Post(CommunityIndexVM data);
    }

    public class CommunityIndexViewService : ICommunityIndexViewService
    {
        ICommunityRepository _communityRepository;

        public CommunityIndexViewService(ICommunityRepository communityRepository)
        {
            _communityRepository = communityRepository;
        }

        public CommunityIndexVM Get()
        {
            var commItems = _communityRepository.GetAll().Select(o => new CommunityIndexItemVM { Name = o.Name, Id = o.Id }).ToList();
            return  new CommunityIndexVM { ItemViewModels = commItems };
        }

        public void Post(CommunityIndexVM data)
        {
            throw new NotImplementedException();
        }
    }

    public interface ICommunityCreateViewService
    {
        CommunityUpdateVM Get();
        Task<int> Post(string appUserId, CommunityUpdateVM data);
    }

    public class CommunityCreateViewService : ICommunityCreateViewService
    {
        ICommunityRepository _communityRepository;
        IMemberRepository _memberRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public CommunityCreateViewService(
            ICommunityRepository orgRepo, 
            IMemberRepository memberRepository,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        ){
            _communityRepository = orgRepo;
            _memberRepository = memberRepository;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public CommunityUpdateVM Get()
        {
            CommunityUpdateVM model = new CommunityUpdateVM
            {
                MemberProfileItemTemplates = Enumerable.Range(0, 5).Select(i => new MemberProfileItemTemplate()).ToList()
            };

            return model;
        }

        public async Task<int> Post(string appUserId, CommunityUpdateVM data)
        {
            Community org = new Community { Name = data.Name };
            _communityRepository.Add(org);
            await _communityRepository.CommitAsync();

            Member newMember = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(org.Id, appUserId);
            newMember.CommunityRole = RoleName.CommunityOwner;
            await _memberRepository.CommitAsync();

            var required = data.MemberProfileItemTemplates.Where(p => p.IsRequired == true).Select(p => p.ItemName);
            await _memberProfileItemTemplateRepository.AddRquiredTemplatesAsync(org.Id, required);

            var optional = data.MemberProfileItemTemplates.Where(p => p.IsRequired == false).Select(p => p.ItemName);
            await _memberProfileItemTemplateRepository.AddOptionalTemplatesAsync(org.Id, optional);

            await _memberProfileItemTemplateRepository.CommitAsync();

            return org.Id;
        }
    }


    public interface ICommunityEditViewService
    {
        CommunityUpdateVM Get(int orgId);
        Task Post(string appUserId, CommunityUpdateVM data);
    }

    public class CommunityEditViewService : ICommunityEditViewService
    {
        ICommunityRepository _communityRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public CommunityEditViewService(
            ICommunityRepository orgRepo,
            IMemberRepository memberRepository,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        ){
            _communityRepository = orgRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public CommunityUpdateVM Get(int orgId)
        {
            var org = _communityRepository.GetMany(o => o.Id == orgId).Include(o => o.MemberProfileItemTemplates).Single();

            CommunityUpdateVM result = new CommunityUpdateVM { Id = org.Id, Name = org.Name};
            result.MemberProfileItemTemplates = org.MemberProfileItemTemplates;

            return result;
        }

        public async Task Post(string appUserId, CommunityUpdateVM data)
        {
            var org = _communityRepository.GetById(data.Id);
            org.Name = data.Name;
            await _communityRepository.CommitAsync();

            _memberProfileItemTemplateRepository.Delete(t => t.CommunityId == org.Id);

            var required = data.MemberProfileItemTemplates.Where(t => t.IsRequired).Select(t => t.ItemName);
            await _memberProfileItemTemplateRepository.AddRquiredTemplatesAsync(org.Id, required);

            var optional = data.MemberProfileItemTemplates.Where(t => !t.IsRequired).Select(t => t.ItemName);
            await _memberProfileItemTemplateRepository.AddOptionalTemplatesAsync(org.Id, optional);

            await _communityRepository.CommitAsync();
        }
    }
}
