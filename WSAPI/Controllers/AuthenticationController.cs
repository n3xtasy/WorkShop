using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WSAPI.Context;
using WSAPI.Logger;
using WSAPI.Models.User;

namespace WSAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController(UserContext userContext)
        {
            UserRepository = new UserRepository(userContext);
        }

        private UserRepository UserRepository { get; set; }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(CreateUserViewModel createUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(createUserViewModel);
            }

            if (UserRepository.Exist(createUserViewModel))
            {
                ModelState.AddModelError("UserExists", "Пользователь с такой почтой уже существует.");

                return BadRequest(ModelState);
            }

            await UserRepository.CreateAsync(createUserViewModel);

            MailLogger.SendRegistrationLog(createUserViewModel.Email, createUserViewModel.Password);

            return Ok();
        }
        private async Task<ClaimsIdentity> GetIdentityAsync(AuthenticateUserViewModel authenticateUserViewModel)
        {
            var user = await UserRepository.GetUserByAuthenticationDataAsync(authenticateUserViewModel);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUserViewModel authenticateUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetIdentityAsync(authenticateUserViewModel);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthorizationOptions.ISSUER,
                    audience: AuthorizationOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenData
            {
                Token = encodedJwt,
                Username = identity.Name
            };

            MailLogger.SendAuthorizationLog(identity.Name, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            return Ok(response);
        }
    }
}
