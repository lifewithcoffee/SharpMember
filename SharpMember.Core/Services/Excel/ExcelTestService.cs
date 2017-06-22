using Npoi.Core.HSSF.Util;
using Npoi.Core.SS.UserModel;
using Npoi.Core.SS.Util;
using Npoi.Core.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SharpMember.Core.Services.Excel
{
    public class ExcelTestService
    {
        public void ReadFile()
        {
            //var newFile = @"C:\_temp\校友会机构会员注册清单 revision 2016-12-21.xlsx";
            var newFile = @"C:\_temp\test.xlsx";

            //using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            using (var fs = new FileStream(newFile, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);  // NOTE the excel file MUST not contain comments, otherwise an exception will throw out
                for(int s = 0; s < workbook.NumberOfSheets; s++)
                {
                    var sheet = workbook.GetSheetAt(s);
                    Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~ sheet name: " + sheet.SheetName + " ~~~~~~~~~~~~~~~~~~~~");
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);

                        if(null != row)
                        {
                            row.Cells.ForEach(c => {
                                Debug.Write(c.ToString() + " | ");
                            });
                        }
                        Debug.WriteLine("");
                    }
                }
            }
        }

        public void WriteFile()
        {
            var newFile = @"C:\_temp\test_write.xlsx";

            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {

                IWorkbook workbook = new XSSFWorkbook();

                ISheet sheet1 = workbook.CreateSheet("Sheet1");

                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                var rowIndex = 0;
                IRow row = sheet1.CreateRow(rowIndex);
                row.Height = 30 * 80;
                row.CreateCell(0).SetCellValue("这是单元格内容，可以设置很长，看能不能自动调整列宽");
                sheet1.AutoSizeColumn(0);
                rowIndex++;


                var sheet2 = workbook.CreateSheet("Sheet2");
                var style1 = workbook.CreateCellStyle();
                style1.FillForegroundColor = HSSFColor.Blue.Index2;
                style1.FillPattern = FillPattern.SolidForeground;

                var style2 = workbook.CreateCellStyle();
                style2.FillForegroundColor = HSSFColor.Yellow.Index2;
                style2.FillPattern = FillPattern.SolidForeground;

                var cell2 = sheet2.CreateRow(0).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(0);

                cell2 = sheet2.CreateRow(1).CreateCell(0);
                cell2.CellStyle = style2;
                cell2.SetCellValue(1);

                cell2 = sheet2.CreateRow(2).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(2);

                cell2 = sheet2.CreateRow(3).CreateCell(0);
                cell2.CellStyle = style2;
                cell2.SetCellValue(3);

                cell2 = sheet2.CreateRow(4).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(4);

                workbook.Write(fs);
            }
        }
    }
}
