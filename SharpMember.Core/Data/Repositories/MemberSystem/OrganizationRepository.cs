using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.MemberSystem;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IOrganizationRepository : IRepositoryBase<Organization, ApplicationDbContext> { }

    public class OrganizationRepository : RepositoryBase<Organization, ApplicationDbContext>, IOrganizationRepository
    {
        public OrganizationRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }
    }
}
