using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Kata_Invoicing
{
    public class ExcelHelper
    {
        public static int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.PNG);
        }
    }
}
