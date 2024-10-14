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

        public LoginController(ILogger<LoginController> _logger)
        {
            logger = _logger;
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
        public IActionResult Post([FromBody] UserModel user)
        {
            if (user == null)
                return BadRequest("User data is null.");

            try
            {
                if(!UserAuth(user))
                    return BadRequest("Invalid password.");
                return Ok("Login successful.");

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

        [HttpDelete]
        public IActionResult Delete([FromBody] UserModel user)
        {
            try
            {
                if (!UserAuth(user))
                    return BadRequest("Invalid password.");
                string uname=user.UName;
                PsudoUserDbService.Instance().Remove(user.UEmail);
                return Ok($"user {uname} removed successfuly");
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        private bool UserAuth(UserModel user)
        {
            var storedUser = PsudoUserDbService.Instance().Get(user.UEmail);
            var hashedPassword = PasswordHasher.HashPassword(user.UPassword);

            if (!PasswordHasher.VerifyPassword(storedUser.UPassword, user.UPassword))
                return false;

            return true;
        }

    }
}
