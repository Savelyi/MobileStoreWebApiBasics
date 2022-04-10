using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileStore.DTO.ModelsToAuthorize;
using MobileStore.Models;
using MobileStore.Utils.Authenticate;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MobileStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }
        [HttpPost("/SignIn")]
        public async Task<IActionResult> SignIn(UserForSignInDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "ValidationError" });
            }
            var user = await _userManager.FindByNameAsync(userDto.UserName);
            bool correctPassword = await _userManager.CheckPasswordAsync(user, userDto.Password);
            if (user == null || !correctPassword)
            {
                return Unauthorized();
            }
            await _userManager.AddToRoleAsync(user, "user");

            var encodedJwt = CreateToken(user);

            var response = new
            {
                access_token = encodedJwt,
                username = user.UserName
            };
            return Ok(response);


        }

        [HttpPost("/SignUp")]
        public async Task<IActionResult> SignUp(UserForSignUpDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "ValidationError" });
            }


            var user = new User
            {
                UserName = userDto.UserName,

            };
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                    return BadRequest(ModelState);
                }
            }
            await _userManager.AddToRoleAsync(user, "user");
            return StatusCode(201);



        }

        

        private async Task<string> CreateToken(IdentityUser user)
        {

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, await _userManager.GetUserNameAsync(user)),
                };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role);
            }
            ClaimsIdentity identity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);


        }
    }
}
