using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberProfileItemTemplateRepository : IRepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>
    {
        IQueryable<MemberProfileItemTemplate> GetByOrganizationId(int orgId);
        Task<MemberProfileItemTemplate> AddTemplateAsync(int orgId, string itemName, bool isRequired = false);
        Task AddRquiredTemplatesAsync(int orgId, IEnumerable<string> itemNames);
        Task AddOptionalTemplatesAsync(int orgId, IEnumerable<string> itemNames);
    }

    public class MemberProfileItemTemplateRepository : RepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>, IMemberProfileItemTemplateRepository
    {
        public MemberProfileItemTemplateRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public IQueryable<MemberProfileItemTemplate> GetByOrganizationId(int orgId)
        {
            return this.GetMany(t => t.OrganizationId == orgId);
        }

        public async Task<MemberProfileItemTemplate> AddTemplateAsync(int orgId, string itemName, bool isRequired = false)
        {
            if(null == await this.UnitOfWork.Context.Organizations.FindAsync(orgId))
            {
                throw new OrganizationNotExistsException(orgId);
            }
            return Add(new MemberProfileItemTemplate { OrganizationId = orgId, ItemName = itemName, IsRequired = isRequired });
        }

        public async Task AddRquiredTemplatesAsync(int orgId, IEnumerable<string> itemNames)
        {
            foreach(var name in itemNames)
            {
                await this.AddTemplateAsync(orgId, name, true);
            }
        }

        public async Task AddOptionalTemplatesAsync(int orgId, IEnumerable<string> itemNames)
        {
            foreach(var name in itemNames)
            {
                await this.AddTemplateAsync(orgId, name, false);
            }
        }
    }
}
