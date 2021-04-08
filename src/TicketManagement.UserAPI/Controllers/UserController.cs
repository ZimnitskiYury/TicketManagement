using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.UserAPI.DataAccess;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Services;

namespace TicketManagement.UserAPI.Controllers
{
    /// <summary>
    /// Controller for all actions with db of users.
    /// </summary>
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="roleManager">RoleManager of Identity.</param>
        /// <param name="signInManager">SignInManager of Identity.</param>
        /// <param name="userManager">UserManager of Identity.</param>
        /// <param name="jwtTokenService">JwtTocken service for create tokens.</param>
        public UsersController(
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            JwtTokenService jwtTokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="model">Model contains Username, e-mail, language and password.</param>
        /// <returns>jwt-token.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                SurName = model.SurName,
                Email = model.Email,
                UserName = model.Login,
                Language = model.Language,
                Balance = 0,
            };

            await CreateRoleIfNotExists("Admin");
            await CreateRoleIfNotExists("Venue");
            await CreateRoleIfNotExists("Event");
            await CreateRoleIfNotExists("User");

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                switch (model.Login)
                {
                    case "Admin":
                        {
                            await _userManager.AddToRoleAsync(user, "Admin");
                            break;
                        }

                    case "Venue":
                        {
                            await _userManager.AddToRoleAsync(user, "Venue");
                            break;
                        }

                    case "Event":
                        {
                            await _userManager.AddToRoleAsync(user, "Event");
                            break;
                        }

                    default:
                        {
                            await _userManager.AddToRoleAsync(user, "User");
                            break;
                        }
                }

                var roles = await _userManager.GetRolesAsync(user);
                return Ok(_jwtTokenService.GetToken(user, roles));
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Login into API and get token.
        /// </summary>
        /// <param name="model">Model from UI.</param>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Login);
                var roles = await _userManager.GetRolesAsync(user);
                return Ok(_jwtTokenService.GetToken(user, roles));
            }

            return Forbid();
        }

        /// <summary>
        /// All available Roles. Only for Admin.
        /// </summary>
        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles;
            return roles!=null ? Ok(roles) : (IActionResult)Forbid();
        }

        /// <summary>
        /// Add role to user. Only for Admin.
        /// </summary>
        [HttpGet("{login}/roles/{role}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleToUserAsync(string login, string role)
        {
            var user = await _userManager.FindByNameAsync(login);

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok();
            }

            return Forbid();
        }

        /// <summary>
        /// Delete role from user. Only for Admin.
        /// </summary>
        [HttpDelete("{login}/roles/{role}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoleFromUserAsync(string login, string role)
        {
            var user = await _userManager.FindByNameAsync(login);

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok();
            }

            return Forbid();
        }

        /// <summary>
        /// Get info of user.
        /// </summary>
        [Authorize]
        [HttpGet("profile/{login}")]
        public async Task<IActionResult> Profile(string login)
        {
            var result = await _userManager.FindByNameAsync(login);
            var roles = await _userManager.GetRolesAsync(result);
            if (result != null)
            {
                var user = new ProfileModel
                {
                    Id = result.Id,
                    Login = result.UserName,
                    FirstName = result.FirstName,
                    SurName = result.SurName,
                    Email = result.Email,
                    Language = result.Language,
                    Balance = result.Balance,
                    Roles = roles,
                };
                return Ok(user);
            }

            return Forbid();
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="user">User info for changes.</param>
        [Authorize]
        [HttpPost("profile/edit")]
        public async Task<IActionResult> Edit([FromForm] ProfileModel user)
        {
            var userBase = await _userManager.FindByIdAsync(user.Id);
            userBase.FirstName = user.FirstName;
            userBase.SurName = user.SurName;
            userBase.Email = user.Email;
            userBase.Language = user.Language;
            userBase.Balance = user.Balance;
            var result = await _userManager.UpdateAsync(userBase);
            if (result.Succeeded)
            {
                return Ok();
            }

            return Forbid();
        }

        /// <summary>
        /// Validate jwt-token.
        /// </summary>
        /// <param name="token">token for validate.</param>
        [AllowAnonymous]
        [HttpGet("validate")]
        public IActionResult Validate(string token)
        {
            return _jwtTokenService.ValidateToken(token) ? Ok() : (IActionResult)Forbid();
        }

        /// <summary>
        /// Get all user's profile.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = from user in _userManager.Users.ToList()
                        select new ProfileModel
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            SurName = user.SurName,
                            Email = user.Email,
                            Login = user.UserName,
                            Balance = user.Balance,
                            Language = user.Language,
                            Roles = _userManager.GetRolesAsync(user).Result,
                        };
            return Ok(users);
        }

        /// <summary>
        /// Delete selected User by Login/UserName.
        /// </summary>
        /// <param name="login">Username.</param>
        /// <returns>200 - Ok.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{login}")]
        public async Task<IActionResult> DeleteAsync(string login)
        {
            var user = await _userManager.FindByNameAsync(login);
            var result = await _userManager.DeleteAsync(user);

            return Ok(result);
        }

        private async Task CreateRoleIfNotExists(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
