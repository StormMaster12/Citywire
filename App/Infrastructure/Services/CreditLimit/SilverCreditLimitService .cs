using App.Abstraction.Models;
using App.Abstraction.ThirdParty;

namespace App.Infrastructure.Services.CreditLimit;

public class SilverCreditLimitService : BaseCreditLimitService
{
    public SilverCreditLimitService(ICustomerCreditService customerCreditService) : base(customerCreditService)
    {
    }

    public override Customer SetCreditLimit(Customer customer)
    {
        var creditLimit =
            CustomerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);
        creditLimit *= 2;

        customer.CreditLimit = creditLimit;
        customer.HasCreditLimit = true;

        return customer;
    }
}