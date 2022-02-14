using App.Abstraction;
using App.Abstraction.Data;
using App.Abstraction.Services.CreditLimit;
using App.Abstraction.ThirdParty;
using App.Infrastructure.Services;
using Moq;

namespace App.Test.Infrastructure;

public class CustomerServiceTestFixture
{
    public CustomerService Service;
    public Mock<ICompanyRepository> MockCompanyRepository;
    public Mock<ICustomerDataAccessWrapper> MockDataAccessWrapper;
    public Mock<ICustomerCreditService> MockCreditService;
    public Mock<ICustomerValidator> MockCustomerValidator;
    public Mock<ICreditLimitFactory> MockCreditLimitFactory;
    public Mock<ICreditLimitService> MockCreditLimitService;

    public CustomerServiceTestFixture()
    {
        MockCompanyRepository = new();
        MockDataAccessWrapper = new();
        MockCreditService = new();
        MockCustomerValidator = new();
        MockCreditLimitFactory = new();
        MockCreditLimitService = new();

        Service = new(MockCustomerValidator.Object, MockCompanyRepository.Object, MockCreditLimitFactory.Object, MockDataAccessWrapper.Object);
    }
}