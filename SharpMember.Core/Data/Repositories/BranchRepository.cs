using SharpMember.Core.Data.Models;
using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories
{
    public interface IBranchRepository : IRepositoryBase<Branch, ApplicationDbContext> { }

    public class BranchRepository : RepositoryBase<Branch, ApplicationDbContext>, IBranchRepository
    {
        public BranchRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger)
        {
        }
    }
}
