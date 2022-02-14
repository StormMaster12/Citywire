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
    public CustomerService Service;
    public Mock<ICompanyRepository> MockCompanyRepository;
    public Mock<ICustomerDataAccessWrapper> MockDataAccessWrapper;
    public Mock<ICustomerCreditService> MockCreditService;

    public CustomerServiceIntegrationTestFixture()
    {
        MockCompanyRepository = new();
        MockDataAccessWrapper = new();
        MockCreditService = new();

        var creditLimitFactory = new CreditLimitFactory(new List<ICreditLimitService>()
        {
            new BronzeCreditLimitService(MockCreditService.Object),
            new GoldCreditLimitService(MockCreditService.Object),
            new SilverCreditLimitService(MockCreditService.Object)
        });

        var mockProvider = new Mock<IDateTimeProvider>();
        mockProvider.Setup(x => x.Now()).Returns(new DateTime(2022, 1, 1));

        Service = new(new CustomerValidator(mockProvider.Object), MockCompanyRepository.Object, creditLimitFactory, MockDataAccessWrapper.Object);
    }
}