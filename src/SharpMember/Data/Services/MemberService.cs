using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Data.ServiceBase;
using SharpMember.Data.Models;

namespace SharpMember.Data.Services
{
    public class MemberService : EfCoreServiceBase<Member>
    {
        IUnitOfWork<ApplicationDbContext> uow;

        public MemberService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
        {
            uow = unitOfWork;
        }

        public Member GetByMemberNumber(int memberNumber)
        {
            return uow.Context.Members.Single(i => i.MemberNumber == memberNumber);
        }
    }
}
