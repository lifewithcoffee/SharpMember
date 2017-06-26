using Npoi.Core.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Services.Excel
{
    public class AssociatedMemberPageModel
    {
    }

    public interface IAssociatedMemberPageReader
    {
        List<AssociatedMemberPageModel> Read(IWorkbook workbook);
    }

    public class AssociatedMemberPageReader : IAssociatedMemberPageReader
    {
        public List<AssociatedMemberPageModel> Read(IWorkbook workbook)
        {
            throw new NotImplementedException();
        }
    }
}
