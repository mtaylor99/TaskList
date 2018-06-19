using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TaskList.BL.Domain;
using TaskList.DAL.Interfaces;
using TaskList.DAL.Models;

namespace TaskList.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private ILogger<TokenController> logger;
        private UserManager<AspNetUser> userManager;
        private IIdentityRepository identityRepository;   
        Identity identity;

        public TokenController(IConfiguration config, ILogger<TokenController> log, UserManager<AspNetUser> userMgr, IIdentityRepository identityRepo)
        {
            _config = config;
            logger = log;
            userManager = userMgr;
            identityRepository = identityRepo;
            identity = new Identity(logger, identityRepository);

            logger.LogInformation("Api - Token Controller");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(UserModel user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            //Get Roles
            var userRoles = identityRepository.GetUserRoles(user.Id);
            foreach(var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(LoginModel loginModel)
        {
            UserModel user = null;

            if (ModelState.IsValid)
            {
                AspNetUser aspNetUser = userManager.FindByNameAsync(loginModel.Username).Result;

                if (aspNetUser != null)
                {
                    bool result = userManager.CheckPasswordAsync(aspNetUser, loginModel.Password).Result;

                    if (result)
                    {
                        user = new UserModel { Id = aspNetUser.Id, Name = aspNetUser.UserName, Email = aspNetUser.Email };
                    }
                }
            }

            return user;
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class UserModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}
