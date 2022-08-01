using Kata_Invoicing.InvoiceManager.InvoiceExport;
using System;

namespace Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            //Invoices: SettlementReport: InvoiceID: 3
            //Invoice: InvoiceID: 1

            InvoiceGenerator settlementReportGenerator = new InvoiceGenerator(1);
            var exportResult1 = settlementReportGenerator.ExportInvoiceXls();
            Console.WriteLine($"File {exportResult1.FilePath} exported");

            InvoiceGenerator invoiceGenerator = new InvoiceGenerator(3);
            var exportResult2 = invoiceGenerator.ExportInvoiceXls();
            Console.WriteLine($"File {exportResult2.FilePath} exported");

            Console.WriteLine("finished...");
            Console.ReadKey();
        }
    }
}
