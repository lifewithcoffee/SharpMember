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

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IOrganizationRepository : IRepositoryBase<Organization, ApplicationDbContext>
    {
        Organization Add(string name);
        Task<Organization> CreateAsync(string appUserId, string name);
        Member RegisterMember(string appUserId);
        void CancelMember(string appUserId);
    }

    public class OrganizationRepository : RepositoryBase<Organization, ApplicationDbContext>, IOrganizationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork
            , ILogger<OrganizationRepository> logger
            , UserManager<ApplicationUser> userManager
        ) : base(unitOfWork, logger)
        {
            this._userManager = userManager;
        }

        public Organization Add(string name)
        {
            return this.Add(new Organization { Name = name });
        }

        public async Task<Organization> CreateAsync(string appUserId, string name)
        {
            ApplicationUser appUser = await this._userManager.FindByIdAsync(appUserId);
            Member newMember = this.RegisterMember(appUserId);
            Organization org = new Organization { Name = name };

            org.Members.Add(newMember);
            throw new NotImplementedException();
        }

        public Member RegisterMember(string appUserId)
        {
            throw new NotImplementedException();
        }

        public void CancelMember(string appUserId)
        {
            throw new NotImplementedException();
        }
    }
}
