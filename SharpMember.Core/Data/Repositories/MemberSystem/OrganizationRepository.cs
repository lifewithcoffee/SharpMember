using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SharpMember.Core.Global;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IOrganizationRepository : IRepositoryBase<Organization, ApplicationDbContext>
    {
        Organization Add(string name);
        Task<Organization> CreateCommittedAsync(string appUserId, string name);
        void CancelMember(string appUserId);
    }

    public class OrganizationRepository : RepositoryBase<Organization, ApplicationDbContext>, IOrganizationRepository
    {
        IMemberRepository _memberRepository;

        public OrganizationRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<OrganizationRepository> logger,
            IMemberRepository memberRepository
        ) : base(unitOfWork, logger)
        {
            this._memberRepository = memberRepository;
        }

        public Organization Add(string name)
        {
            return this.Add(new Organization { Name = name });
        }

        public async Task<Organization> CreateCommittedAsync(string appUserId, string name)
        {
            Organization org = new Organization { Name = name };
            this.Add(org);
            await this.CommitAsync();

            Member newMember = await this._memberRepository.GenerateNewMemberWithProfileItemsAsync(org.Id, appUserId);
            newMember.OrganizationRole = RoleName.OrganizationOwner;
            await this.CommitAsync();

            return org;
        }

        public void CancelMember(string appUserId)
        {
            throw new NotImplementedException();
        }
    }
}
