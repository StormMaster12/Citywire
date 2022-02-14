using App.Abstraction.Models;
using App.Abstraction.Services.CreditLimit;
using App.Abstraction.ThirdParty;
using App.Infrastructure.Services.CreditLimit;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace App.Test.Infrastructure.CreditLimit;

public class CreditLimitFactoryTests
{
    [Theory]
    [InlineData(Classification.Bronze, typeof(BronzeCreditLimitService))]
    [InlineData(Classification.Silver, typeof(SilverCreditLimitService))]
    [InlineData(Classification.Gold, typeof(GoldCreditLimitService))]
    public void GetCreditLimitService_Classification_Should_Return_Correct_Type(Classification classification, Type type)
    {
        Mock<ICustomerCreditService> mockCreditService = new Mock<ICustomerCreditService>();

        var creditLimitFactory = new CreditLimitFactory(new List<ICreditLimitService>()
        {
            new BronzeCreditLimitService(mockCreditService.Object),
            new GoldCreditLimitService(mockCreditService.Object),
            new SilverCreditLimitService(mockCreditService.Object)
        });

        var service = creditLimitFactory.GetCreditLimitService(classification);
        service.Should().BeOfType(type);
    }
}