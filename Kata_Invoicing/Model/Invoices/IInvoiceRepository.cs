using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

