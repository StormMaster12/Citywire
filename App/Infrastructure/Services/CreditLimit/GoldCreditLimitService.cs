using App.Abstraction.Models;
using App.Abstraction.ThirdParty;

namespace App.Infrastructure.Services.CreditLimit;

public class GoldCreditLimitService : BaseCreditLimitService
{
    public GoldCreditLimitService(ICustomerCreditService customerCreditService) : base(customerCreditService)
    {
    }

    public override Customer SetCreditLimit(Customer customer)
    {
        customer.HasCreditLimit = false;
        return customer;
    }
}