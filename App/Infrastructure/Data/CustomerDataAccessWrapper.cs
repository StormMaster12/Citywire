using System.Threading.Tasks;
using App.Abstraction.Data;
using App.Abstraction.Models;

namespace App.Infrastructure.Data;

public class CustomerDataAccessWrapper : ICustomerDataAccessWrapper
{
    public Task AddCustomer(Customer customer)
    {
        //Not awaiting here because only wrapper, and do not need the overhead.
        return CustomerDataAccess.AddCustomer(customer);
    }
}