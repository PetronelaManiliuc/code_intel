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
            invoicePayment.ID = DataHelper.GetInteger(dataReader[FieldNames.Id]);
            invoicePayment.InvoiceID = DataHelper.GetInteger(dataReader[FieldNames.InvoiceID]);
            invoicePayment.ClientSiteID = DataHelper.GetInteger(dataReader[FieldNames.ClientSiteID]);
            invoicePayment.ClientSiteName = DataHelper.GetString(dataReader[FieldNames.ClientSiteName]);
            invoicePayment.MethodID = DataHelper.GetInteger(dataReader[FieldNames.MethodID]);
            invoicePayment.MethodName = DataHelper.GetString(dataReader[FieldNames.MethodName]);
            invoicePayment.Currency = DataHelper.GetString(dataReader[FieldNames.Currency]);
            invoicePayment.Amount = DataHelper.GetDecimal(dataReader[FieldNames.Amount]);
            invoicePayment.SettlementCurrency = DataHelper.GetString(dataReader[FieldNames.SettlementCurrency]);
            invoicePayment.CalculatedAmount = DataHelper.GetDecimal(dataReader[FieldNames.CalculatedAmount]);
            invoicePayment.ExchangeRate = DataHelper.GetDecimal(dataReader[FieldNames.ExchangeRate]);
            invoicePayment.PaymentTypeID = DataHelper.GetInteger(dataReader[FieldNames.PaymentTypeID]);

            return invoicePayment;
        }
    }
}
