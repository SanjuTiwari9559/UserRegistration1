using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserRegistration1.Authentication;

namespace UserRegistration1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AuthenticationController(UserManager<AplicationUser> userManager,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost]
        [Route("Register")]

        public async Task<IActionResult> register([FromBody]RegisterModel user)
        {
            var userExist= await userManager. FindByNameAsync(user.UserName);
            if(userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new Responce { status="Error",message="User Is Alredy Created"});
            }
            AplicationUser aplicationUser = new AplicationUser
            {
                Email = user.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = user.UserName,
            };
            var result=await userManager.CreateAsync(aplicationUser,user.Password);
            if(!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Responce { status = "Error", message = "User Not created" });
            }
            return Ok(new Responce { message = "User Successfull Created" });
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> login([FromBody] LoginModel loginModel)
        {
            var userExist = await userManager.FindByNameAsync(loginModel.userName);
            if(userExist != null &&  await userManager.CheckPasswordAsync(userExist,loginModel.password)) {

                var userRoles = await userManager.GetUsersInRoleAsync("userExist");
                var authClamis = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginModel.userName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

                };
                foreach(var userRole in userRoles)
                {
                    authClamis.Add(new Claim(ClaimTypes.Role,"userRole"));
                }
                var authsignKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:Valid Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClamis,
                    signingCredentials: new SigningCredentials(authsignKey,SecurityAlgorithms.HmacSha256));
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = userExist.UserName
                });
            }
            return Unauthorized();
        }
    }
}
