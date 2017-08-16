using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberGroupRoleRelationRepository : IRepositoryBase<MemberGroupRoleRelation, ApplicationDbContext>
    {

    }

    public class MemberGroupRoleRelationRepository : RepositoryBase<MemberGroupRoleRelation, ApplicationDbContext>, IMemberGroupRoleRelationRepository
    {
        public MemberGroupRoleRelationRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
