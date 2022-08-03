using System.Collections.Generic;
using Kata_Invoicing.Infrastructure;
using Kata_Invoicing.Infrastructure.RepositoryFramework;

namespace Kata_Invoicing.Model.Invoices
{
    public static class InvoiceService
    {
        #region Private Fields
        private static IInvoiceRepository repository;
        private static IUnitOfWork unitOfWork;
        #endregion

        static InvoiceService()
        {
            unitOfWork = new UnitOfWork();
            repository = RepositoryFactory.GetRepository<IInvoiceRepository, Invoice>(InvoiceService.unitOfWork);
        }
        
        //Invoice
        public static Invoice GetInvoiceByID(int invoiceID)
        {
            return repository.FindBy(invoiceID);
        }

        public static bool UpdateInvoice(Invoice item)
        {
            repository[item.Key] = item;
            return unitOfWork.Commit();
        }

        public static List<InvoicePayment> GetInvoicePayments(int invoiceID)
        {
            return repository.GetInvoicePayments(invoiceID);
        }
        
        public static List<InvoiceFee> GetInvoiceFees(int invoiceID)
        {
            return repository.GetInvoiceFees(invoiceID);
        }
 
        public static InvoiceDetails GetInvoiceDetails(int invoiceID)
        {
            return repository.GetInvoiceDetails(invoiceID);
        }

       
    }
}
