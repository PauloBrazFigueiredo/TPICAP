using System.Collections.Generic;
using System.Threading.Tasks;
using TPICAP.Data.Models;

namespace TPICAP.Data.Interfaces
{
    public interface IPersonsRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        Task<Person> AddAsync(Person person);
        Task<Person> ModifyAsync(Person person);
        Task RemoveByIdAsync(int id);
    }
}
