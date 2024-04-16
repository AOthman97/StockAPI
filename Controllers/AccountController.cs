using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService,
        SignInManager<AppUser> signInManager) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        private OkObjectResult UserInfo(AppUser? user)
        {
            return Ok(
                new NewUserDto
                {
                    Email = user.Email,
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
                });
        }

        private UnauthorizedObjectResult UnAuthorized() => Unauthorized("Invalid Username/Password!");

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

                if (user == null) return UnAuthorized();

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!result.Succeeded) return UnAuthorized();

                return UserInfo(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (roleResult.Succeeded)
                    {
                        return UserInfo(appUser);
                    }
                    else
                        return StatusCode(500, roleResult.Errors);
                }
                else
                    return StatusCode(500, createdUser.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}