using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharpMember.Data.ServiceBase;
using SharpMember.Data.Models;
using SharpMember.Business;
using System.IO;
using Npoi.Core.SS.UserModel;
using Npoi.Core.XSSF.UserModel;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace SharpMember.Data.Services
{
    public interface IMemberService
    {
        Member GetByMemberNumber(int memberNumber);
        void ImportFromExcel();
    }

    public class MemberService : EfCoreServiceBase<Member, SqliteDbContext>, IMemberService
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        public MemberService(
            IUnitOfWork<SqliteDbContext> unitOfWork, 
            ILogger<MemberService> logger, 
            ILoggerFactory loggerFactory
        ) : base(unitOfWork) {
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public Member GetByMemberNumber(int memberNumber)
        {
            return this.UnitOfWork.Context.Members.Single(i => i.MemberNumber == memberNumber);
        }

        public void ImportFromExcel()
        {
            //var newFile = @"C:\_temp\校友会机构会员注册清单 revision 2016-12-21.xlsx";
            var newFile = @"C:\_temp\test.xlsx";

            using (var fs = new FileStream(newFile, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);  // NOTE the excel file MUST not contain comments, otherwise an exception will throw out

                var members = new ZjuaaaExcelFileFullMemberSheetReadService(
                    workbook, 
                    _loggerFactory.CreateLogger<IFullMemberSheetReadService>()
                ).ReadRow();
            }
        }
    }
}
