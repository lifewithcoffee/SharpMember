using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Data.ServiceBase;
using SharpMember.Data.Models;
using SharpMember.Business;

namespace SharpMember.Data.Services
{
    public class MemberService : EfCoreServiceBase<Member, SqliteDbContext>
    {
        IZjuaaaExcelFileFullMemberSheetReadService _zjuExcelSvc;

        public MemberService(IUnitOfWork<SqliteDbContext> unitOfWork) : base(unitOfWork) { }

        public Member GetByMemberNumber(int memberNumber)
        {
            return this.UnitOfWork.Context.Members.Single(i => i.MemberNumber == memberNumber);
        }

        public void ImportFromExcel()
        {

        }

    }
}
