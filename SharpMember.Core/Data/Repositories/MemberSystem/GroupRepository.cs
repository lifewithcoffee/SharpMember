using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;
using System.Threading.Tasks;
using SharpMember.Core.Definitions;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IGroupRepository : IRepositoryBase<Group, ApplicationDbContext>
    {
        Task<Group> AddWithExceptionAsync(int orgId, string memberGroupName);
    }

    public class GroupRepository : RepositoryBase<Group, ApplicationDbContext>, IGroupRepository
    {
        private readonly IOrganizationRepository _organizationRepository;

        public GroupRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<GroupRepository> logger,
            IOrganizationRepository organizationRepository
        ) : base(unitOfWork, logger)
        {
            this._organizationRepository = organizationRepository;
        }

        public override Group Add(Group entity)
        {
            throw new NotSupportedException("A MemberGroup must be added in an organization, use AddAsync(orgId, memberGroupName) instead.");
        }

        /// <summary>
        /// Precondition:
        /// - the organization must exist
        /// - the MemberGroup with the specified name must NOT exist
        /// </summary>
        public async Task<Group> AddWithExceptionAsync(int orgId, string memberGroupName)
        {
            if (!await _organizationRepository.ExistAsync(o => o.Id == orgId))
            {
                throw new OrganizationNotExistsException(orgId);
            }

            if (await this.ExistAsync(m => m.Name == memberGroupName))
            {
                throw new MemberNameExistsException($"MemberGroup with name {memberGroupName} already exists.");
            }

            return base.Add(new Group { Name = memberGroupName });
        }
    }
}
