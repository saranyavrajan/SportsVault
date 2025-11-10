using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SportsVault.Entity;
using SportsVault.Models;
using SportsVault.Services.Implementations;
using SportsVault.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using static SportsVault.Services.Implementations.UserService;

namespace SportsVault.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class UserController(IUserService userService, IAuthService authService) : ControllerBase
    {
        //List All Users
        [HttpGet]
        [SwaggerOperation(Summary = "List All Users", Description = "Gives a list of all current Users.")]
        public async Task<ActionResult> GetAllUsers (CancellationToken ct)
        {
            var users = await userService.GetAllUsersAsync(ct);

            if (users == null || !users.Any())
                return NotFound("No users found.");

            return Ok(users);
        }

        //Create new user  
        [HttpPost]        
        [SwaggerOperation(Summary = "Create New User", Description = "Create a new user.")]  
        public async Task<ActionResult<UserCreateDto>> CreateUser([FromBody] UserCreateDto request, CancellationToken ct) 
        {
            var usercreated = await userService.CreateUserAsync(request, ct);
            if (usercreated is null)
                return Conflict(new ProblemDetails { Title = "Email already exists." });

            return Ok(usercreated);  // Need to point to get
        }

        // List all users
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Check User Login", Description = "Check if a user has valid access or not.")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result is null)
                return BadRequest("Invalid Username or Password.");
            return Ok(result);
        }

        //Refresh a RefreshToken
        [HttpPost("refresh-token")]
        [SwaggerOperation(Summary = "Generate New Refresh Token", Description = "Generate New Refresh Token after expiry.")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid Refresh Token.");

            return Ok(result);
        }
        public sealed record AdminUpgradeRequest(string Email);

        [Authorize(Roles = "admin")]
        [HttpPatch("adminrole")]
        [SwaggerOperation(Summary = "Change role to admin performed by Admin Role", Description = "Give Admin Access for User.")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangeAdminRole([FromBody] AdminUpgradeRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Email))
                return BadRequest("Email is required.");

            var result = await userService.AdminUpgradeResultAsync(req.Email, ct);

            return result switch
            {
                AdminUpgradeResult.NotFound => NotFound("User not found."),
                AdminUpgradeResult.AlreadyAdmin => Ok(new { message = "User is already Admin." }),
                AdminUpgradeResult.Promoted => Ok(new { message = "User promoted to Admin." }),
                AdminUpgradeResult.NotApplicable => BadRequest("Role not eligible for admin promotion."),
                _ => BadRequest("Unable to update role.")
            };
        }
        
        public sealed record DeleteUserRequest(string Email);

        [Authorize(Roles = "admin")]
        [HttpDelete("delete")]
        [SwaggerOperation(Summary = "Delete a User", Description = "Delete a User from Users table")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest req, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(req.Email))
                return BadRequest("Enter a valid Email.");

            var deleted = await userService.DeleteUserByEmailAsync(req.Email, ct);

            if (!deleted)
                return NotFound("User not found.");

            return Ok(new { message = "User deleted successfully." });
        }
    }
}
