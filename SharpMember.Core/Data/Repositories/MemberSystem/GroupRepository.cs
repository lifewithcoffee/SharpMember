using SharpMember.Core.Data.Models;
using NetCoreUtils.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;
using System.Threading.Tasks;
using SharpMember.Core.Definitions;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IGroupRepository : IRepositoryBase<Group>
    { }

    public class GroupRepository : RepositoryBase<Group, ApplicationDbContext>, IGroupRepository
    {
        public GroupRepository( IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
        { }
    }
}
