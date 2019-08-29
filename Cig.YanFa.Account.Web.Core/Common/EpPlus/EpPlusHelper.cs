using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Cig.YanFa.Account.Web.Core.Common.EpPlus
{
    public class EpPlusHelper
    {
        public static void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.Length == 0)
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }
        public static void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }

        public static void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.Count == 0 || propertySelectors.Length == 0)
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }
            //自适应
            for (int i = 1; i <= propertySelectors.Length; i++)
            {
                sheet.Column(i).AutoFit();
            }
        }
    }
}
