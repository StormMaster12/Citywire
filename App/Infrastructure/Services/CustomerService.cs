using System;
using System.Threading.Tasks;
using App.Abstraction;
using App.Abstraction.Data;
using App.Abstraction.Models;
using App.Abstraction.Services.CreditLimit;

namespace App.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerValidator _customerValidator;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICreditLimitFactory _creditLimitFactory;
        private readonly ICustomerDataAccessWrapper _customerDataAccessWrapper;

        public CustomerService(ICustomerValidator customerValidator, ICompanyRepository companyRepository, ICreditLimitFactory creditLimitFactory, ICustomerDataAccessWrapper customerDataAccessWrapper)
        {
            _customerValidator = customerValidator;
            _companyRepository = companyRepository;
            _creditLimitFactory = creditLimitFactory;
            _customerDataAccessWrapper = customerDataAccessWrapper;
        }

        public async Task<bool> AddCustomer(string firstname, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            if (!_customerValidator.ValidateCustomer(firstname, surname, email, dateOfBirth))
            {
                return false;
            }

            var company = await _companyRepository.GetById(companyId);
            if (company == null)
            {
                return false;
            }

            var customer = new Customer(firstname, surname, dateOfBirth, email, company);

            customer = _creditLimitFactory.GetCreditLimitService(company.Classification).SetCreditLimit(customer);

            if (customer.HasCreditLimit && customer.CreditLimit < 500)
            {
                return false;
            }

            await _customerDataAccessWrapper.AddCustomer(customer);

            return true;
        }
    }
}
