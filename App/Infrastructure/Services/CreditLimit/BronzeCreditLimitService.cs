using App.Abstraction.Models;
using App.Abstraction.ThirdParty;

namespace App.Infrastructure.Services.CreditLimit;

public class BronzeCreditLimitService  : BaseCreditLimitService
{
    public BronzeCreditLimitService(ICustomerCreditService customerCreditService) : base(customerCreditService)
    {
    }

    public override Customer SetCreditLimit(Customer customer)
    {
        var creditLimit = CustomerCreditService.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth);

        customer.CreditLimit = creditLimit;
        customer.HasCreditLimit = true;

        return customer;
    }
}