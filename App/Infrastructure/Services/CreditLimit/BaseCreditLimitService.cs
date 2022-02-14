using App.Abstraction.Models;
using App.Abstraction.Services.CreditLimit;
using App.Abstraction.ThirdParty;

namespace App.Infrastructure.Services.CreditLimit;

public abstract class BaseCreditLimitService : ICreditLimitService
{
    protected readonly ICustomerCreditService CustomerCreditService;

    protected BaseCreditLimitService(ICustomerCreditService customerCreditService)
    {
        CustomerCreditService = customerCreditService;
    }

    public abstract Customer SetCreditLimit(Customer customer);
}