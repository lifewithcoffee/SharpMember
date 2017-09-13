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
    { }

    public class GroupRepository : RepositoryBase<Group, ApplicationDbContext>, IGroupRepository
    {
        public GroupRepository( IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<GroupRepository> logger ) : base(unitOfWork, logger)
        { }
    }
}
