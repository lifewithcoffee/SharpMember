using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberGroupRepository : IRepositoryBase<MemberGroup, ApplicationDbContext>
    {
        Task<MemberGroup> AddAsync(int orgId, string memberGroupName);
    }

    public class MemberGroupRepository : RepositoryBase<MemberGroup, ApplicationDbContext>, IMemberGroupRepository
    {
        private readonly IOrganizationRepository _organizationRepository;

        public MemberGroupRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork
            , ILogger logger
            , IOrganizationRepository organizationRepository
        ) : base(unitOfWork, logger)
        {
            this._organizationRepository = organizationRepository;
        }

        public override MemberGroup Add(MemberGroup entity)
        {
            throw new NotSupportedException("A MemberGroup must be added in an organization, use AddAsync(orgId, memberGroupName) instead.");
        }

        /// <summary>
        /// Precondition:
        /// - the organization must exist
        /// - the MemberGroup with the specified name must NOT exist
        /// </summary>
        public async Task<MemberGroup> AddAsync(int orgId, string memberGroupName)
        {
            if (!await _organizationRepository.ExistAsync(o => o.Id == orgId))
            {
                throw new OrganizationNotExistException($"Organization with Id {orgId} does not exist.");
            }

            if (await this.ExistAsync(m => m.Name == memberGroupName))
            {
                throw new MemberNameExistException($"MemberGroup with name {memberGroupName} already exists.");
            }

            return base.Add(new MemberGroup { Name = memberGroupName });
        }
    }
}
