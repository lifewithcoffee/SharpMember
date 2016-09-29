using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Data.ServiceBase;
using SharpMember.Data.Models;

namespace SharpMember.Data.Services
{
    public class UserAdditionalInfoService : EfCoreServiceBase<UserAdditionalInfo>
    {
        IUnitOfWork<ApplicationDbContext> uow;

        public UserAdditionalInfoService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
        {
            uow = unitOfWork;
        }

        public UserAdditionalInfo GetByMemberNumber(int memberNumber)
        {
            return uow.Context.UserAdditionalInfo.Single(i => i.MemberNumber == memberNumber);
        }
    }
}
