using SharpMember.Core.Data.Repositories.AuthorizationSystem;
using SharpMember.Core.Data.Repositories.MemberSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Services
{
    public interface IAuthorizationService
    {
        Task<bool> ValidateMemberPermissionAsync(int memberId, string operation);
        bool HasOrganizationManagementPrivilege(int memberId, int orgId);
        bool HasGroupManagementPrivilege(int memberId, int groupId);
    }

    public class AuthorizationService : IAuthorizationService
    {
        IMemberRepository _memberRepo;
        IPermisstionRepository _permisstionRepo;

        public AuthorizationService(IMemberRepository memberRepo, IPermisstionRepository permisstionRepo)
        {
            this._memberRepo = memberRepo;
            this._permisstionRepo = permisstionRepo;
        }

        public bool HasGroupManagementPrivilege(int memberId, int groupId)
        {
            throw new NotImplementedException();
        }

        public bool HasOrganizationManagementPrivilege(int memberId, int orgId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateMemberPermissionAsync(int memberId, string operation)
        {
            int? memberRoleId = _memberRepo.GetById(memberId).MemberRoleId;
            bool permitted = await _permisstionRepo.ExistAsync(p => p.MemberRoleId == memberRoleId && p.Operation == operation);
            return permitted;
        }
    }
}
