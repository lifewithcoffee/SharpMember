using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Global;

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
        Task Post(string appUserId, string name);
    }

    public class OrganizationCreateViewService : IOrganizationCreateViewService
    {
        IOrganizationRepository _organizationRepository;
        IMemberRepository _memberRepository;

        public OrganizationCreateViewService(IOrganizationRepository orgRepo, IMemberRepository memberRepository)
        {
            _organizationRepository = orgRepo;
            _memberRepository = memberRepository;
        }

        public OrganizationCreateVM Get()
        {
            OrganizationCreateVM model = new OrganizationCreateVM
            {
                MemberProfileItemTemplates = Enumerable.Range(0, 5).Select(i => new MemberProfileItemTemplate()).ToList()
            };

            return model;
        }

        public async Task Post(string appUserId, string name)
        {
            Organization org = new Organization { Name = name };
            _organizationRepository.Add(org);
            await _organizationRepository.CommitAsync();

            Member newMember = await _memberRepository.GenerateNewMemberWithProfileItemsAsync(org.Id, appUserId);
            newMember.OrganizationRole = RoleName.OrganizationOwner;
            await _memberRepository.CommitAsync();
        }
    }
}
