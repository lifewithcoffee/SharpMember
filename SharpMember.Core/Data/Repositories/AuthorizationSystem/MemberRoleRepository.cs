using SharpMember.Core.Data.Models.AuthorizationSystem;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Repositories.MemberSystem;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories.AuthorizationSystem
{
    public interface IMemberRoleRepository : IRepositoryBase<MemberRole, ApplicationDbContext>
    {
        void AddRole(int orgId, string roleName);

        void AddMember(int roleId, int memberId);
        void RemoveMember(int roldId, int memberId);
    }

    public class MemberRoleRepository : RepositoryBase<MemberRole, ApplicationDbContext>, IMemberRoleRepository
    {
        private readonly IMemberRepository _memberRepo;

        public MemberRoleRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork
            , ILogger logger
            , IMemberRepository memberRepo
        ) : base(unitOfWork, logger)
        {
            this._memberRepo = memberRepo;
        }

        public void AddMember(int roleId, int memberId)
        {
            Member member = this._memberRepo.GetById(memberId);
            member.MemberRoleId = roleId;
        }

        public void AddRole(int orgId, string roleName)
        {
            MemberRole role = new MemberRole { Name = roleName, OrganizationId = orgId };
            this.Add(role);
        }

        public void RemoveMember(int roldId, int memberId)
        {
            Member member = this._memberRepo.GetById(memberId);
            member.MemberRoleId = null;
        }
    }
}
