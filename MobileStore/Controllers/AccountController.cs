using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileStore.DTO.AccountModels;
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
        MobileStoreDbContext db;
        public AccountController(MobileStoreDbContext context)
        {
            db = context;
        }
        [HttpPost("/SignIn")]
        public async Task<IActionResult> SignIn(UserForSignInDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "ValidationError" });
            }
            var user = await GetIdentity(userDto);
            if (user == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            var encodedJwt = Authenticate(user.UserName,user.RoleId.ToString());



            var response = new
            {
                access_token = encodedJwt,
                username = user.UserName
            };

            return Ok(response);
        }
        [Route("/SignUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp(UserForSignUpDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "ValidationError" });
            }
            var user = await GetIdentity(userDto);
            if (user != null)
            {
                return BadRequest(new
                {
                    errorText = "User already exists"
                });
            }
            user=new User
            {
                UserName = userDto.UserName,
                RoleId = userDto.RoleId,
                Password = userDto.Password
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            var encodedJwt=Authenticate(user.UserName,user.RoleId.ToString());
            var response = new
            {
                access_token = encodedJwt,
                username = user.UserName
            };
            return Ok(response);

        }

        private async Task<User> GetIdentity(AuthorizeModel model)
        {
            return await db.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName && x.Password == model.Password);

        }
        private static string Authenticate(string username,string role)
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
