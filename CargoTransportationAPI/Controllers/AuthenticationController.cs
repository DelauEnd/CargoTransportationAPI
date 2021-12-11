using CargoTransportationAPI.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ExtendedControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAuthenticationManager authManager;

        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IAuthenticationManager authManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.authManager = authManager;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userForCreation"></param>
        /// <returns>Returns the newly created user</returns>
        /// <response code="400">If created user is incorrect</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForCreationDto userForCreation)
        {
            var user = mapper.Map<User>(userForCreation);

            var result = await userManager.CreateAsync(user, userForCreation.Password);

            if (!result.Succeeded)
                return BuildUnregistratedResult(result);

            if (userForCreation.Roles != null)
            {
                var validRoles = await ValidateAndRebuildRolesAsync(userForCreation.Roles);
                await userManager.AddToRolesAsync(user, validRoles);
            }

            return Ok(userForCreation);
        }

        /// <summary>
        /// Authenticate user by username and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns access token for authenticated user</returns>
        /// <response code="400">If sended is null</response>
        /// <response code="401">If incorrect username or password</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            var validUser = await authManager.ReturnUserIfValid(user);

            if (validUser == null)
            {
                logger.LogWarn($"{nameof(Authenticate)}: wrong login or password");
                return Unauthorized();
            }

            return Ok(new { Token = await authManager.CreateToken(validUser) });
        }


        private async Task<ICollection<string>> ValidateAndRebuildRolesAsync(ICollection<string> roles)
        {
            foreach (var role in roles)
                await RemoveIfNotExistAsync(roles, role);

            return roles;
        }

        private async Task RemoveIfNotExistAsync(ICollection<string> roles, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
                roles.Remove(role);
        }

        private IActionResult BuildUnregistratedResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
    }
}