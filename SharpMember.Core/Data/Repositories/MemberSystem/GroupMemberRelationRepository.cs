using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IGroupMemberRelationRepository : IRepositoryBase<GroupMemberRelation, ApplicationDbContext>
    {

    }

    public class GroupMemberRelationRepository : RepositoryBase<GroupMemberRelation, ApplicationDbContext>, IGroupMemberRelationRepository
    {
        public GroupMemberRelationRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<GroupMemberRelationRepository> logger) : base(unitOfWork, logger)
        {
        }
    }
}
