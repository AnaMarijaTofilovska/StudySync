using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudySync.DTOs;
using StudySync.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudySync.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // We need to inject dependencys 
        private readonly UserManager<ApplicationUser> _userManager; //to query anything about user (what we previosly did in service and repo)
        private readonly IConfiguration _configuration; // allows to access anything we registerd in appsettings json
        private readonly RoleManager<IdentityRole> _roleManager;    

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        //First endpoint/method for registering :
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDTO model)
        {
            //1. Chec if user exsists
            var userExists= await _userManager.FindByEmailAsync(model.Email);
            if(userExists!=null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exsists!" });
            }

            //2.Prep data: Map the DTO  to ApplicatiobnUser
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email
            };

            //3. Create instance of the user in the DB 
            var result= await _userManager.CreateAsync(user,model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed.Please " });
            }

            // OPTIONAL:Ensure the "Admin" role exists
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            // Assign the "Admin" role to the user
            await _userManager.AddToRoleAsync(user, "Admin");


            return Ok(new { Status = "Success", Message = "User created succesfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            //1. Check if user exsists
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            //2.Generate list of claims for he user , his username and unique Identifier(JTI(
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //4.Create a JQT singing key from the key in our appsettings 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //5. Contruct the JWT .exp time,claims and the signing key 
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: claims,
                signingCredentials: creds
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost("role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            //1. Check if role exsists 
            var roleExsists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExsists)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    return Ok(new { Status = "Success", Message = "Role created successfully." });
                }
                else
                {
                    return BadRequest(new { Status = "Error", Message = "Role creation failed." });
                }
            }
            else
            {
                return BadRequest(new { Status = "Error", Message = "Role already exsists." });
            }
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] UserRoleAssignmentDTO model)
        {
            // 1. Validate input
            if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
            {
                return BadRequest(new { Status = "Error", Message = "UserId and RoleName are required." });
            }

            // 2. Check if user exists
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound(new { Status = "Error", Message = "User not found." });
            }

            // 3. Check if role exists
            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return BadRequest(new { Status = "Error", Message = "Role does not exist." });
            }

            // 4. Assign role to user
            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok(new { Status = "Success", Message = $"Role '{model.RoleName}' assigned to user." });
            }

            return BadRequest(new { Status = "Error", Message = "Failed to assign role." });
        }




    }
}
