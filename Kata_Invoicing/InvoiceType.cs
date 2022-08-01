namespace Kata_Invoicing
{
    public enum InvoiceType
    {
        Settlement = 2,
        Invoice = 3,
        SettlementInvoice = 1
    }

    public enum PaymentType
    {
        Payment = 1,
        Refund = 2
    }
    public enum FeeType
    {
        TransactionFee = 1,
        RefundFee = 2
    }
}
