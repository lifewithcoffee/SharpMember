using SharpMember.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Business
{
    public interface IZjuaaaExcelNameListService
    {
        bool ValidateFile();

        /// <param name="rowNum">Start from 1</param>
        Member ReadFullMemberSheetRow(int rowNum);

        /// <param name="rowNum">Start from 1</param>
        Member ReadAssociatedSheetRow(int rowNum);
    }

    public class ZjuaaaExcelNameListService : IZjuaaaExcelNameListService
    {
        public Member ReadAssociatedSheetRow(int rowNum)
        {
            throw new NotImplementedException();
        }

        public Member ReadFullMemberSheetRow(int rowNum)
        {
            throw new NotImplementedException();
        }

        public bool ValidateFile()
        {
            throw new NotImplementedException();
        }
    }
}
