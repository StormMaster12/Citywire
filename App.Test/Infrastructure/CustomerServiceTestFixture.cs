using App.Abstraction;
using App.Abstraction.Data;
using App.Abstraction.Services.CreditLimit;
using App.Abstraction.ThirdParty;
using App.Infrastructure.Services;
using Moq;

namespace App.Test.Infrastructure;

public class CustomerServiceTestFixture
{
    public Mock<ICompanyRepository> MockCompanyRepository;
    public Mock<ICreditLimitFactory> MockCreditLimitFactory;
    public Mock<ICreditLimitService> MockCreditLimitService;
    public Mock<ICustomerCreditService> MockCreditService;
    public Mock<ICustomerValidator> MockCustomerValidator;
    public Mock<ICustomerDataAccessWrapper> MockDataAccessWrapper;
    public CustomerService Service;

    public CustomerServiceTestFixture()
    {
        MockCompanyRepository = new Mock<ICompanyRepository>();
        MockDataAccessWrapper = new Mock<ICustomerDataAccessWrapper>();
        MockCreditService = new Mock<ICustomerCreditService>();
        MockCustomerValidator = new Mock<ICustomerValidator>();
        MockCreditLimitFactory = new Mock<ICreditLimitFactory>();
        MockCreditLimitService = new Mock<ICreditLimitService>();

        Service = new CustomerService(MockCustomerValidator.Object, MockCompanyRepository.Object,
            MockCreditLimitFactory.Object, MockDataAccessWrapper.Object);
    }
}