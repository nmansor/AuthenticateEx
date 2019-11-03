using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [EnableCors("CorsPolicy")]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> UserManager { get; }
        private SignInManager<ApplicationUser> SignInManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await UserManager.FindByNameAsync(model.Email);
                    if (user == null)
                    {
                        user = new ApplicationUser();
                        user.Email = model.Email;
                        user.FirstName = model.FirstName;
                        user.FamilyName = model.FamilyName;
                        user.UserName = model.Email;
                        IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return Ok();
                        }
                    }
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await UserManager.FindByNameAsync(model.UserName);
                    if (user != null)
                    {
                       SignInResult result = await SignInManager.CheckPasswordSignInAsync(user, model.Password, false);
                        if (result.Succeeded)  // create the token
                        {
                            // create key
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTTokenParams.Key));
                            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                            var claims = new[]
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, model.UserName),  
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName),
                            };

                            // create a token for this subject with claims and ..
                            var token = new JwtSecurityToken(
                                JWTTokenParams.Issuer,
                                JWTTokenParams.Audience,
                                claims,
                                expires: DateTime.UtcNow.AddMinutes(30),
                                signingCredentials: creds
                                );
                            //  return her is your token and here when it expires
                            var results = new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = token.ValidTo
                            };
                            return Created("", result);
                        }

                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}