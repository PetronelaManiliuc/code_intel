using System.Collections.Generic;
using Kata_Invoicing.Infrastructure.RepositoryFramework;

namespace Kata_Invoicing.Model.Invoices
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        List<InvoicePayment> GetInvoicePayments(int invoiceID);
        List<InvoiceFee> GetInvoiceFees(int invoiceID);
        InvoiceDetails GetInvoiceDetails(int invoiceID);

    }
}

