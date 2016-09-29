using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Data.ServiceBase;
using SharpMember.Data.Models;

namespace SharpMember.Data.Services
{
    public class UserService : EfCoreServiceBase<UserAdditionalInfo>
    {
        IUnitOfWork<ApplicationDbContext> uow;

        public UserService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
        {
            uow = unitOfWork;
        }

        /// <param name="id">The ID number here is UserAdditionalInfo.MemberNumber</param>
        /// <returns></returns>
        public override UserAdditionalInfo GetById(int id)
        {
            return uow.Context.UserAdditionalInfo.Single(i => i.MemberNumber == id);
        }
    }
}
