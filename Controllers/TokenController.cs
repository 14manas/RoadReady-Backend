using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoadReady.DTO;
using RoadReady.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoadReady.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly RoadReadyContext _context;
        private readonly ILogger<TokenController> _logger;

        public TokenController(IConfiguration configuration, RoadReadyContext context, IMapper mapper, ILogger<TokenController> logger)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet("isauthenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = HttpContext.User;
                var userClaimId = user.Claims.FirstOrDefault(c => c.Type == "UserId");
                var userId = int.Parse(userClaimId.Value);
                //var userObj =await _context.Users.FindAsync(userId);
                var userObj = _context.Users.FirstOrDefault(u => u.UserId == userId);


                var userRole = await GetUserRole(userObj.Usertypeid);
               






                return Ok(new { success = true, userData = userObj });
            }
            return Ok(new { success = false, userData = "" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post(LoginDTO _userData)
        {
            try
            {
                if (_userData != null && _userData.Username != null && _userData.Password != null)
                {
                    var user = await GetUser(_userData.Username, _userData.Password);

                    if (user != null)
                    {
                        var userRole = await GetUserRole(user.Usertypeid);
                        //create claims details based on the user information
                        var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("UserId", user.UserId.ToString()),
                            new Claim("UserName", user.Username),
                            new Claim("Email", user.Email),
                            new Claim(ClaimTypes.Role,userRole)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn);

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), success = "True", userData = user });
                    }
                    else
                    {
                        return BadRequest("Invalid credentials");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while generating token: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private async Task<UserDTO> GetUser(string username, string password)
        {
            try
            {

                var obj = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
                return _mapper.Map<UserDTO>(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user: {ex.Message}");
                return null;
            }
        }

        private async Task<string> GetUserRole(int? UsertypeId)
        {
            try
            {
                return await _context.Usertypes.Where(x => x.Usertypeid == UsertypeId).Select(x => x.Usertypename).FirstAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user role: {ex.Message}");
                return null;
            }
        }
    }
}
