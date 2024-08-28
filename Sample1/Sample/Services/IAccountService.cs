using SitraLand_Api.Models.Response;
using SitraLand_Api.Models.UserModels;

namespace SitraLand_Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseGlobal> SignUp(UserDTO model);
        Task<ResponseGlobal> Login(LoginUserDTO model);
        Task<ResponseGlobal> AddRoleToUser(AddRoleToUserModel model);
        Task<ResponseGlobal> GetRoleBasedUsers(string role);
        Task<ResponseGlobal> GetRoles();
    }
}
