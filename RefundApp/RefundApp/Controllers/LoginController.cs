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

            if (user == null)
                return BadRequest("User data is null.");
            try
            {
                user.UPassword = PasswordHasher.HashPassword(user.UPassword);
                //await PsudoUserDbService.Instance().Add(user);
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
            if (user == null)
                return BadRequest("User data is null.");

            try
            {
                if (!await loginService.UserAuth(user))
                    return BadRequest("Invalid password.");
                string Token = JwtUtils.GenerateJwtToken(user.UEmail, secretKey);
                return Ok(new { Message = "Login successful.", Token = Token });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest("user data is null.");
            UserModel u;
            try
            {
                u = PsudoUserDbService.Instance().Get(user.UEmail);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            if (u.UPassword != user.UPassword)
                return BadRequest();
            PsudoUserDbService.Instance().Update(user);
            return Ok();
        }


    }
}
