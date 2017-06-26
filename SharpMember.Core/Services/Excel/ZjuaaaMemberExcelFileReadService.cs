using Microsoft.Extensions.Logging;
using Npoi.Core.SS.UserModel;
using Npoi.Core.XSSF.UserModel;
using SharpMember.Core.Data.Models;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Core.Services.Excel
{
    public interface IZjuaaaMemberExcelFileReadService
    {
        void ImportFromExcel(string filePath);
    }

    public class ZjuaaaMemberExcelFileReadService : IZjuaaaMemberExcelFileReadService
    {
        ILogger _logger;
        IFullMemberPageReader _fullMemberPageReader;
        IAssociatedMemberPageReader _associatedMemberPageReader;

        public ZjuaaaMemberExcelFileReadService(
            ILogger<ZjuaaaMemberExcelFileReadService> logger,
            IFullMemberPageReader fullMemberPageReader,
            IAssociatedMemberPageReader associatedMemberPageReader )
        {
            this._logger = logger;
            this._fullMemberPageReader = fullMemberPageReader;
            this._associatedMemberPageReader = associatedMemberPageReader;
        }

        public void ImportFromExcel(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var member = this._fullMemberPageReader.Read(new XSSFWorkbook(fs)); // NOTICE: the excel file MUST not contain comments, otherwise an exception will throw out
            }
        }

        private List<FullMemberPageModel> ReadAssociatedMembers(IWorkbook workbook)
        {
            throw new NotImplementedException();
        }

        

        public bool ValidateFile()
        {
            throw new NotImplementedException();
        }
    }
}
