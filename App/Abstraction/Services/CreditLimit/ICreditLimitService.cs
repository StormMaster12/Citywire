using App.Abstraction.Models;

namespace App.Abstraction.Services.CreditLimit;

public interface ICreditLimitService
{
    Customer SetCreditLimit(Customer customer);
}