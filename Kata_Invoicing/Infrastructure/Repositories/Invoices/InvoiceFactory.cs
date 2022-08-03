using System.Data;
using Kata_Invoicing.Infrastructure.EntityFactoryFramework;
using Kata_Invoicing.Model.Invoices;

namespace Kata_Invoicing.Infrastructure.Repositories.Invoices
{
    class InvoiceFactory : IEntityFactory<Invoice>
    {
        #region IEntityFactory<Invoice> Members

        public Invoice BuildEntity(IDataReader reader)
        {
            var invoice = new Invoice();
            invoice.ID = DataHelper.GetInteger(reader[FieldNames.Id]);
            invoice.ContractID = DataHelper.GetInteger(reader[FieldNames.ContractID]);
            invoice.SettlementCycleID = DataHelper.GetInteger(reader[FieldNames.SettlementCycleID]);
            invoice.DateCreated = DataHelper.GetDateTime(reader[FieldNames.DateCreated]);
            invoice.InvoiceDate = DataHelper.GetDateTime(reader[FieldNames.InvoiceDate]);
            invoice.InvoiceStartDate = DataHelper.GetDateTime(reader[FieldNames.InvoiceStartDate]);
            invoice.InvoiceEndDate = DataHelper.GetDateTime(reader[FieldNames.InvoiceEndDate]);
            invoice.InvoiceTypeID = DataHelper.GetInteger(reader[FieldNames.InvoiceTypeID]);
            invoice.Number = DataHelper.GetString(reader[FieldNames.Number]);
            invoice.TotalPayOutAmount = DataHelper.GetDecimal(reader[FieldNames.TotalPayOutAmount]);
            invoice.SettlementCurrency = DataHelper.GetString(reader[FieldNames.SettlementCurrency]);
            invoice.InvoiceFilePathXLS = DataHelper.GetString(reader[FieldNames.InvoiceFilePathXLS]);
            return invoice;
        }
        #endregion
    }
}
