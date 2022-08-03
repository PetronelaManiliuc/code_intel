# coding-kata-invoicing
-----------------
**Terminology**
- Merchant - someone who owns a website	
- SettlementReport - financial document which represents the total amount that a merchant should receive 
- Invoice - financial document which represents the total fees/taxes we are charging the merchant
- Payment - the transfer of money or goods and services in exchange for a product or service
- Refunds - a sum of money which is returned to the Customer
- Fee - a tax that Nuvei is charging for using our services

-----------------
**Instructions**

1. Install SQL Express: https://www.microsoft.com/en-us/download/details.aspx?id=55994 
and restore the backupDB from the GitHub repository  [master\DB\Invoicing_Kata.bak]
2. Change the export path in Settings.xml to your local folder 
where you want to export the files

-----------------
**Assessment**

Please refactor the code to be maintainable, scalable and testable, so that:  
1. The code should be easy to explain -  we can easily integrate a new developer into the team 
2.  We can easily extend the application if we need:
- a new type of Invoice (SettlementInvoice),
- a new type of transaction (Refund), 
- a new type of fee (refund fee), 
- a new type of format(e.g PDF) - now is only excel (XLS)
3. Unit tests are a plus

Keywords: SOLID and Design patterns

**We are NOT interested in:**
1. redesigning existing tables or procedures from Database
2. changing the existing code that creates excel sheets (GenerateInvoiceSheet method from InvoiceGenerator.cs)	
    
