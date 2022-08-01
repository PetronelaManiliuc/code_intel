using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Invoicing
{
    public static class Constants
    {
        //Settings
        //public const string SettingsDirectory = "Settings";  //Set this empty on next line for Kata purpose
        public const string SettingsDirectory = "";
        public const string SettingsFileName = "Settings.xml";

        public const string SingleDate = "dd MMM yy";
        public const string IntervalDate = "dd MMM";

        public const string XlsExtension = ".xls";
        
        public const string ExcelAmountsFormatPattern = "[${0}] #,##0.00";
        public const string ExcelAmountsNoCurrencyFormatPattern = "#,##0.00";
        public const string Excel4DecimalsFormatPattern = "0.0000";
        public const string Excel6DecimalsFormatPattern = "0.000000";
        public const string LONG_DATE_FORMAT = "dd/MM/yyyy";
        
    }
}
