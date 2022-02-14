using System;
using App.Abstraction;
using App.Infrastructure.Services;
using Moq;

namespace App.Test.Infrastructure;

public class CustomerValidatorTestFixture
{
    public CustomerValidator Validator;

    public CustomerValidatorTestFixture()
    {
        var mockProvider = new Mock<IDateTimeProvider>();
        mockProvider.Setup(x => x.Now()).Returns(new DateTime(2022, 1, 1));

        Validator = new CustomerValidator(mockProvider.Object);
    }
}