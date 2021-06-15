using System.Threading.Tasks;
using TPICAP.API.Models;

namespace TPICAP.API.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> CreateJwtSecurityToken(LoginModel login);
    }
}
