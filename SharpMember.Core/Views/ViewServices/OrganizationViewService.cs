using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Global;
using Microsoft.EntityFrameworkCore;

namespace SharpMember.Core.Views.ViewServices
{
    public interface IOrganizationIndexViewService
    {
        OrganizationIndexVM Get();
        void Post(OrganizationIndexVM data);
    }

    public class OrganizationIndexViewService : IOrganizationIndexViewService
    {
        IOrganizationRepository _organizationRepository;

        public OrganizationIndexViewService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public OrganizationIndexVM Get()
        {
            var orgItems = _organizationRepository.GetAll().Select(o => new OrganizationIndexItemVM { Name = o.Name, Id = o.Id }).ToList();
            return  new OrganizationIndexVM { ItemViewModels = orgItems };
        }

        public void Post(OrganizationIndexVM data)
        {
            throw new NotImplementedException();
        }
    }

    public interface IOrganizationCreateViewService
    {
        OrganizationCreateVM Get();
        Task Post(string appUserId, OrganizationCreateVM data);
    }

    public class OrganizationCreateViewService : IOrganizationCreateViewService
    {
        IOrganizationRepository _organizationRepository;
        IMemberRepository _memberRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public OrganizationCreateViewService(
            IOrganizationRepository orgRepo, 
            IMemberRepository memberRepository,
            IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
        ){
            _organizationRepository = orgRepo;
            _memberRepository = memberRepository;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public OrganizationCreateVM Get()
        {
            OrganizationCreateVM model = new OrganizationCreateVM
            {
                MemberProfileItemTemplates = Enumerable.Range(0, 5).Select(i => new MemberProfileItemTemplate()).ToList()
            };

            return model;
        }

        public async Task Post(string appUserId, OrganizationCreateVM data)
        {
            Organization org = new Organization { Name = data.Name };
            _organizationRepository.Add(org);
            await _organizationRepository.CommitAsync();

            Member newMember = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(org.Id, appUserId);
            newMember.OrganizationRole = RoleName.OrganizationOwner;
            await _memberRepository.CommitAsync();

            var required = data.MemberProfileItemTemplates.Where(p => p.IsRequired == true).Select(p => p.ItemName);
            await _memberProfileItemTemplateRepository.AddRquiredTemplatesAsync(org.Id, required);

            var optional = data.MemberProfileItemTemplates.Where(p => p.IsRequired == false).Select(p => p.ItemName);
            await _memberProfileItemTemplateRepository.AddOptionalTemplatesAsync(org.Id, optional);

            await _memberProfileItemTemplateRepository.CommitAsync();
        }
    }


    public interface IOrganizationEditViewService
    {
        OrganizationEditVM Get(int orgId);
        Task Post(string appUserId, OrganizationEditVM data);
    }

    public class OrganizationEditViewService : IOrganizationEditViewService
    {
        IOrganizationRepository _organizationRepository;
        //IMemberRepository _memberRepository;
        IMemberProfileItemTemplateRepository _memberProfileItemTemplateRepository;

        public OrganizationEditViewService(
           IOrganizationRepository orgRepo,
           IMemberRepository memberRepository,
           IMemberProfileItemTemplateRepository memberProfileItemTemplateRepository
       )
        {
            _organizationRepository = orgRepo;
            //_memberRepository = memberRepository;
            _memberProfileItemTemplateRepository = memberProfileItemTemplateRepository;
        }

        public OrganizationEditVM Get(int orgId)
        {
            var org = _organizationRepository.GetMany(o => o.Id == orgId).Include(o => o.MemberProfileItemTemplates).Single();

            OrganizationEditVM result = new OrganizationEditVM { Id = org.Id, Name = org.Name};
            result.MemberProfileItemTemplates = org.MemberProfileItemTemplates;

            return result;
        }

        public Task Post(string appUserId, OrganizationEditVM data)
        {
            throw new NotImplementedException();
        }
    }
}
