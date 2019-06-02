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
    public interface IMemberProfileItemTemplateRepository
    {
        IQueryable<MemberProfileItemTemplate> GetByCommunityId(int commId);
        Task<MemberProfileItemTemplate> AddTemplateAsync(int commId, string itemName, bool isRequired);
        Task AddTemplatesAsync(int orgId, IEnumerable<string> itemNames, bool isRequired);
        void AddOrUpdateItemTemplates(int commId, IList<MemberProfileItemTemplate> newTemplates);
        IRepositoryBase<MemberProfileItemTemplate> Repo { get; }
    }

    public class MemberProfileItemTemplateRepository : IMemberProfileItemTemplateRepository
    {
        IRepositoryBase<MemberProfileItemTemplate> _repo;
        IRepositoryBase<Community> _communityRepo;

        public MemberProfileItemTemplateRepository(
            IRepositoryBase<MemberProfileItemTemplate> repo,
            IRepositoryBase<Community> communityRepo
            )
        {
            _repo = repo;
            _communityRepo = communityRepo;
        }

        public IRepositoryBase<MemberProfileItemTemplate> Repo { get { return _repo; } }

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
            return _repo.GetMany(t => t.CommunityId == commId);
        }

        public async Task<MemberProfileItemTemplate> AddTemplateAsync(int commId, string itemName, bool isRequired)
        {
            if(!await _communityRepo.ExistAsync(c => c.Id == commId))
            {
                throw new CommunityNotExistsException(commId);
            }
            return _repo.Add(new MemberProfileItemTemplate { CommunityId = commId, ItemName = itemName, IsRequired = isRequired });
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
            if(null == _communityRepo.GetById(commId))
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

            _repo.UpdateRange(newTemplates);
        }
    }
}
