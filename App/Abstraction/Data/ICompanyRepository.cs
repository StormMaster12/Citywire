using System.Threading.Tasks;
using App.Abstraction.Models;

namespace App.Abstraction.Data
{
    public interface ICompanyRepository
    {
        Task<Company?> GetById(int id);
    }
}