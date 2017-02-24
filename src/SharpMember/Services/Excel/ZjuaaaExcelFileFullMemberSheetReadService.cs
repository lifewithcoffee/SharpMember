using Microsoft.Extensions.Logging;
using Npoi.Core.SS.UserModel;
using SharpMember.Data.Models;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Services.Excel
{
    public interface IZjuaaaExcelFileMemberReadService
    {
        bool ValidateFile();

        List<Member> ReadRow(IWorkbook workbook);
    }

    public interface IFullMemberSheetReadService : IZjuaaaExcelFileMemberReadService { }

    public class ZjuaaaExcelFileFullMemberSheetReadService : IFullMemberSheetReadService
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
        int[] NormalizedNameColumns = new int[] { 1, 2 };   // column 1: Family Name; column 2: Given Name

        public ZjuaaaExcelFileFullMemberSheetReadService(ILogger<ZjuaaaExcelFileFullMemberSheetReadService> logger)
        {
            _logger = logger;
        }

        public List<Member> ReadRow(IWorkbook workbook)
        {
            List<Member> result = new List<Member>();

            ISheet sheet = workbook.GetSheet("Full Member");
            int currentRowNumber = 0;
            try
            {
                for (int rowNum = sheet.FirstRowNum + 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    currentRowNumber = rowNum;
                    var row = sheet.GetRow(rowNum);
                    if (row != null)
                    {
                        Member member = new Member();

                        ICell cell = row.GetCell(dict[nameof(MemberEntity.MemberNumber)]);
                        string cellValue = cell == null ? "" : cell.ToString();
                        if (!string.IsNullOrWhiteSpace(cellValue))
                        {
                            int number;
                            if (int.TryParse(cellValue, out number))
                            {
                                member.MemberNumber = number;
                            }
                        }
                        else
                        {
                            member.MemberNumber = 0;
                        }

                        cell = row.GetCell(dict[nameof(MemberEntity.EnglishName)]);
                        member.EnglishName = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(MemberEntity.ChineseName)]);
                        member.ChineseName = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(MemberEntity.Email)]);
                        member.Email = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(MemberEntity.Occupation)]);
                        member.Occupation = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(MemberEntity.Wechat)]);
                        member.Wechat = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(MemberEntity.QQ)]);
                        member.QQ = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(MemberEntity.Skype)]);
                        member.Skype = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(MemberEntity.RegisterDate)]);
                        string registerDate = cell == null ? "" : cell.ToString();
                        if (!string.IsNullOrWhiteSpace(registerDate))
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParse(registerDate, out parsedDate))
                            {
                                member.RegisterDate = parsedDate;
                            }
                        }
                        else
                        {
                            member.RegisterDate = null;
                        }

                        cell = row.GetCell(dict[nameof(MemberEntity.CeaseDate)]);
                        string ceaseDate = cell == null ? "" : cell.ToString();
                        if (!string.IsNullOrWhiteSpace(ceaseDate))
                        {
                            DateTime parsedDate;
                            if (DateTime.TryParse(ceaseDate, out parsedDate))
                            {
                                member.CeaseDate = parsedDate;
                            }
                        }
                        else
                        {
                            member.CeaseDate = null;
                        }

                        cell = row.GetCell(dict[nameof(MemberEntity.State)]);
                        member.State = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(MemberEntity.Remarks)]);
                        member.Remarks = cell == null ? "" : cell.ToString();

                        StringBuilder sb = new StringBuilder();

                        foreach(int idx in phoneColumns)
                        {
                            cell = row.GetCell(idx);
                            if(cell != null)
                            {
                                sb.Append(cell.ToString());
                                sb.Append(";");
                            }
                        }

                        member.Phone = sb.ToString();
                        if(member.Phone.Length > 0)
                        {
                            member.Phone = member.Phone.Remove(member.Phone.Length - 1);    // remove the last ';'
                        }

                        foreach(int idx in NormalizedNameColumns)
                        {
                            cell = row.GetCell(idx);
                            if(cell != null)
                            {
                                sb.Append(cell.ToString());
                                sb.Append(',');
                            }
                        }
                        member.NormalizedName = sb.ToString();
                        if(member.NormalizedName.Length > 0)
                        {
                            member.NormalizedName = member.NormalizedName.Remove(member.NormalizedName.Length - 1); // remove the last ','
                        }

                        // if contains any of those names
                        if(     !string.IsNullOrWhiteSpace(member.NormalizedName)
                            ||  !string.IsNullOrWhiteSpace(member.ChineseName)
                            ||  !string.IsNullOrWhiteSpace(member.EnglishName)
                        ){
                            result.Add(member);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex, string.Format("Row number: {0}", currentRowNumber));
            }

            return result;
        }

        public bool ValidateFile()
        {
            throw new NotImplementedException();
        }
    }
}
