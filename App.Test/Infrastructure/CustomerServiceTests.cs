using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Abstraction.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Test.Infrastructure;

public class CustomerServiceTests : IClassFixture<CustomerServiceTestFixture>
{
    private readonly CustomerServiceTestFixture _fixture;

    public CustomerServiceTests(CustomerServiceTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddCustomer_Validator_Returns_False_Should_Return_False()
    {
        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);

        _fixture.MockCustomerValidator.Setup(x => x.ValidateCustomer(firstname, surname, email, dob)).Returns(false);

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

        _fixture.MockCustomerValidator.Setup(x => x.ValidateCustomer(firstname, surname, email, dob)).Returns(true);
        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync((Company?) null);

        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task AddCustomer_Company_Classification_Should_Be_Used_To_Get_Credit_Limit()
    {
        var firstname = "f";
        var surname = "s";
        var email = "e@.";
        var dob = new DateTime(1987, 1, 1);
        var companyId = 1;
        var company = new Company {Classification = Classification.Gold};
        var customer = new Customer(firstname, surname, dob, email, company)
            {HasCreditLimit = true, CreditLimit = 2000};

        _fixture.MockCustomerValidator.Setup(x => x.ValidateCustomer(firstname, surname, email, dob)).Returns(true);
        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync(company);
        _fixture.MockCreditLimitFactory.Setup(x => x.GetCreditLimitService(company.Classification))
            .Returns(_fixture.MockCreditLimitService.Object);
        _fixture.MockCreditLimitService.Setup(x => x.SetCreditLimit(It.IsAny<Customer>())).Returns(customer);

        Expression<Func<Customer, bool>> customerExpression = x => x.HasCreditLimit == customer.HasCreditLimit
                                                                   && x.CreditLimit == customer.CreditLimit
                                                                   && x.Company.Classification == company.Classification
                                                                   && x.DateOfBirth == dob
                                                                   && x.EmailAddress == email
                                                                   && x.Firstname == firstname
                                                                   && x.Surname == surname;

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

        var company = new Company {Classification = Classification.Gold};
        var customer = new Customer(firstname, surname, dob, email, company) {HasCreditLimit = true, CreditLimit = 2};

        _fixture.MockCustomerValidator.Setup(x => x.ValidateCustomer(firstname, surname, email, dob)).Returns(true);
        _fixture.MockCompanyRepository.Setup(x => x.GetById(companyId)).ReturnsAsync(company);
        _fixture.MockCreditLimitFactory.Setup(x => x.GetCreditLimitService(company.Classification))
            .Returns(_fixture.MockCreditLimitService.Object);
        _fixture.MockCreditLimitService.Setup(x => x.SetCreditLimit(It.IsAny<Customer>())).Returns(customer);

        var result = await _fixture.Service.AddCustomer(firstname, surname, email, dob, 1);

        result.Should().BeFalse();
    }
}