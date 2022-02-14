using System;
using System.Threading.Tasks;
using App.Abstraction;
using App.Infrastructure;
using App.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Test.Infrastructure;

public class CustomerValidatorTests : IClassFixture<CustomerValidatorTestFixture>
{
    private readonly CustomerValidatorTestFixture _fixture;

    public CustomerValidatorTests(CustomerValidatorTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("", "s", "e@.", "1987-01-11")]
    [InlineData("f", "", "e@.", "1987-01-11")]
    [InlineData("f", "s", "", "1987-01-11")]
    [InlineData("f", "s", "e.", "1987-01-11")]
    [InlineData("f", "s", "e@", "1987-01-11")]
    [InlineData("f", "s", "e@.", "2022-1-1")]
    public void ValidateCustomer_Invalid_Values_Should_Return_False(string firstname, string surname, string email, DateTime dob)
    {
        var result = _fixture.Validator.ValidateCustomer(firstname, surname, email, dob);

        result.Should().BeFalse();
    }

    [Fact]
    public void ValidateCustomer_Valid_Values_Should_Return_True()
    {
        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);

        var result = _fixture.Validator.ValidateCustomer(firstname, surname, email, dob);

        result.Should().BeTrue();
    }
}