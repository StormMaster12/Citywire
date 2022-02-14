using App.Abstraction.Models;

namespace App.Abstraction.Services.CreditLimit;

public interface ICreditLimitFactory
{
    ICreditLimitService GetCreditLimitService(Classification classification);
}