using System.Threading.Tasks;
using App.Abstraction.Data;
using App.Abstraction.Models;

namespace App.Infrastructure.Data;

public class CustomerDataAccessWrapper : ICustomerDataAccessWrapper
{
    public Task AddCustomer(Customer customer)
    {
        return CustomerDataAccess.AddCustomer(customer);
    }
}