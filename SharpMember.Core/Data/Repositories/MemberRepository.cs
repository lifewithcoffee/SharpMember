using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Core.Data.RepositoryBase;
using SharpMember.Core.Data.Models;
using SharpMember.Services.Excel;
using System.IO;
using Npoi.Core.SS.UserModel;
using Npoi.Core.XSSF.UserModel;
using Microsoft.Extensions.Logging;

namespace SharpMember.Core.Data.Repositories
{
    public interface IMemberRepository : IRepositoryBase<Member,SqliteDbContext>
    {
        Member GetByMemberNumber(int memberNumber);
        void ImportFromExcel();
    }

    public class MemberRepository : RepositoryBase<Member, SqliteDbContext>, IMemberRepository
    {
        private readonly IFullMemberSheetReadService _fullMemberSheetReadService;

        public MemberRepository(
            IUnitOfWork<SqliteDbContext> unitOfWork, 
            ILogger<MemberRepository> logger, 
            IFullMemberSheetReadService fullMemberSheetReadService): base(unitOfWork, logger)
        {
            this._fullMemberSheetReadService = fullMemberSheetReadService;
        }

        public Member GetByMemberNumber(int memberNumber)
        {
            return this._unitOfWork.Context.Members.Single(i => i.MemberNumber == memberNumber);
        }

        public void ImportFromExcel()
        {
            //var newFile = @"C:\_temp\校友会机构会员注册清单 revision 2016-12-21.xlsx";
            var newFile = @"C:\_temp\test.xlsx";

            using (var fs = new FileStream(newFile, FileMode.Open, FileAccess.Read))
            {
                var member = this._fullMemberSheetReadService.ReadRow(new XSSFWorkbook(fs)); // NOTICE: the excel file MUST not contain comments, otherwise an exception will throw out
            }
        }
    }
}
