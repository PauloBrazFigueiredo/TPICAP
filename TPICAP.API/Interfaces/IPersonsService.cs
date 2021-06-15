using System.Collections.Generic;
using System.Threading.Tasks;
using TPICAP.API.Models;

namespace TPICAP.API.Interfaces
{
    public interface IPersonsService
    {
        Task<IEnumerable<PersonResponseModel>> GetAllAsync();
        Task<PersonResponseModel> GetByIdAsync(int id);
        Task<PersonResponseModel> AddAsync(PersonCreationModel person);
        Task<PersonResponseModel> ModifyAsync(PersonModificationModel person);
        Task RemoveByIdAsync(int id);
    }
}
