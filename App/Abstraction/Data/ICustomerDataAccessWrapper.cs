using System.Threading.Tasks;
using App.Abstraction.Models;

namespace App.Abstraction.Data;

public interface ICustomerDataAccessWrapper
{
    Task AddCustomer(Customer customer);
}