using System;
using App.Abstraction.Models;
using App.Abstraction.ThirdParty;
using App.Infrastructure.Services.CreditLimit;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Test.Infrastructure.CreditLimit;

public class BronzeCreditLimitServiceTests
{
    [Fact]
    public void SetCreditLimit_Should_Set_Credit_Limit_And_Has_Limit()
    {
        var creditLimit = 20;
        var customer = new Customer("f", "s", DateTime.Now, "e", new Company());

        Mock<ICustomerCreditService> mockCreditService = new Mock<ICustomerCreditService>();
        mockCreditService.Setup(x => x.GetCreditLimit(customer.Firstname, customer.Surname, customer.DateOfBirth)).Returns(creditLimit);

        var service = new BronzeCreditLimitService(mockCreditService.Object);

        var result = service.SetCreditLimit(customer);

        result.CreditLimit.Should().Be(creditLimit);
        result.HasCreditLimit.Should().BeTrue();
    }
}