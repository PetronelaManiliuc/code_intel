using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Kata_Invoicing.Infrastructure.Common.Utils;
using Kata_Invoicing.Model.Invoices;
using Kata_Invoicing.Model.Settings;
using log4net;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Kata_Invoicing.InvoiceManager.InvoiceExport
{
    public class InvoiceGenerator
    {
        #region Private properties

        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoiceGenerator));
        private HSSFWorkbook workbook;

        private readonly int invoiceId;
        //private Invoice currentInvoice;

        public string ErrorMessage;

        //InvoiceData
        public InvoiceDetails InvoiceDetails;
        public List<InvoicePayment> InvoicePayments;
        public List<InvoiceFee> InvoiceFees;


        //Invoice
        public string InvoiceNumber;
        public string InvoiceDate;
        public string InvoiceStartDate;
        public string InvoiceEndDate;
        public string InvoiceSettlementCurrency;
        public string InvoiceTypeName;

        //Payments
        public decimal PaymentsAmount;
        public decimal TotalPaymentsAmount;

        //TransactionFees
        public decimal TransactionFeesAmount;
        public decimal TotalTransactionFeesAmount;

        //Total
        public string TotalDescription;
        public decimal TotalPayOutAmount;
        #endregion

        #region Constructor

        public InvoiceGenerator(int invoiceId)
        {
            this.invoiceId = invoiceId;

            try
            {
                GetInvoiceDataFromDb();
                ComputeInvoiceFirstPage();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ErrorMessage = ex.Message;
            }
        }

        #endregion

        #region Export Invoice Methods

        public ExportResult ExportInvoiceXls()
        {
            var exportResult = new ExportResult();
            try
            {
                var settings = SettingsService.GetSettings();

                if (string.IsNullOrEmpty(settings.ExportInvoicesPath))
                {
                    exportResult.ErrorMessage = "Invoice Export Directory is not configured!";
                    return exportResult;
                }

                InitializeWorkBook();
                
                GenerateInvoiceSheet();
                
                //make final adjustments to the workbook according to the InvoiceType
                workbook.SetSheetName(0, InvoiceTypeName);
   
                //save invoice to the specific path
                exportResult.FilePath = SaveWorkBook(settings);
                if (!string.IsNullOrEmpty(exportResult.FilePath))
                {
                    //update invoice Status (only if not Final)
                    var invoice = InvoiceService.GetInvoiceByID(invoiceId);

                    invoice.InvoiceFilePathXLS = exportResult.FilePath;
                    var ok = InvoiceService.UpdateInvoice(invoice);
                    if (ok)
                    {
                        exportResult.IsOk = true;
                        InvoiceDetails.InvoiceFilePathXLS = exportResult.FilePath;
                    }
                    else
                    {
                        exportResult.ErrorMessage = "Problem on updating invoice in database!";
                    }
                }
            }
            catch (IOException e)
            {
                Log.Error($"InvoiceID : {invoiceId} - {e}");
                exportResult.ErrorMessage =
                    "Invoice not Exported! Please make sure that another user is not processing this invoice, and then try again. \n" +
                    e.Message;
            }
            catch (Exception e)
            {
                Log.Error($"InvoiceID : {invoiceId} - {e}");
                exportResult.ErrorMessage = e.Message;
            }


            return exportResult;
        }

        #region Private Methods

        //get data from db
        private void GetInvoiceDataFromDb()
        {
            try
            {
                //get invoice data from database
                InvoiceDetails = InvoiceService.GetInvoiceDetails(invoiceId);
                InvoicePayments = InvoiceService.GetInvoicePayments(invoiceId);
                InvoiceFees = InvoiceService.GetInvoiceFees(invoiceId);
            }
            catch (SqlException e)
            {
                Log.Error(e.Message);
                throw;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        private void ComputeInvoiceFirstPage()
        {
            try
            {
                #region InvoiceData

                //set Invoice Data that is computed on the generation time
                InvoiceNumber = !string.IsNullOrEmpty(InvoiceDetails.InvoiceNumber)
                    ? InvoiceDetails.InvoiceNumber
                    : "000000000000";
                InvoiceDate = InvoiceDetails.InvoiceDate.ToString(Constants.LONG_DATE_FORMAT);
                InvoiceStartDate = InvoiceDetails.InvoiceStartDate.ToString(Constants.LONG_DATE_FORMAT);
                InvoiceEndDate = InvoiceDetails.InvoiceEndDate.ToString(Constants.LONG_DATE_FORMAT);
                InvoiceSettlementCurrency = InvoiceDetails.InvoiceSettlementCurrency;

                InvoiceTypeName = InvoiceDetails.InvoiceTypeName;

                #endregion

                #region Payments

                //set the payments
                PaymentsAmount = InvoicePayments.Where(x => x.PaymentTypeID == (int)PaymentType.Payment)
                    .Sum(x => x.CalculatedAmount);

                TotalPaymentsAmount = PaymentsAmount;

                #endregion

                #region TransactionFees

                TransactionFeesAmount = InvoiceFees.Where(x => x.FeeID == (int)FeeType.TransactionFee).Sum(f => f.CalculatedAmount);

                TotalTransactionFeesAmount = TransactionFeesAmount;

                #endregion

                #region Total

                TotalDescription = InvoiceDetails.InvoiceTypeID == (int)InvoiceType.Invoice ? "TOTAL FEES" : "TOTAL PAY OUT";

                TotalPayOutAmount = InvoiceDetails.InvoiceTypeID == (int)InvoiceType.Invoice ? TotalTransactionFeesAmount : PaymentsAmount;

                #endregion
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        //export invoice helpers
        private void InitializeWorkBook()
        {
            //from template
            var file = new FileStream(@"Templates/Invoice.xls", FileMode.Open, FileAccess.Read);
            workbook = new HSSFWorkbook(file);

            var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Smart2Pay";
            workbook.DocumentSummaryInformation = dsi;

            var si = PropertySetFactory.CreateSummaryInformation();
            si.Title = "InvoiceItem";
            si.CreateDateTime = DateTime.Now;
            workbook.SummaryInformation = si;
        }

        private string SaveWorkBook(Settings settings)
        {
            var exportPath = Path.Combine(settings.ExportInvoicesPath);
            if (!Directory.Exists(exportPath)) Directory.CreateDirectory(exportPath);

            var invoiceName = InvoiceDetails.InvoiceNumber + Constants.XlsExtension;
            var invoiceFullName = Path.Combine(exportPath, invoiceName);

            var file = new FileStream(invoiceFullName, FileMode.Create);
            workbook.Write(file);
            file.Close();

            var filePath = invoiceFullName;
            return filePath;
        }

        private void GenerateInvoiceSheet()
        {
            var invoiceSheet = workbook.GetSheet("Invoice");

            var clientName = !string.IsNullOrEmpty(InvoiceDetails.ClientName)
                ? InvoiceDetails.ClientName
                : "";
            
            #region InvoiceHeader

            var indexStart = new IndexCell
            {
                Col = 1,
                Row = 5
            };
            var indexCellTemp = new IndexCell();

            IRow row;

            //Invoice Type Name
            if (!string.IsNullOrEmpty(InvoiceTypeName))
            {
                row = CellUtil.GetRow(4, invoiceSheet);
                CellUtil.GetCell(row, 0).SetCellValue(InvoiceTypeName);
            }

            //Client Dates
            row = CellUtil.GetRow(indexStart.Row++, invoiceSheet);
            var cell = CellUtil.GetCell(row, indexStart.Col);

            cell.SetCellValue(clientName);
            cell.CellStyle.WrapText = true;

            indexCellTemp.Row = indexStart.Row + 7;

            indexStart.Row = 5;
            indexStart.Col = 4;


            row = CellUtil.GetRow(indexStart.Row++, invoiceSheet);
            cell = CellUtil.GetCell(row, indexStart.Col - 1);
            cell.SetCellValue(InvoiceTypeName.ToLower() + " number");
            CellUtil.GetCell(row, indexStart.Col).SetCellValue(InvoiceNumber);

            row = CellUtil.GetRow(indexStart.Row++, invoiceSheet);
            cell = CellUtil.GetCell(row, indexStart.Col);
            cell.SetCellValue(InvoiceDate);

            row = CellUtil.GetRow(indexStart.Row++, invoiceSheet);
            cell = CellUtil.GetCell(row, indexStart.Col);
            cell.SetCellValue(InvoiceStartDate);
            cell = CellUtil.GetCell(row, indexStart.Col + 1);
            cell.SetCellValue(InvoiceEndDate);

            indexStart.Row = indexCellTemp.Row;

            #endregion

            if (InvoiceDetails.InvoiceTypeID != (int)InvoiceType.Invoice)
            {
                #region Payments

                indexStart.Col = 0;
                indexCellTemp.Col = indexStart.Col;
                indexStart.Row = indexCellTemp.Row;

                //Payments
                row = CellUtil.GetRow(indexCellTemp.Row, invoiceSheet);
                cell = CellUtil.GetCell(row, indexCellTemp.Col++);
                cell.SetCellValue("Payments");
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.Bolded, CellStyleUtil.Border.TopLeft, BorderStyle.Thin, null);

                row = CellUtil.GetRow(indexCellTemp.Row++, invoiceSheet);
                cell = CellUtil.GetCell(row, indexCellTemp.Col++);
                cell.SetCellValue($"{InvoiceStartDate} - {InvoiceEndDate}");
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.TopBottom, BorderStyle.Medium, null);

                cell = CellUtil.GetCell(row, indexCellTemp.Col++);
                cell.SetCellValue(string.Empty);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.TopBottom, BorderStyle.Medium, null);

                cell = CellUtil.GetCell(row, indexCellTemp.Col++);
                cell.SetCellValue(string.Empty);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.TopBottom, BorderStyle.Medium, null);

                cell = CellUtil.GetCell(row, indexCellTemp.Col);
                cell.SetCellValue((double)PaymentsAmount);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Number,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.All, BorderStyle.Medium,
                    InvoiceSettlementCurrency);

                indexCellTemp.Col = (short)(indexStart.Col + 1);

             

                //Total Payments
                row = CellUtil.GetRow(indexCellTemp.Row, invoiceSheet);
                cell = CellUtil.GetCell(row, indexStart.Col + 5);
                cell.SetCellValue((double)TotalPaymentsAmount);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Number,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.All, BorderStyle.Medium,
                    InvoiceSettlementCurrency);

                //set borders
                for (var i = indexStart.Row + 1; i <= indexCellTemp.Row; i++)
                {
                    row = CellUtil.GetRow(i, invoiceSheet);
                    cell = CellUtil.GetCell(row, indexStart.Col);
                    cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                        CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.Left, BorderStyle.Thin, null);
                }

                //set borders on total row
                row = CellUtil.GetRow(indexCellTemp.Row + 1, invoiceSheet);
                for (var i = 0; i < indexStart.Col + 5; i++)
                {
                    cell = CellUtil.GetCell(row, indexStart.Col + i);
                    cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                        CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.Top, BorderStyle.Thin, null);
                }

                indexStart.Row = indexCellTemp.Row + 5;

                #endregion
            }

            if (InvoiceDetails.InvoiceTypeID != (int)InvoiceType.Settlement)
            {
                #region Transaction Fees

                indexStart.Col = 0;
                indexCellTemp.Col = indexStart.Col;
                indexCellTemp.Row = indexStart.Row;


                //Transaction Fees
                row = CellUtil.GetRow(indexCellTemp.Row, invoiceSheet);
                cell = CellUtil.GetCell(row, indexCellTemp.Col++);
                cell.SetCellValue("Transaction Fees");
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.Bolded, CellStyleUtil.Border.TopLeft, BorderStyle.Thin, null);

                row = CellUtil.GetRow(indexCellTemp.Row++, invoiceSheet);
                cell = CellUtil.GetCell(row, indexCellTemp.Col++);
                cell.SetCellValue($"{InvoiceStartDate} - {InvoiceEndDate}");
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.TopBottom, BorderStyle.Medium, null);

                cell = CellUtil.GetCell(row, indexCellTemp.Col++);
                cell.SetCellValue(string.Empty);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Number,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.TopBottom, BorderStyle.Medium, null);

                cell = CellUtil.GetCell(row, indexCellTemp.Col++);
                cell.SetCellValue(string.Empty);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.TopBottom, BorderStyle.Medium, null);


                cell = CellUtil.GetCell(row, indexCellTemp.Col);
                cell.SetCellValue((double)TransactionFeesAmount);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Number,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.All, BorderStyle.Medium,
                    InvoiceSettlementCurrency);

                indexCellTemp.Col = (short)(indexStart.Col + 1);
                
                //Total Transaction Fees
                row = CellUtil.GetRow(indexCellTemp.Row, invoiceSheet);
                cell = CellUtil.GetCell(row, indexStart.Col + 5);
                cell.SetCellValue((double)TotalTransactionFeesAmount);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Number,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.All, BorderStyle.Medium,
                    InvoiceSettlementCurrency);

                //set borders
                for (var i = indexStart.Row + 1; i <= indexCellTemp.Row; i++)
                {
                    row = CellUtil.GetRow(i, invoiceSheet);
                    cell = CellUtil.GetCell(row, indexStart.Col);
                    cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                        CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.Left, BorderStyle.Thin, null);
                }

                //set borders on total row
                row = CellUtil.GetRow(indexCellTemp.Row + 1, invoiceSheet);
                for (var i = 0; i < indexStart.Col + 5; i++)
                {
                    cell = CellUtil.GetCell(row, indexStart.Col + i);
                    cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                        CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.Top, BorderStyle.Thin, null);
                }

                indexStart.Row = indexCellTemp.Row + 5;
                indexStart.Col = 0;

                #endregion
            }

            #region TOTAL

            //TOTAL PAY OUT
            row = CellUtil.GetRow(indexStart.Row + 2, invoiceSheet);
            for (var i = 1; i < 5; i++)
            {
                cell = CellUtil.GetCell(row, indexStart.Col + i);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.Bottom, BorderStyle.Medium, null);
            }

            row = CellUtil.GetRow(indexStart.Row + 3, invoiceSheet);
            for (var i = 1; i < 5; i++)
            {
                cell = CellUtil.GetCell(row, indexStart.Col + i);
                cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                    CellStyleUtil.Bold.NotBolded, CellStyleUtil.Border.Bottom, BorderStyle.Thin, null);
            }

            cell = CellUtil.GetCell(row, indexStart.Col + 1);
            cell.SetCellValue(TotalDescription);
            cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Text,
                CellStyleUtil.Bold.Bolded, CellStyleUtil.Border.Bottom, BorderStyle.Thin, null);

            row = CellUtil.GetRow(indexStart.Row + 3, invoiceSheet);
            cell = CellUtil.GetCell(row, indexStart.Col + 5);
            cell.SetCellValue((double)TotalPayOutAmount); //totalPayoutAmount
            cell.CellStyle = CellStyleUtil.GetCellStyleKey(workbook, CellStyleUtil.CellType.Number,
                CellStyleUtil.Bold.Bolded, CellStyleUtil.Border.All, BorderStyle.Medium, InvoiceSettlementCurrency);

            #endregion

        
        }

        #endregion
    }

    #endregion
}