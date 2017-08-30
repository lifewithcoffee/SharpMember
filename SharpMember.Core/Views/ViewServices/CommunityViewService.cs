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

    public interface ICommunityMembersViewService
    {
        CommunityMembersVM Get(int orgId);
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
                .Select(m => new CommunityMemberItemVM { Id = m.Id, Name = m.Name, MemberNumber = m.MemberNumber, Renewed = m.Renewed })
                .ToList();

            return new CommunityMembersVM { CommunityId = commId, ItemViewModels = items };
        }

        public async Task PostToDeleteSelected(CommunityMembersVM data)
        {
            data.ItemViewModels.ForEach(i => _memberRepo.Delete(m => m.Id == i.Id));
            await _memberRepo.CommitAsync();
        }
    }

    public interface ICommunityCreateViewService
    {
        CommunityUpdateVM Get();
        Task<int> PostAsync(string appUserId, CommunityUpdateVM data);
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

        public async Task<int> PostAsync(string appUserId, CommunityUpdateVM data)
        {
            Community community = new Community { Name = data.Name };
            _communityRepository.Add(community);
            await _communityRepository.CommitAsync();

            Member newMember = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(community.Id, appUserId);
            newMember.CommunityRole = RoleName.CommunityOwner;
            await _memberRepository.CommitAsync();

            var required = data.MemberProfileItemTemplates.Where(p => p.IsRequired == true).Select(p => p.ItemName);
            await _memberProfileItemTemplateRepository.AddTemplatesAsync(community.Id, required, true);

            var optional = data.MemberProfileItemTemplates.Where(p => p.IsRequired == false).Select(p => p.ItemName);
            await _memberProfileItemTemplateRepository.AddTemplatesAsync(community.Id, optional, false);

            await _memberProfileItemTemplateRepository.CommitAsync();

            return community.Id;
        }
    }


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
        ){
            _communityRepository = orgRepo;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public CommunityUpdateVM Get(int commId)
        {
            var org = _communityRepository.GetMany(o => o.Id == commId).Include(o => o.MemberProfileItemTemplates).Single();

            CommunityUpdateVM result = new CommunityUpdateVM { Id = org.Id, Name = org.Name};
            result.MemberProfileItemTemplates = org.MemberProfileItemTemplates;

            return result;
        }

        public async Task PostAsync(CommunityUpdateVM data)
        {
            var community = _communityRepository.GetById(data.Id);
            community.Name = data.Name;
            await _communityRepository.CommitAsync();

            _memberProfileItemTemplateRepository.UpdateItemTemplates(data.Id, data.MemberProfileItemTemplates);
            await _communityRepository.CommitAsync();
        }
    }
}
