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
        IQueryable<MemberProfileItemTemplate> GetByCommunityId(int commId);
        Task<MemberProfileItemTemplate> AddTemplateAsync(int commId, string itemName, bool isRequired);
        Task AddTemplatesAsync(int orgId, IEnumerable<string> itemNames, bool isRequired);
        void UpdateItemTemplates(int commId, IList<MemberProfileItemTemplate> newTemplates);
    }

    public class MemberProfileItemTemplateRepository : RepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>, IMemberProfileItemTemplateRepository
    {
        public MemberProfileItemTemplateRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<MemberProfileItemTemplateRepository> logger) : base(unitOfWork, logger) { }

        public class MemberProfileItemTemplateIdComparer : IEqualityComparer<MemberProfileItemTemplate>
        {
            public bool Equals(MemberProfileItemTemplate x, MemberProfileItemTemplate y)
            {
                if (x.Id == y.Id)
                {
                    return true;
                }

                return false;
            }

            public int GetHashCode(MemberProfileItemTemplate obj)
            {
                return obj.GetHashCode();
            }
        }

        public IQueryable<MemberProfileItemTemplate> GetByCommunityId(int commId)
        {
            return this.GetMany(t => t.CommunityId == commId);
        }

        public async Task<MemberProfileItemTemplate> AddTemplateAsync(int commId, string itemName, bool isRequired)
        {
            if(null == await this.UnitOfWork.Context.Communities.FindAsync(commId))
            {
                throw new CommunityNotExistsException(commId);
            }
            return Add(new MemberProfileItemTemplate { CommunityId = commId, ItemName = itemName, IsRequired = isRequired });
        }

        public async Task AddTemplatesAsync(int commId, IEnumerable<string> itemNames, bool isRequired)
        {
            var nonWhiteSpaceItemNames = itemNames.Where(i => !string.IsNullOrWhiteSpace(i));
            foreach(var name in nonWhiteSpaceItemNames)
            {
                await this.AddTemplateAsync(commId, name, isRequired);
            }
        }

        public void UpdateItemTemplates(int commId, IList<MemberProfileItemTemplate> newTemplates)
        {
            var community = this.UnitOfWork.Context.Communities.Find(commId);
            if(null == community)
            {
                throw new CommunityNotExistsException(commId);
            }

            foreach(var template in newTemplates)
            {
                if(template.CommunityId <= 0)
                {
                    template.CommunityId = commId;
                }
                else if(template.CommunityId != commId)
                {
                    throw new CommunityIdMismatchesException(commId, template.CommunityId);
                }
            }

            IList<MemberProfileItemTemplate> oldTemplates = community.MemberProfileItemTemplates;

            this.DeleteRange(oldTemplates.Except(newTemplates, new MemberProfileItemTemplateIdComparer()));
            this.UpdateRange(newTemplates.Intersect(oldTemplates, new MemberProfileItemTemplateIdComparer()));
            this.AddRange(newTemplates.Except(oldTemplates, new MemberProfileItemTemplateIdComparer()));
        }
    }
}
