using Microsoft.AspNetCore.Mvc;
using RefundApp.Models;
using RefundApp.PsudoServices;
using RefundApp.Services;
using RefundApp.Utils;
namespace RefundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private readonly string secretKey;
        private readonly LoginService loginService;


        public LoginController(ILogger<LoginController> _logger, IConfiguration configuration, LoginService _loginService)
        {
            logger = _logger;
            secretKey = configuration["Jwt:SecretKey"];
            loginService = _loginService;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            try
            {
                user.UPassword = PasswordHasher.HashPassword(user.UPassword);
                await loginService.Add(user);
                logger.LogInformation($"added new user:\n{user.ToString}\n");
                return Ok($"User {user.UName} added successfully with id {user.Id}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            if (!await loginService.UserAuth(user))
                return NotFound($"User {user} Not Found");
            string Token = JwtUtils.GenerateJwtToken(user.UEmail, secretKey);
            return Ok(new { Message = "Login successful.", Token = Token });
        }

        [HttpGet("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                var l = await loginService.Get();
                if (l == null || l.Count == 0)
                    return NotFound("No Users Found");
                return Ok(l);
            }
            else
            {
                var u = await loginService.Get(id.Value);
                if (u is null)
                    return NotFound($"User {id} Not Found");
                return Ok(u);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserModel user)
        {
            if (! await loginService.Update(user))
                return NotFound($"User {user} Not Found");
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await loginService.Delete(id))
                return Ok();
            return BadRequest($"User {id} not found");
        }


    }
}
