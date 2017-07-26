using SharpMember.Core.Data.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface IMemberProfileItemTemplateRepository : IRepositoryBase<MemberProfileItemTemplate, ApplicationDbContext> { }

    public class MemberProfileItemTemplateRepository : RepositoryBase<MemberProfileItemTemplate, ApplicationDbContext>, IMemberProfileItemTemplateRepository
    {
        public MemberProfileItemTemplateRepository(IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger logger) : base(unitOfWork, logger) { }
    }
}
