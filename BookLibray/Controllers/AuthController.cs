using AutoMapper;
using BookLibrary.Data;
using BookLibrary.Models.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(ApplicationDbContext context, ILogger<AuthController> logger, UserManager<IdentityUser> userManager, IConfiguration
            configuration, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(AuthUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Insufficent data provided");
            }

            _logger.LogInformation($"registration attempt for {userDto.Email} ");


            try
            {
                var user = _mapper.Map<IdentityUser>(userDto);
                user.UserName = userDto.Email;
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                await _userManager.AddToRoleAsync(user, "User");
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError($"something went wrong in the {nameof(Register)}", ex);
                return Problem($"something went wrong ", statusCode: 500);
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(AuthLoginUserDto userDto)
        {
            _logger.LogInformation($"login attempt for {userDto.Email} ");
            
            if (userDto == null)
            {
                return BadRequest();
            }
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);

                if (user == null || passwordValid == false)
                {
                    return Unauthorized(userDto);
                }

                string tokenString = await GenerateToken(user);

                var response = new AuthResponseDto()
                {
                    Email = userDto.Email,
                    UserId = user.Id,
                    Token = tokenString
                };

                return Accepted(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"something went wrong in the {nameof(Login)}", ex);
                return Problem($"something went wrong ", statusCode: 500);
            }
        }
        /// <summary>
        /// Generates a new JWT 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>new token</returns>
        private async Task<string> GenerateToken(IdentityUser user)
        {
            // creating a security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // getting roles
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(user);

            // getting  claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)
            }.Union(roleClaims)
            .Union(userClaims);

            var token = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
