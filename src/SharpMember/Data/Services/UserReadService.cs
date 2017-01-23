using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Data.ServiceBase;
using SharpMember.Data.Models;

namespace SharpMember.Data.Services
{
    public class UserReadService : EfCoreServiceBase<ApplicationUser>
    {
        IUnitOfWork<ApplicationDbContext> uow;

        public UserReadService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
        {
            uow = unitOfWork;
        }

        public ApplicationUser GetByMemberNumber(int memberNumber)
        {
            return uow.Context.Users.Single(i => i.MemberNumber == memberNumber);
        }
    }
}
