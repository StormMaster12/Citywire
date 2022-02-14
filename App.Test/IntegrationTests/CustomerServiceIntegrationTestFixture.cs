using System;
using System.Collections.Generic;
using App.Abstraction;
using App.Abstraction.Data;
using App.Abstraction.Services.CreditLimit;
using App.Abstraction.ThirdParty;
using App.Infrastructure.Services;
using App.Infrastructure.Services.CreditLimit;
using Moq;

namespace App.Test.IntegrationTests;

public class CustomerServiceIntegrationTestFixture
{
    public Mock<ICompanyRepository> MockCompanyRepository;
    public Mock<ICustomerCreditService> MockCreditService;
    public Mock<ICustomerDataAccessWrapper> MockDataAccessWrapper;
    public CustomerService Service;

    public CustomerServiceIntegrationTestFixture()
    {
        MockCompanyRepository = new Mock<ICompanyRepository>();
        MockDataAccessWrapper = new Mock<ICustomerDataAccessWrapper>();
        MockCreditService = new Mock<ICustomerCreditService>();

        var creditLimitFactory = new CreditLimitFactory(new List<ICreditLimitService>
        {
            new BronzeCreditLimitService(MockCreditService.Object),
            new GoldCreditLimitService(MockCreditService.Object),
            new SilverCreditLimitService(MockCreditService.Object)
        });

        var mockProvider = new Mock<IDateTimeProvider>();
        mockProvider.Setup(x => x.Now()).Returns(new DateTime(2022, 1, 1));

        Service = new CustomerService(new CustomerValidator(mockProvider.Object), MockCompanyRepository.Object,
            creditLimitFactory, MockDataAccessWrapper.Object);
    }
}