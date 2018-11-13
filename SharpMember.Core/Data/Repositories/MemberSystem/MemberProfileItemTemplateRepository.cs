using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using SharpMember.Core.Definitions;
using AutoMapper;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberProfileItemTemplateRepository : IRepositoryBase<MemberProfileItemTemplate>
    {
        IQueryable<MemberProfileItemTemplate> GetByCommunityId(int commId);
        Task<MemberProfileItemTemplate> AddTemplateAsync(int commId, string itemName, bool isRequired);
        Task AddTemplatesAsync(int orgId, IEnumerable<string> itemNames, bool isRequired);
        void AddOrUpdateItemTemplates(int commId, IList<MemberProfileItemTemplate> newTemplates);
    }

    public class MemberProfileItemTemplateRepository : RepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>, IMemberProfileItemTemplateRepository
    {
        IRepositoryRead<Community> _communityReader;

        public MemberProfileItemTemplateRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            IRepositoryRead<Community> communityReade
        ) : base(unitOfWork)
        {
            _communityReader = communityReade;
        }

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
            if(!await _communityReader.ExistAsync(c => c.Id == commId))
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

        /// <summary>
        /// MemberProfileItemTemplate items in <paramref name="newTemplates"/> with:
        ///     * Valid Id values will be updated
        ///     * Invalid Id values will be added
        /// </summary>
        public void AddOrUpdateItemTemplates(int commId, IList<MemberProfileItemTemplate> newTemplates)
        {
            var community = _communityReader.GetById(commId);
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

            this.UpdateRange(newTemplates);
        }
    }
}
