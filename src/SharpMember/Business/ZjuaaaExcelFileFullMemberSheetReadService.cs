using Microsoft.Extensions.Logging;
using Npoi.Core.SS.UserModel;
using SharpMember.Data.Models;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Business
{
    public interface IZjuaaaExcelFileFullMemberSheetReadService
    {
        bool ValidateFile();

        Member ReadRow(IWorkbook workbook, int rowNum);
    }

    public class ZjuaaaExcelFileFullMemberSheetReadService : IZjuaaaExcelFileFullMemberSheetReadService
    {
        ILogger _logger;

        Dictionary<string, int> dict = new Dictionary<string, int> {
            { nameof(MemberEntity.MemberNumber) , 0 },
            { nameof(MemberEntity.EnglishName)  , 3 },
            { nameof(MemberEntity.ChineseName)  , 4 },
            { nameof(MemberEntity.Email)        , 5 },
            { nameof(MemberEntity.Occupation)   , 6 },
            { nameof(MemberEntity.Wechat)       , 9 },
            { nameof(MemberEntity.QQ)           , 10},
            { nameof(MemberEntity.Skype)        , 11},
            { nameof(MemberEntity.RegisterDate) , 12},
            { nameof(MemberEntity.CeaseDate)    , 13},
            { nameof(MemberEntity.State)        , 14},
            { nameof(MemberEntity.Remarks)      , 15},
        };


        int[] phoneColumns = new int[] { 7, 8 };

        public ZjuaaaExcelFileFullMemberSheetReadService(ILogger logger)
        {
            _logger = logger;
        }

        public Member ReadRow(IWorkbook workbook, int rowNum)
        {
            Member result = null;

            try
            {
                var sheet = workbook.GetSheet("Associated Member");
                var row = sheet.GetRow(rowNum);
                if (row != null)
                {
                    result = new Member();

                    string cellValue = row.GetCell(dict[nameof(MemberEntity.MemberNumber)]).ToString();
                    if(!string.IsNullOrWhiteSpace(cellValue))
                    {
                        result.MemberNumber = int.Parse(cellValue);
                    }

                    result.EnglishName = row.GetCell(dict[nameof(MemberEntity.EnglishName)]).ToString();
                    result.ChineseName = row.GetCell(dict[nameof(MemberEntity.ChineseName)]).ToString();
                    result.Email = row.GetCell(dict[nameof(MemberEntity.Email)]).ToString();
                    result.Occupation = row.GetCell(dict[nameof(MemberEntity.Occupation)]).ToString();
                    result.Wechat = row.GetCell(dict[nameof(MemberEntity.Wechat)]).ToString();
                    result.QQ = row.GetCell(dict[nameof(MemberEntity.QQ)]).ToString();
                    result.Skype = row.GetCell(dict[nameof(MemberEntity.Skype)]).ToString();

                    string registerDate = row.GetCell(dict[nameof(MemberEntity.RegisterDate)]).ToString();
                    if(!string.IsNullOrWhiteSpace(registerDate))
                    {
                        DateTime parsedDate;
                        if (DateTime.TryParse(registerDate, out parsedDate))
                        {
                            result.RegisterDate = parsedDate;
                        }
                    }

                    string ceaseDate = row.GetCell(dict[nameof(MemberEntity.CeaseDate)]).ToString();
                    if (!string.IsNullOrWhiteSpace(ceaseDate))
                    {
                        DateTime parsedDate;
                        if(DateTime.TryParse(ceaseDate, out parsedDate))
                        {
                            result.CeaseDate = parsedDate;
                        }
                    }

                    result.State = row.GetCell(dict[nameof(MemberEntity.State)]).ToString();
                    result.Remarks = row.GetCell(dict[nameof(MemberEntity.Remarks)]).ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex);
                result = null;
            }

            return result;
        }

        public bool ValidateFile()
        {
            throw new NotImplementedException();
        }
    }
}
