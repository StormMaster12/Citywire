using System;
using System.Threading.Tasks;

namespace App.Abstraction
{
    public interface ICustomerService
    {
        Task<bool> AddCustomer(string firname, string surname, string email, DateTime dateOfBirth, int companyId);
    }
}