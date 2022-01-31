using CargoTransportationAPI.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Enums;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Controllers.v1
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

            return Ok(userForCreation);
        }

        /// <summary>
        /// Add role to user
        /// | Required role: Administrator
        /// </summary>
        /// <param name="login"></param>
        /// <param name="role"></param>
        /// <returns>Returns edited user</returns>
        /// <response code="400">If userRole not exists</response>
        /// <response code="401">If user unauthenticated</response>
        /// <response code="404">If user not found</response>
        /// <response code="403">If user authenticated but has incorrect role</response>
        /// <response code="500">Unhandled exception</response>
        [HttpPost]
        [Route("AddRole")]
        [Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> AddRoleToUser([FromQuery]string login, [FromQuery]string role)
        {
            var user = await userManager.FindByNameAsync(login);

            if (user == null)
                return NotFound();

            if (!await roleManager.RoleExistsAsync(role))
                return BadRequest();

            await userManager.AddToRoleAsync(user, role);

            return Ok(user);
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

            return Ok(new { Token = await authManager.CreateToken(validUser), Roles = await userManager.GetRolesAsync(validUser) });
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