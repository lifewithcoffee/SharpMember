using SharpMember.Data.Models;
using SharpMember.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SharpMember.Data.Repositories
{
    public interface IMemberProfileItemRepository : IRepositoryBase<MemberProfileItem, SqliteDbContext>
    { }

    public class MemberProfileItemRepository : RepositoryBase<MemberProfileItem, SqliteDbContext>, IMemberProfileItemRepository
    {
        public MemberProfileItemRepository(IUnitOfWork<SqliteDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }
    }
}
