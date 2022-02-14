using System;
using System.Collections.Generic;
using System.Linq;
using App.Abstraction.Models;
using App.Abstraction.Services.CreditLimit;

namespace App.Infrastructure.Services.CreditLimit;

public class CreditLimitFactory : ICreditLimitFactory
{
    private readonly Dictionary<Classification, Func<ICreditLimitService>> _funcs;

    public CreditLimitFactory(IEnumerable<ICreditLimitService> creditLimitServices)
    {
        _funcs = new Dictionary<Classification, Func<ICreditLimitService>>
        {
            { Classification.Gold, () => creditLimitServices.First(x => x is GoldCreditLimitService) },
            { Classification.Silver, () => creditLimitServices.First(x => x is SilverCreditLimitService) },
            { Classification.Bronze, () => creditLimitServices.First(x => x is BronzeCreditLimitService) },
        };
    }

    public ICreditLimitService GetCreditLimitService(Classification classification)
    {
        return _funcs[classification]();
    }
}