using System;
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
            ((InvoiceFee)invoiceFeeView).ID = DataHelper.GetInteger(dataReader["ID"]);
            ((InvoiceFee)invoiceFeeView).InvoiceID = DataHelper.GetInteger(dataReader["InvoiceID"]);
            ((InvoiceFee)invoiceFeeView).ClientSiteID = DataHelper.GetInteger(dataReader["ClientSiteID"]);
            ((InvoiceFee)invoiceFeeView).ClientSiteName = DataHelper.GetString(dataReader["ClientSiteName"]);
            ((InvoiceFee)invoiceFeeView).MethodID = DataHelper.GetInteger(dataReader["MethodID"]);
            ((InvoiceFee)invoiceFeeView).MethodName = DataHelper.GetString(dataReader["MethodName"]);
            ((InvoiceFee)invoiceFeeView).FeeID = DataHelper.GetInteger(dataReader["FeeID"]);
            ((InvoiceFee)invoiceFeeView).FeeDescription = DataHelper.GetString(dataReader["FeeDescription"]);
            ((InvoiceFee)invoiceFeeView).Formula = DataHelper.GetString(dataReader["Formula"]);
            ((InvoiceFee)invoiceFeeView).Count = DataHelper.GetInteger(dataReader["Count"]);
            ((InvoiceFee)invoiceFeeView).CalculatedAmount = DataHelper.GetDecimal(dataReader["CalculatedAmount"]);
            ((InvoiceFee)invoiceFeeView).SettlementCurrency = DataHelper.GetString(dataReader["SettlementCurrency"]);
            ((InvoiceFee)invoiceFeeView).Country = DataHelper.GetString(dataReader["Country"]);
            ((InvoiceFee)invoiceFeeView).TrxMonth = DataHelper.GetInteger(dataReader["TrxMonth"]);
            ((InvoiceFee)invoiceFeeView).TrxYear = DataHelper.GetInteger(dataReader["TrxYear"]);
            return invoiceFeeView;
        }
    }
}
