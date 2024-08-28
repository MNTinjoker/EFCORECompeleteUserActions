using SitraLand_Api.Models.UserModels;

namespace SitraLand_Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(LoginUserDTO userDto);
        Task<string> CreateToken();
    }
}
