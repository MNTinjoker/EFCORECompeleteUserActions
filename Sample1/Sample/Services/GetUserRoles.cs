using Microsoft.AspNetCore.Identity;
using SitraLand_Api.Data;

namespace SitraLand_Api.Services.Implementations
{
    public class GetUserRoles
    {
        private ApiUser _user;
        private readonly UserManager<ApiUser> _userManager;
        public GetUserRoles(UserManager<ApiUser> usermanager)
        {
            _userManager = usermanager;
        }
        public async Task<string> UserRole()
        {
            var roles = await _userManager.GetRolesAsync(_user);
            string role = roles.ToString();
            return role;
        }
    }
}
