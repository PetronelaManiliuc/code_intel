using System.Collections.Generic;
using System.Globalization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Kata_Invoicing
{
    public static class CellStyleUtil
    {
        public enum CellType
        {
            Text,
            Date,
            Number,
            Number6Decimals,
            Number4Decimals,
            Number2Decimals
        }
        public enum Bold
        {
            NotBolded,
            Bolded
        }
        public enum Border
        {
            None,
            Top,
            Bottom,
            Left,
            Right,
            TopBottom,
            LeftRight,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            TopLeftRight,
            TopLeftBottom,
            TopRightBottom,
            BottomLeftRight,
            All
        }

        public static Dictionary<string, ICellStyle> Styles = new Dictionary<string, ICellStyle>();

        public static ICellStyle GetCellStyleKey(HSSFWorkbook book, CellStyleUtil.CellType cellType, CellStyleUtil.Bold cellBold, CellStyleUtil.Border cellBorder, BorderStyle borderStyle, string currencySymbol)
        {
            string styleKey = "";
            styleKey = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", cellType, cellBold, currencySymbol, cellBorder, borderStyle, book.GetHashCode());
            ICellStyle style;
            IFont font;
            IDataFormat format;

            if (!Styles.ContainsKey(styleKey))
            {
                style = book.CreateCellStyle();
                switch (cellType)
                {
                    case CellStyleUtil.CellType.Text:
                        break;
                    case CellStyleUtil.CellType.Date:
                        format = book.CreateDataFormat();
                        string datePatten = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                        style.DataFormat = format.GetFormat(datePatten);
                        break;
                    case CellStyleUtil.CellType.Number:
                        if (!string.IsNullOrEmpty(currencySymbol))
                        {
                            string formatPattern = string.Format(Constants.ExcelAmountsFormatPattern, currencySymbol);
                            format = book.CreateDataFormat();
                            style.DataFormat = format.GetFormat(formatPattern);
                        }
                        break;
                    case CellStyleUtil.CellType.Number2Decimals:
                        style.DataFormat = HSSFDataFormat.GetBuiltinFormat(Constants.ExcelAmountsNoCurrencyFormatPattern);
                        break;
                    case CellStyleUtil.CellType.Number4Decimals:
                        format = book.CreateDataFormat();
                        style.DataFormat = format.GetFormat(Constants.Excel4DecimalsFormatPattern);
                        break;
                    case CellStyleUtil.CellType.Number6Decimals:
                        format = book.CreateDataFormat();
                        style.DataFormat = format.GetFormat(Constants.Excel6DecimalsFormatPattern);
                        break;
                    default:
                        break;
                }

                switch (cellBold)
                {
                    case CellStyleUtil.Bold.NotBolded:
                        break;
                    case CellStyleUtil.Bold.Bolded:
                        font = book.CreateFont();
                        font.Boldweight = (short)FontBoldWeight.Bold;
                        style.SetFont(font);
                        break;
                    default:
                        break;
                }
                switch (cellBorder)
                {
                    case CellStyleUtil.Border.None:
                        break;
                    case CellStyleUtil.Border.Top:
                        style.BorderTop = borderStyle;
                        break;
                    case CellStyleUtil.Border.Bottom:
                        style.BorderBottom = borderStyle;
                        break;
                    case CellStyleUtil.Border.Left:
                        style.BorderLeft = borderStyle;
                        break;
                    case CellStyleUtil.Border.Right:
                        style.BorderRight = borderStyle;
                        break;
                    case CellStyleUtil.Border.TopBottom:
                        style.BorderTop = borderStyle;
                        style.BorderBottom = borderStyle;
                        break;
                    case CellStyleUtil.Border.LeftRight:
                        style.BorderLeft = borderStyle;
                        style.BorderRight = borderStyle;
                        break;
                    case CellStyleUtil.Border.TopLeft:
                        style.BorderTop = borderStyle;
                        style.BorderLeft = borderStyle;
                        break;
                    case CellStyleUtil.Border.TopRight:
                        style.BorderTop = borderStyle;
                        style.BorderRight = borderStyle;
                        break;
                    case CellStyleUtil.Border.BottomLeft:
                        style.BorderBottom = borderStyle;
                        style.BorderLeft = borderStyle;
                        break;
                    case CellStyleUtil.Border.BottomRight:
                        style.BorderBottom = borderStyle;
                        style.BorderRight = borderStyle;
                        break;
                    case CellStyleUtil.Border.TopLeftRight:
                        style.BorderTop = borderStyle;
                        style.BorderLeft = borderStyle;
                        style.BorderRight = borderStyle;
                        break;
                    case CellStyleUtil.Border.TopLeftBottom:
                        style.BorderTop = borderStyle;
                        style.BorderBottom = borderStyle;
                        style.BorderLeft = borderStyle;
                        break;
                    case CellStyleUtil.Border.TopRightBottom:
                        style.BorderTop = borderStyle;
                        style.BorderBottom = borderStyle;
                        style.BorderRight = borderStyle;
                        break;
                    case CellStyleUtil.Border.BottomLeftRight:
                        style.BorderBottom = borderStyle;
                        style.BorderLeft = borderStyle;
                        style.BorderRight = borderStyle;
                        break;
                    case CellStyleUtil.Border.All:
                        style.BorderTop = borderStyle;
                        style.BorderBottom = borderStyle;
                        style.BorderLeft = borderStyle;
                        style.BorderRight = borderStyle;
                        break;
                    default:
                        break;
                }
                Styles.Add(styleKey, style);
            }

            return Styles[styleKey];
        }
    }
}
