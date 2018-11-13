using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;
using SharpMember.Core.Data.Models.MemberSystem;
using SharpMember.Core.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SharpMember.Core.Definitions;
using NetCoreUtils.Database;

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface ICommunityRepository : IRepositoryBase<Community>
    {
        Community Add(string name);
        void CancelMember(string appUserId);
    }

    public class CommunityRepository : RepositoryBase<Community, ApplicationDbContext>, ICommunityRepository
    {

        public CommunityRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork
        ) : base(unitOfWork)
        {
        }

        public Community Add(string name)
        {
            return this.Add(new Community { Name = name });
        }

        public void CancelMember(string appUserId)
        {
            throw new NotImplementedException();
        }
    }
}
