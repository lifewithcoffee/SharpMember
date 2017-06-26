using Microsoft.Extensions.Logging;
using Npoi.Core.SS.UserModel;
using SharpMember.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Services.Excel
{
    public class FullMemberPageModel
    {
        public int MemberNumber { get; set; }
        public bool Renewed { get; set; }
        public string NormalizedName { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string Organization { get; set; }
        public string Occupation { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string EducationalLevel { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Birthplace { get; set; }
        public string Wechat { get; set; }
        public string QQ { get; set; }
        public string Skype { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? CeaseDate { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Remarks { get; set; }
    }

    public interface IFullMemberPageReader
    {
        List<FullMemberPageModel> Read(IWorkbook workbook);
    }

    public class FullMemberPageReader : IFullMemberPageReader
    {
        Dictionary<string, int> dict = new Dictionary<string, int> {
            { nameof(FullMemberPageModel.MemberNumber) , 0 },
            { nameof(FullMemberPageModel.EnglishName)  , 3 },
            { nameof(FullMemberPageModel.ChineseName)  , 4 },
            { nameof(FullMemberPageModel.Email)        , 5 },
            { nameof(FullMemberPageModel.Occupation)   , 6 },
            { nameof(FullMemberPageModel.Wechat)       , 9 },
            { nameof(FullMemberPageModel.QQ)           , 10},
            { nameof(FullMemberPageModel.Skype)        , 11},
            { nameof(FullMemberPageModel.RegisterDate) , 12},
            { nameof(FullMemberPageModel.CeaseDate)    , 13},
            { nameof(FullMemberPageModel.State)        , 14},
            { nameof(FullMemberPageModel.Remarks)      , 15},
        };

        int[] phoneColumns = new int[] { 7, 8 };
        int[] NormalizedNameColumns = new int[] { 1, 2 };   // column 1: Family Name; column 2: Given Name

        ILogger _logger;

        public FullMemberPageReader(ILogger<ZjuaaaMemberExcelFileReadService> logger)
        {
            this._logger = logger;
        }

        public List<FullMemberPageModel> Read(IWorkbook workbook)
        {
            List<FullMemberPageModel> result = new List<FullMemberPageModel>();

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
                        FullMemberPageModel member = new FullMemberPageModel();

                        ICell cell = row.GetCell(dict[nameof(FullMemberPageModel.MemberNumber)]);
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

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.EnglishName)]);
                        member.EnglishName = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.ChineseName)]);
                        member.ChineseName = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.Email)]);
                        member.Email = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.Occupation)]);
                        member.Occupation = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.Wechat)]);
                        member.Wechat = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.QQ)]);
                        member.QQ = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.Skype)]);
                        member.Skype = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.RegisterDate)]);
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

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.CeaseDate)]);
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

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.State)]);
                        member.State = cell == null ? "" : cell.ToString();

                        cell = row.GetCell(dict[nameof(FullMemberPageModel.Remarks)]);
                        member.Remarks = cell == null ? "" : cell.ToString();

                        StringBuilder sb = new StringBuilder();

                        foreach (int idx in phoneColumns)
                        {
                            cell = row.GetCell(idx);
                            if (cell != null)
                            {
                                sb.Append(cell.ToString());
                                sb.Append(";");
                            }
                        }

                        member.Phone = sb.ToString();
                        if (member.Phone.Length > 0)
                        {
                            member.Phone = member.Phone.Remove(member.Phone.Length - 1);    // remove the last ';'
                        }

                        foreach (int idx in NormalizedNameColumns)
                        {
                            cell = row.GetCell(idx);
                            if (cell != null)
                            {
                                sb.Append(cell.ToString());
                                sb.Append(',');
                            }
                        }
                        member.NormalizedName = sb.ToString();
                        if (member.NormalizedName.Length > 0)
                        {
                            member.NormalizedName = member.NormalizedName.Remove(member.NormalizedName.Length - 1); // remove the last ','
                        }

                        // if contains any of those names
                        if (!string.IsNullOrWhiteSpace(member.NormalizedName)
                            || !string.IsNullOrWhiteSpace(member.ChineseName)
                            || !string.IsNullOrWhiteSpace(member.EnglishName)
                        )
                        {
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
    }
}
