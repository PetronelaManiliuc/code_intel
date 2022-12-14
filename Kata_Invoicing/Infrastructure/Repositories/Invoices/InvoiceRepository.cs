using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Kata_Invoicing.Model.Invoices;

namespace Kata_Invoicing.Infrastructure.Repositories.Invoices
{
    class InvoiceRepository : SqlRepositoryBase<Invoice>, IInvoiceRepository
    {
        protected override string GetEntityName()
        {
            return "Invoice";
        }

        protected override string GetKeyFieldName()
        {
            return FieldNames.Id;
        }

        #region Public Constructors

        public InvoiceRepository()
            : this(null)
        {
        }

        public InvoiceRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        #endregion

        #region UnitOfWorkImplementation
        protected override void BuildChildCallbacks()
        {

        }

        protected override SqlCommand AllEntitiesSqlCommand(string commandText)
        {
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = commandText;

            return command;
        }

        protected override SqlCommand EntityByKeySqlCommand(int key)
        {
            SqlCommand command = AllEntitiesSqlCommand("InvoiceGetByID");
            command.Parameters.Add("@ID", SqlDbType.Int);
            command.Parameters["@ID"].Value = (key);
            return command;
        }

        protected SqlCommand UpdateEntitySqlCommand(Invoice invoice)
        {
            SqlCommand command = AllEntitiesSqlCommand("InvoiceUpdate");

            command.Parameters.Add("@ID", SqlDbType.Int);
            command.Parameters["@ID"].Value = invoice.ID;


            command.Parameters.Add("@InvoiceFilePathXLS", SqlDbType.VarChar);
            command.Parameters["@InvoiceFilePathXLS"].Value = invoice.InvoiceFilePathXLS;

            return command;
        }

        protected override bool PersistNewItem(Invoice item)
        {
            return false;
        }

        protected override bool PersistUpdatedItem(Invoice item)
        {
            return PersistanceManager.PersistanceManager.ExecuteStoredProcedureNonQuery(UpdateEntitySqlCommand(item));
        }

        protected override bool PersistDeletedItem(Invoice item)
        {
            return false;
        }
        #endregion

        #region Public Methods

        public List<InvoicePayment> GetInvoicePayments(int invoiceID)
        {
            SqlCommand command = AllEntitiesSqlCommand("InvoicePaymentsGetByInvoiceID");

            command.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.Int));
            command.Parameters["@InvoiceID"].Value = invoiceID;

            return PersistanceManager.PersistanceManager.ExecuteStoredProcedureQuery<InvoicePayment>(command).ToList();
        }


        public List<InvoiceFee> GetInvoiceFees(int invoiceID)
        {
            SqlCommand command = AllEntitiesSqlCommand("InvoiceFeesGetByInvoiceID");

            command.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.Int));
            command.Parameters["@InvoiceID"].Value = invoiceID;

            return PersistanceManager.PersistanceManager.ExecuteStoredProcedureQuery<InvoiceFee>(command).ToList();
        }


        public InvoiceDetails GetInvoiceDetails(int invoiceID)
        {
            SqlCommand command = AllEntitiesSqlCommand("InvoiceDetailsGetByInvoiceID");

            command.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.Int));
            command.Parameters["@InvoiceID"].Value = invoiceID;

            return PersistanceManager.PersistanceManager.ExecuteStoredProcedureQuery<InvoiceDetails>(command).ToList().FirstOrDefault() ?? new InvoiceDetails();
        }

        #endregion
    }
}
