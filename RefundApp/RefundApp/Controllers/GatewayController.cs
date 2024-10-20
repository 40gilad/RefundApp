using Microsoft.AspNetCore.Mvc;
using RefundApp.Models;
using RefundApp.PsudoServices;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;

namespace RefundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> logger;

        public GatewayController(ILogger<GatewayController> _logger)
        {
            logger = _logger;
        }

        [HttpPost("ProcessRequest")]
        public IActionResult ProcessRequest(string route, [FromBody] Dictionary<string, string> data)
        {
            var sessionId = HttpContext.Session.GetString("SessionId");
            string lower_route = route.ToLowerInvariant();

            if (lower_route == "register")
            {
                UserModel userModel = new UserModel(data);
                if (userModel == null)
                {
                    return BadRequest("Invalid user data.");
                }

                return RedirectToAction("Register", "Login", userModel);
            }

            //if (string.IsNullOrEmpty(sessionId) || !IsValidSession(sessionId))
            //    return Unauthorized("Invalid or expired session.");

            switch (lower_route)
            {
                case "login":
                    return RedirectToAction("Post", "Login", data);
                case "addrefund":
                    return RedirectToAction("Post", "Refund", data);
                case "getrefund":
                    return RedirectToAction("Get", "Refund");

                default:
                    return BadRequest("Invalid route.");
            }
        }

        private bool IsValidSession(string sessionId)
        {
            // Validate the session, e.g., check in a session store or database
            return PsudoUserDbService.Instance().Get().Values.Any(user => user.SessionId == sessionId);
        }
    }
}
