using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kata_Invoicing.Infrastructure;
using Kata_Invoicing.Infrastructure.PersistanceManager;

namespace Kata_Invoicing.Model.Invoices
{
    public class InvoicePayment : IItem
    {
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public int ClientSiteID { get; set; }
        public string ClientSiteName { get; set; }
        public int MethodID { get; set; }
        public string MethodName { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string SettlementCurrency { get; set; }
        public decimal CalculatedAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public int PaymentTypeID { get; set; }



        public IItem CreateFromReader(System.Data.IDataReader dataReader)
        {
            var invoicePayment = new InvoicePayment();
            invoicePayment.ID = DataHelper.GetInteger(dataReader["ID"]);
            invoicePayment.InvoiceID = DataHelper.GetInteger(dataReader["InvoiceID"]);
            invoicePayment.ClientSiteID = DataHelper.GetInteger(dataReader["ClientSiteID"]);
            invoicePayment.ClientSiteName = DataHelper.GetString(dataReader["ClientSiteName"]);
            invoicePayment.MethodID = DataHelper.GetInteger(dataReader["MethodID"]);
            invoicePayment.MethodName = DataHelper.GetString(dataReader["MethodName"]);
            invoicePayment.Currency = DataHelper.GetString(dataReader["Currency"]);
            invoicePayment.Amount = DataHelper.GetDecimal(dataReader["Amount"]);
            invoicePayment.SettlementCurrency = DataHelper.GetString(dataReader["SettlementCurrency"]);
            invoicePayment.CalculatedAmount = DataHelper.GetDecimal(dataReader["CalculatedAmount"]);
            invoicePayment.ExchangeRate = DataHelper.GetDecimal(dataReader["ExchangeRate"]);
            invoicePayment.PaymentTypeID = DataHelper.GetInteger(dataReader["PaymentTypeID"]);

            return invoicePayment;
        }
    }
}
