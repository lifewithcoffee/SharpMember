using SharpMember.Core.Data.RepositoryBase;
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

namespace SharpMember.Core.Data.Repositories.MemberSystem
{
    public interface ICommunityRepository : IRepositoryBase<Community, ApplicationDbContext>
    {
        Community Add(string name);
        void CancelMember(string appUserId);
    }

    public class CommunityRepository : RepositoryBase<Community, ApplicationDbContext>, ICommunityRepository
    {
        IMemberRepository _memberRepository;

        public CommunityRepository(
            IUnitOfWork<ApplicationDbContext> unitOfWork,
            ILogger<CommunityRepository> logger,
            IMemberRepository memberRepository
        ) : base(unitOfWork, logger)
        {
            this._memberRepository = memberRepository;
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
