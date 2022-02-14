using System;
using App.Abstraction.Models;
using App.Abstraction.ThirdParty;
using App.Infrastructure.Services.CreditLimit;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Test.Infrastructure.CreditLimit;

public class GoldCreditLimitServiceTests
{
    [Fact]
    public void SetCreditLimit_Should_Not_Set_Credit_Limit_And_Has_Limit()
    {
        var creditLimit = 0;
        var customer = new Customer("f", "s", DateTime.Now, "e", new Company());

        var mockCreditService = new Mock<ICustomerCreditService>();
        mockCreditService.Setup(x => x.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth))
            .Verifiable();

        var service = new GoldCreditLimitService(mockCreditService.Object);

        var result = service.SetCreditLimit(customer);

        result.CreditLimit.Should().Be(creditLimit);
        result.HasCreditLimit.Should().BeFalse();

        mockCreditService.Verify(x => x.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth),
            Times.Never);
    }
}