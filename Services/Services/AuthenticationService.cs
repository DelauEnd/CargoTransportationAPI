using AutoMapper;
using Logistics.Models;
using Logistics.Models.RequestDTO.CreateDTO;
using Logistics.Models.ResponseDTO;
using Logistics.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Logistics.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public readonly UserManager<User> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly IAuthenticationManager authManager;
        public readonly IMapper mapper;

        public AuthenticationService(UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, IAuthenticationManager authManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.authManager = authManager;
            this.mapper = mapper;
        }

        public async Task AddRoleToUser(string login, string role)
        {
            var user = await userManager.FindByNameAsync(login);

            if (user == null)
                throw new Exception("User not found");

            if (!await roleManager.RoleExistsAsync(role))
                throw new Exception("Role not exists");

            await userManager.AddToRoleAsync(user, role);

        }

        public async Task<AuthenticatedUserInfo> AuthenticateUser(UserForAuthenticationDto user)
        {
            var validUser = await authManager.ReturnUserIfValid(user);

            if (validUser == null)
            {
                throw new Exception("Unauthorized");
            }

            return new AuthenticatedUserInfo
            {
                AuthToken = await authManager.CreateToken(validUser),
                UserRoles = await userManager.GetRolesAsync(validUser)
            };
        }

        public async Task CreateUser(UserForCreationDto userForCreation)
        {
            var user = mapper.Map<User>(userForCreation);

            var result = await userManager.CreateAsync(user, userForCreation.Password);

            if (!result.Succeeded)
            {
                var errors = "";
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Code}: {error.Description}\n";
                }
                throw new Exception(errors);
            }
        }
    }
}
