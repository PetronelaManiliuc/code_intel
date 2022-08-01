using System;
using System.ComponentModel;
using Kata_Invoicing.Infrastructure;
using Kata_Invoicing.Infrastructure.PersistanceManager;

namespace Kata_Invoicing.Model.Invoices
{
    public class InvoiceDetails : IItem, INotifyPropertyChanged
    {
        #region Properties
        public string SupplierLogoPath { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierName { get; set; }
        public string SupplierDisplayName { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceStartDate { get; set; }
        public DateTime InvoiceEndDate { get; set; }
        public int InvoiceTypeID { get; set; }
        public string InvoiceTypeName { get; set; }
        public decimal TotalPayOutAmount { get; set; }
        public string InvoiceSettlementCurrency { get; set; }
        public string InvoiceFilePathXLS { get; set; }
        public int InvoiceID { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public IItem CreateFromReader(System.Data.IDataReader dataReader)
        {
            var invoiceDetails = new InvoiceDetails();

            invoiceDetails.SupplierLogoPath = DataHelper.GetString(dataReader["SupplierLogoPath"]);
            invoiceDetails.SupplierAddress = DataHelper.GetString(dataReader["SupplierAddress"]);
            invoiceDetails.SupplierName = DataHelper.GetString(dataReader["SupplierName"]);
            invoiceDetails.SupplierDisplayName = DataHelper.GetString(dataReader["SupplierDisplayName"]);
            invoiceDetails.ClientCode = DataHelper.GetString(dataReader["ClientCode"]);
            invoiceDetails.ClientName = DataHelper.GetString(dataReader["ClientName"]);
            invoiceDetails.ClientAddress = DataHelper.GetString(dataReader["ClientAddress"]);
            invoiceDetails.InvoiceNumber = DataHelper.GetString(dataReader["InvoiceNumber"]);
            invoiceDetails.InvoiceDate = DataHelper.GetDateTime(dataReader["InvoiceDate"]);
            invoiceDetails.InvoiceStartDate = DataHelper.GetDateTime(dataReader["InvoiceStartDate"]);
            invoiceDetails.InvoiceEndDate = DataHelper.GetDateTime(dataReader["InvoiceEndDate"]);
            invoiceDetails.InvoiceTypeID = DataHelper.GetInteger(dataReader["InvoiceTypeID"]);
            invoiceDetails.InvoiceTypeName = DataHelper.GetString(dataReader["InvoiceTypeName"]);
            invoiceDetails.TotalPayOutAmount = DataHelper.GetDecimal(dataReader["TotalPayOutAmount"]);
            invoiceDetails.InvoiceSettlementCurrency = DataHelper.GetString(dataReader["InvoiceSettlementCurrency"]);
            invoiceDetails.InvoiceFilePathXLS = DataHelper.GetString(dataReader["InvoiceFilePathXLS"]);
            invoiceDetails.InvoiceID = DataHelper.GetInteger(dataReader["InvoiceID"]);
            return invoiceDetails;
        }
    }

}
