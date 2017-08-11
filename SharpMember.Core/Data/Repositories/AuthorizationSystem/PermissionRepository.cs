using SharpMember.Core.Data.Models.AuthorizationSystem;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SharpMember.Core.Data.Repositories.AuthorizationSystem
{
    public interface IPermisstionRepository : IRepositoryBase<Permission, ApplicationDbContext>
    {
        Task<bool> OperationEnabledAsync(int memberRoldId, string operation);
    }

    public class PermissionRepository : RepositoryBase<Permission, ApplicationDbContext>, IPermisstionRepository
    {
        private readonly MemberRoleRepository _memberRoleRepo;

        public PermissionRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }

        public async Task<bool> OperationEnabledAsync(int memberRoldId, string operation)
        {
            bool permitted = await this.ExistAsync(p => p.MemberRoleId == memberRoldId && p.Operation == operation);
            return permitted;
        }
    }
}
