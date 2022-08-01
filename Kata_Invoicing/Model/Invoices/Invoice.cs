using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kata_Invoicing.Infrastructure;
using Kata_Invoicing.Infrastructure.DomainBase;
using Kata_Invoicing.Infrastructure.PersistanceManager;

namespace Kata_Invoicing.Model.Invoices
{
    public class Invoice : IAggregateRoot, IItem
    {
        #region Properties

        public int ID { get; set; }


        public int ContractID { get; set; }


        public int SettlementCycleID { get; set; }


        public DateTime DateCreated { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime InvoiceStartDate { get; set; }

        public DateTime InvoiceEndDate { get; set; }

        public int InvoiceTypeID { get; set; }

        public string Number { get; set; }

        public decimal TotalPayOutAmount { get; set; }

        public string SettlementCurrency { get; set; }

        
        public string InvoiceFilePathXLS { get; set; }


        public string InvoiceDetailFormated
        {
            get { return Number + " - " + "Total: " + TotalPayOutAmount.ToString(); }
        }
        
        #endregion

        public int Key
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public IItem CreateFromReader(IDataReader dataReader)
        {
            Invoice invoice = new Invoice();

            invoice.ID = DataHelper.GetInteger(dataReader["ID"]);
            invoice.ContractID = DataHelper.GetInteger(dataReader["ContractID"]);
            invoice.SettlementCycleID = DataHelper.GetInteger(dataReader["SettlementCycleID"]);
            invoice.DateCreated = DataHelper.GetDateTime(dataReader["DateCreated"]);
            invoice.InvoiceDate = DataHelper.GetDateTime(dataReader["InvoiceDate"]);
            invoice.InvoiceStartDate = DataHelper.GetDateTime(dataReader["InvoiceStartDate"]);
            invoice.InvoiceEndDate = DataHelper.GetDateTime(dataReader["InvoiceEndDate"]);
            invoice.InvoiceTypeID = DataHelper.GetInteger(dataReader["InvoiceTypeID"]);
            invoice.Number = DataHelper.GetString(dataReader["Number"]);
            invoice.TotalPayOutAmount = DataHelper.GetDecimal(dataReader["TotalPayOutAmount"]);
            invoice.SettlementCurrency = DataHelper.GetString(dataReader["SettlementCurrency"]);
            invoice.InvoiceFilePathXLS = DataHelper.GetString(dataReader["InvoiceFilePathXLS"]);

            return invoice;
        }


    }
}
