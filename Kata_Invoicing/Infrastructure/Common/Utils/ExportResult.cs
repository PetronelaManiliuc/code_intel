using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Invoicing
{
    public class ExportResult
    {
        public int ID { get; set; }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public string FilePath { get; set; }
        public decimal Amount { get; set; }
    }
}
