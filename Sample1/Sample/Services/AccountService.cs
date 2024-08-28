using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SitraLand_Api.Data;
using SitraLand_Api.Models.Response;
using SitraLand_Api.Models.UserModels;
using SitraLand_Api.Services.Interfaces;
using System.Net;

namespace SitraLand_Api.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private ApiUser _user;
        private readonly DatabaseContext _databaseContext;

        public AccountService(UserManager<ApiUser> userManager, IMapper mapper, IAuthService authService, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authService = authService;
            _databaseContext = databaseContext;
        }

        public async Task<ResponseGlobal> SignUp(UserDTO model)
        {
            var response = new ResponseGlobal();

            try
            {
                if (model != null)
                {
                    var user = _mapper.Map<ApiUser>(model);
                    user.UserName = model.FullName;
                    user.PhoneNumber = model.Mobile;

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        _user = await _userManager.FindByNameAsync(user.UserName);
                        await _userManager.AddToRoleAsync(_user, "User");
                        response.Message = "User Created Successfully";
                        response.Code = 0;
                    }
                    else
                    {
                        response.Data = result.Errors;
                    }
                }
                else
                {
                    response.Message = "Model User Is Not Valid";
                    response.Code = 1;
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseGlobal> Login(LoginUserDTO model)
        {
            var result = new ResponseGlobal();

            try
            {
                if (!await _authService.ValidateUser(model))
                {
                    result.Message = "Dont Have Access";
                    return result;
                }

                _user = await _userManager.FindByNameAsync(model.UserName);
                var roles = await _userManager.GetRolesAsync(_user);
                string role = roles[0];
                result.Data = await _authService.CreateToken();
                result.Message = role;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseGlobal> AddRoleToUser(AddRoleToUserModel model)
        {
            var result = new ResponseGlobal();

            try
            {
                _user = await _userManager.FindByNameAsync(model.UserName);
                if (_user == null)
                {
                    result.Message = "User Not Exists Or Mobile Wrong";
                }

                var roles = await _userManager.RemoveFromRoleAsync(_user, "Admin");
                var removeUserRole = await _userManager.RemoveFromRoleAsync(_user, "User");

                await _userManager.AddToRoleAsync(_user, model.Role);
                result.Message = "Success";
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseGlobal> GetRoleBasedUsers(string role)
        {
            var result = new ResponseGlobal();

            try
            {
                var users = await _userManager.GetUsersInRoleAsync(role);
                var userWithRole = users.Select(u => new
                {
                    u.UserName,
                    u.PhoneNumber
                }).ToList();

                result.Message = "success";
                result.Code = (int)HttpStatusCode.OK;
                result.Data = userWithRole;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseGlobal> GetRoles()
        {
            var result = new ResponseGlobal();

            try
            {
                var roles = await _databaseContext.Roles.AsNoTracking().ToListAsync();
                result.Data = roles;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
