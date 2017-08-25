using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using SharpMember.Core.Definitions;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberProfileItemTemplateRepository : IRepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>
    {
        IQueryable<MemberProfileItemTemplate> GetByCommunityId(int orgId);
        Task<MemberProfileItemTemplate> AddTemplateAsync(int orgId, string itemName, bool isRequired = false);
        Task AddRquiredTemplatesAsync(int orgId, IEnumerable<string> itemNames);
        Task AddOptionalTemplatesAsync(int orgId, IEnumerable<string> itemNames);
    }

    public class MemberProfileItemTemplateRepository : RepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>, IMemberProfileItemTemplateRepository
    {
        public MemberProfileItemTemplateRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<MemberProfileItemTemplateRepository> logger) : base(unitOfWork, logger) { }

        public IQueryable<MemberProfileItemTemplate> GetByCommunityId(int commId)
        {
            return this.GetMany(t => t.CommunityId == commId);
        }

        public async Task<MemberProfileItemTemplate> AddTemplateAsync(int orgId, string itemName, bool isRequired = false)
        {
            if(null == await this.UnitOfWork.Context.Communities.FindAsync(orgId))
            {
                throw new CommunityNotExistsException(orgId);
            }
            return Add(new MemberProfileItemTemplate { CommunityId = orgId, ItemName = itemName, IsRequired = isRequired });
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
