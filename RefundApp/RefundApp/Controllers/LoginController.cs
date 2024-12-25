using Microsoft.AspNetCore.Mvc;
using RefundApp.Models;
using RefundApp.PsudoServices;
using RefundApp.Utils;
namespace RefundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private readonly string secretKey; // Add this field

        public LoginController(ILogger<LoginController> _logger, IConfiguration configuration)
        {
            logger = _logger;
            secretKey = configuration["Jwt:SecretKey"];
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register([FromBody] UserModel user)
        {
            //register logic
            
            if (user == null)
                return BadRequest("user data is null.");
            try
            {
                user.UPassword = PasswordHasher.HashPassword(user.UPassword);
                PsudoUserDbService.Instance().Add(user);
                logger.LogInformation($"added new user:\n{user.ToString}\n");
                return Ok($"User {user.UName} added successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest("User data is null.");

            try
            {
                if (!UserAuth(user))
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


        private bool UserAuth(UserModel user)
        {
            var storedUser = PsudoUserDbService.Instance().Get(user.UEmail);
            var hashedPassword = PasswordHasher.HashPassword(user.UPassword);

            return PasswordHasher.VerifyPassword(storedUser.UPassword, user.UPassword);
        }

    }
}
