using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileStore.DTO.ModelsToAuthorize;
using MobileStore.Models;
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
        readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }
        [HttpPost("/SignIn")]
        public async Task<IActionResult> SignIn(UserForSignInDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "ValidationError" });
            }

            var result = await _signInManager.PasswordSignInAsync(userDto.UserName, userDto.Password,
                false, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(userDto.ReturnUrl) &&
                    Url.IsLocalUrl(userDto.ReturnUrl))
                {
                    return Redirect(userDto.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("ShowProducts", "Home");
                }
            }
            return BadRequest(new
            {
                errorText = "Incorrect Username or Password"
            });
            //var response = new
            //{
            //    access_token = encodedJwt,
            //    username = user.UserName
            //};
        }
        
        [HttpPost("/SignUp")]
        public async Task<IActionResult> SignUp(UserForSignUpDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "ValidationError" });
            }

            //if (user != null)
            //{
            //    return BadRequest(new
            //    {
            //        errorText = "User already exists"
            //    });
            //}
            var user = new User
            {
                UserName = userDto.UserName,

            };
            var result = await _userManager.CreateAsync(user, userDto.Password);
            //var encodedJwt = Authenticate(user.UserName,db.Roles.FirstOrDefault(r=>r.Id==user.RoleId).Name);
            //var response = new
            //{
            //    access_token = encodedJwt,
            //    username = user.UserName
            //};
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false/*,"Authenticate"*/);
                return Ok();
            }
            return BadRequest(new
            {
                errorText = "User already exists"
            });

        }

        [HttpPost("/SignOut")]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return Ok();
        }
        
        private static string Authenticate(string username, string role)
        {

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };
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
