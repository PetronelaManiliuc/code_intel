using Kata_Invoicing.Infrastructure;
using Kata_Invoicing.Infrastructure.PersistanceManager;

namespace Kata_Invoicing.Model.Invoices
{
    public class InvoiceFee : IItem
    {
        public int ID { get; set; }
        public int InvoiceID { get; set; }
        public int ClientSiteID { get; set; }
        public string ClientSiteName { get; set; }
        public int MethodID { get; set; }
        public string MethodName { get; set; }
        public int FeeID { get; set; }
        public string FeeDescription { get; set; }
        public string Formula { get; set; }
        public int Count { get; set; }
        public decimal CalculatedAmount { get; set; }
        public string SettlementCurrency { get; set; }
        public string Country { get; set; }
        public int TrxMonth { get; set; }
        public int TrxYear { get; set; }
  

        public IItem CreateFromReader(System.Data.IDataReader dataReader)
        {
            IItem invoiceFeeView = new InvoiceFee();
            ((InvoiceFee)invoiceFeeView).ID = DataHelper.GetInteger(dataReader[FieldNames.Id]);
            ((InvoiceFee)invoiceFeeView).InvoiceID = DataHelper.GetInteger(dataReader[FieldNames.InvoiceID]);
            ((InvoiceFee)invoiceFeeView).ClientSiteID = DataHelper.GetInteger(dataReader[FieldNames.ClientSiteID]);
            ((InvoiceFee)invoiceFeeView).ClientSiteName = DataHelper.GetString(dataReader[FieldNames.ClientSiteName]);
            ((InvoiceFee)invoiceFeeView).MethodID = DataHelper.GetInteger(dataReader[FieldNames.MethodID]);
            ((InvoiceFee)invoiceFeeView).MethodName = DataHelper.GetString(dataReader[FieldNames.MethodName]);
            ((InvoiceFee)invoiceFeeView).FeeID = DataHelper.GetInteger(dataReader[FieldNames.FeeID]);
            ((InvoiceFee)invoiceFeeView).FeeDescription = DataHelper.GetString(dataReader[FieldNames.FeeDescription]);
            ((InvoiceFee)invoiceFeeView).Formula = DataHelper.GetString(dataReader[FieldNames.Formula]);
            ((InvoiceFee)invoiceFeeView).Count = DataHelper.GetInteger(dataReader[FieldNames.Count]);
            ((InvoiceFee)invoiceFeeView).CalculatedAmount = DataHelper.GetDecimal(dataReader[FieldNames.CalculatedAmount]);
            ((InvoiceFee)invoiceFeeView).SettlementCurrency = DataHelper.GetString(dataReader[FieldNames.SettlementCurrency]);
            ((InvoiceFee)invoiceFeeView).Country = DataHelper.GetString(dataReader[FieldNames.Country]);
            ((InvoiceFee)invoiceFeeView).TrxMonth = DataHelper.GetInteger(dataReader[FieldNames.TrxMonth]);
            ((InvoiceFee)invoiceFeeView).TrxYear = DataHelper.GetInteger(dataReader[FieldNames.TrxYear]);
            return invoiceFeeView;
        }
    }
}
