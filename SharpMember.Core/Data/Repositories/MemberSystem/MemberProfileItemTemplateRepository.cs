using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberProfileItemTemplateRepository : IRepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>
    {
        Task<MemberProfileItemTemplate> AddWithExceptionAsync(int orgId, string itemName, bool isRequired = false);
    }

    public class MemberProfileItemTemplateRepository : RepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>, IMemberProfileItemTemplateRepository
    {
        public MemberProfileItemTemplateRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public async Task<MemberProfileItemTemplate> AddWithExceptionAsync(int orgId, string itemName, bool isRequired = false)
        {
            if(null == await this.UnitOfWork.Context.Organizations.FindAsync(orgId))
            {
                throw new OrganizationNotExistException(orgId);
            }
            return Add(new MemberProfileItemTemplate { OrganizationId = orgId, ItemName = itemName, IsRequired = isRequired });
        }
    }
}
