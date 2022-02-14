using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Abstraction;
using App.Abstraction.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Test.IntegrationTests;

public class CustomerServiceIntegrationTests : IClassFixture<CustomerServiceIntegrationTestFixture>
{
    private readonly CustomerServiceIntegrationTestFixture _fixture;

    public CustomerServiceIntegrationTests(CustomerServiceIntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData("", "s", "e@.", "1987-01-11")]
    [InlineData("f", "", "e@.", "1987-01-11")]
    [InlineData("f", "s", "", "1987-01-11")]
    [InlineData("f", "s", "e@", "1987-01-11")]
    [InlineData("f", "s", "e.", "1987-01-11")]
    [InlineData("f", "s", "e@.", "2022-1-1")]
    public async Task AddCustomer_Invalid_Values_Should_Return_False(string firstname, string surname, string email, DateTime dob)
    {
        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task AddCustomer_No_Company_Exists_Should_Return_False()
    {
        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);
        var companyId = 1;

        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync((Company?)null);

        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task AddCustomer_Company_Classification_Is_Gold_Should_Skip_Credit_Check()
    {
        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);
        var companyId = 1;

        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync(new Company() { Classification = Classification.Gold });

        Expression<Func<Customer, bool>> customerExpression = customer => !customer.HasCreditLimit;

        _fixture.MockDataAccessWrapper.Setup(x => x.AddCustomer(It.Is(customerExpression))).Verifiable();

        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeTrue();

        _fixture.MockDataAccessWrapper.Verify(x => x.AddCustomer(It.Is(customerExpression)), Times.Once);
    }

    [Fact]
    public async Task AddCustomer_Classification_Is_Silver_Should_Double_Credit_Limit()
    {
        var expectedCreditLimit = 2000;

        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);
        var companyId = 1;
        var creditLimit = 1000;

        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync(new Company() { Classification = Classification.Silver });
        _fixture.MockCreditService.Setup(x => x.GetCreditLimit(firstname, surname, dob)).Returns(creditLimit);

        Expression<Func<Customer, bool>> customerExpression = customer => customer.HasCreditLimit && customer.CreditLimit == expectedCreditLimit;

        _fixture.MockDataAccessWrapper.Setup(x => x.AddCustomer(It.Is(customerExpression))).Verifiable();

        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeTrue();

        _fixture.MockDataAccessWrapper.Verify(x => x.AddCustomer(It.Is(customerExpression)), Times.Once);
    }

    [Fact]
    public async Task AddCustomer_Classification_Is_Bronze_Should_Set_Credit_Limit()
    {
        var expectedCreditLimit = 1000;

        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);
        var companyId = 1;
        var creditLimit = 1000;

        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync(new Company() { Classification = Classification.Bronze });
        _fixture.MockCreditService.Setup(x => x.GetCreditLimit(firstname, surname, dob)).Returns(creditLimit);

        Expression<Func<Customer, bool>> customerExpression = customer => customer.HasCreditLimit && customer.CreditLimit == expectedCreditLimit;

        _fixture.MockDataAccessWrapper.Setup(x => x.AddCustomer(It.Is(customerExpression))).Verifiable();

        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeTrue();

        _fixture.MockDataAccessWrapper.Verify(x => x.AddCustomer(It.Is(customerExpression)), Times.Once);
    }

    [Fact]
    public async Task AddCustomer_Credit_Limit_Set_And_Less_Than_Limit_Should_Return_False()
    {
        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);
        var companyId = 1;
        var creditLimit = 100;

        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync(new Company() { Classification = Classification.Bronze });
        _fixture.MockCreditService.Setup(x => x.GetCreditLimit(firstname, surname, dob)).Returns(creditLimit);

        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task AddCustomer_All_Customer_Fields_Should_Be_Set()
    {
        var expectedCreditLimit = 3000;

        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);
        var companyId = 1;
        var creditLimit = 3000;

        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync(new Company() { Name = "a", Classification = Classification.Bronze });
        _fixture.MockCreditService.Setup(x => x.GetCreditLimit(firstname, surname, dob)).Returns(creditLimit);

        Expression<Func<Customer, bool>> customerExpression = customer => customer.HasCreditLimit
                                                                          && customer.CreditLimit == expectedCreditLimit
                                                                          && customer.Company.Name == "a"
                                                                          && customer.DateOfBirth == dob
                                                                          && customer.EmailAddress == email
                                                                          && customer.Firstname == firstname
                                                                          && customer.Surname == surname;

        _fixture.MockDataAccessWrapper.Setup(x => x.AddCustomer(It.Is(customerExpression))).Verifiable();

        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeTrue();

        _fixture.MockDataAccessWrapper.Verify(x => x.AddCustomer(It.Is(customerExpression)), Times.Once);
    }
}