using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Views.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
}
