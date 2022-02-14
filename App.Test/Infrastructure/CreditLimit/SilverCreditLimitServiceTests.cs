using System;
using App.Abstraction.Models;
using App.Abstraction.ThirdParty;
using App.Infrastructure.Services.CreditLimit;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Test.Infrastructure.CreditLimit;

public class SilverCreditLimitServiceTests
{
    [Fact]
    public void SetCreditLimit_Should_Double_Credit_Limit_And_Has_Limit()
    {
        var expectedLimit = 40;
        var creditLimit = 20;
        var customer = new Customer("f", "s", DateTime.Now, "e", new Company());

        var mockCreditService = new Mock<ICustomerCreditService>();
        mockCreditService.Setup(x => x.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
            .Returns(creditLimit);

        var service = new SilverCreditLimitService(mockCreditService.Object);

        var result = service.SetCreditLimit(customer);

        result.CreditLimit.Should().Be(expectedLimit);
        result.HasCreditLimit.Should().BeTrue();
    }
}