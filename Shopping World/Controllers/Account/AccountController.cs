using DataAccess.IRepository;
using DataModels.Auth;
using DataModels.Auth.View;
using DataModels.Other.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace Shopping_World.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitRepository _unit;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IUnitRepository unit, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManger = userManager;
            _unit = unit;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> RegistrationAsync([FromBody] RegistrationViewModel model)
        {
            if (_unit.Users.IfUserEmailAlreadyExists(Guid.Empty, model.Email))
                return Ok(new { statusCode = 400, message = "Email already taken" });

            if (_unit.Users.IfPhoneNumberAlreadyExists(Guid.Empty, model.PhoneNumber))
                return Ok(new { statusCode = 400, message = "Phone Number already taken" });

            var user = new ApplicationUser()
            {
                FullName = model.Name,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                TimeStamp = DateTime.UtcNow.AddHours(6),
                CustomerId = $"SHW{_unit.Users.TotalRecord()}",
                
            };

            await _userManger.CreateAsync(user, model.Password);
            await _userManger.AddToRoleAsync(user, SD.Role_User);

            _unit.UserToken.Add(new UserTokens()
            {
                ApplicationUserId = user.Id,
                Token = CryptographyService.Encrypt(model.Password)
            });

            return Ok(new
            {
                StatusCode = 200,
                message = "Registration Complete. Please Login",
            });
        }

        [AllowAnonymous]
        [HttpPost("ifuseremailalreadyexists")]
        public IActionResult IfUserEmailAlreadyExists([FromBody] IfExistsViewModel model) =>
            _unit.Users.IfUserEmailAlreadyExists(model.Id, model.Name) ?
            Ok(new { statusCode = 200, value = true }) :
            Ok(new { statusCode = 200, value = false });

        [AllowAnonymous]
        [HttpPost("ifphonenumberalreadyexists")]
        public IActionResult IfPhoneNumberAlreadyExists([FromBody] IfExistsViewModel model) =>
            _unit.Users.IfPhoneNumberAlreadyExists(model.Id, model.Name) ?
            Ok(new { statusCode = 200, value = true }) :
            Ok(new { statusCode = 200, value = false });

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return Ok(new { statusCode = 401, message = "Invalid Email or Password" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Ok(new { statusCode = 401, message = "Invalid Email or Password" });

            AuthService authService = new(_userManger, _configuration);

            AuthViewModel token = await authService.GetTokenAsync(user);
            token.RefreshToken = authService.GenerateRefreshToken();
            token.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshDurationInDays"]));

            RefreshToken refreshToken = new()
            {
                Token = token.RefreshToken,
                Expires = token.RefreshTokenExpireTime,
                Created = DateTime.UtcNow,
                ApplicationUserId = user.Id,
                ActualToken = token.Token
            };

            if (user.RegistrationFeePaid)
            {
                token.RegFP = user.CustomerId;
            }

            var perviousRefreshTokens = _unit.RefreshToken.GetRefreshTokenByUserId(user.Id);
            _unit.RefreshToken.RemoveRange(perviousRefreshTokens);

            _unit.RefreshToken.Add(refreshToken);

            return Ok(new
            {
                statusCode = 200,
                value = token
            });
        }

        

        [HttpPost("getall")]
        [Authorize]
        public IActionResult GetAll([FromBody] FilterModel model)
        {
            if (!string.IsNullOrEmpty(model.GlobalFilter))
            {
                model.GlobalFilter = model.GlobalFilter.Trim();
            }

            var x = _unit.Users.UserList(model);
            if (x.FirstOrDefault() == null)
                return Ok(new { statusCode = 204, message = "No Record Found" });

            var list = new List<ApplicationUser>();

            return Ok(new
            {
                statusCode = 200,
                value = list,
                totalRecords = _unit.Users.TotalRecord(model)
            });
        }

        [HttpGet("autologin/{customerId}")]
        //[Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> AutoLoginAsync(string customerId)
        {
            var user = _unit.Users.GetByCustomerId(customerId);
            if (user == null)
                return Ok(new { statusCode = 204, message = "User Not Found" });

            var token1 = _unit.UserToken.GetByUserId(user.Id);
            if (token1 == null)
                return Ok(new { statusCode = 204, message = "Token Not Found" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, CryptographyService.Decrypt(token1.Token), false);
            if (!result.Succeeded)
                return Ok(new { statusCode = 401, message = "Invalid Email or Password" });

            AuthService authService = new(_userManger, _configuration);

            AuthViewModel token = await authService.GetTokenAsync(user);
            token.RefreshToken = authService.GenerateRefreshToken();
            token.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshDurationInDays"]));

            RefreshToken refreshToken = new()
            {
                Token = token.RefreshToken,
                Expires = token.RefreshTokenExpireTime,
                Created = DateTime.UtcNow,
                ApplicationUserId = user.Id,
                ActualToken = token.Token
            };

            if (user.RegistrationFeePaid)
            {
                token.RegFP = user.CustomerId;
            }

            var perviousRefreshTokens = _unit.RefreshToken.GetRefreshTokenByUserId(user.Id);
            _unit.RefreshToken.RemoveRange(perviousRefreshTokens);

            _unit.RefreshToken.Add(refreshToken);

            return Ok(new
            {
                statusCode = 200,
                value = token
            });
        }
       
    }
}
