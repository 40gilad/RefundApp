using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RefundApp.Models;
using RefundApp.PsudoServices;
using System.Net.Http.Headers;

namespace RefundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> logger;
        private readonly HttpClient httpClient;

        private static Dictionary<string, string> serviceEndpoints = new()
        {
            { "register", "https://localhost:7017/Login/Register" },
            { "login", "https://localhost:7017/Login" }
        };

        private static Dictionary<string, string> sessionServiceEndpoints = new()
        {
            { "refund", "https://localhost:7017/Refund" },
            { "a", "https://localhost:7017/Login" },
            { "b", "https://localhost:7017/Login" }
        };


        public GatewayController(ILogger<GatewayController> _logger, HttpClient _httpClient)
        {
            logger = _logger;
            httpClient = _httpClient;
        }

        [HttpPost("ProcessRequest")]
        public async Task<IActionResult> ProcessRequest(string route, [FromBody] UserModel user)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            logger.LogInformation("Processing request for route: {Route}", route);

            if (user == null)
            {
                logger.LogWarning("Invalid user data.");
                return BadRequest("Invalid user data.");
            }

            string lower_route = route.ToLowerInvariant();

            if (!serviceEndpoints.ContainsKey(route) && !sessionServiceEndpoints.ContainsKey(route))
                return BadRequest("Invalid service.");

            try
            {
                var response = await httpClient.PostAsJsonAsync(serviceEndpoints[route], user);

                if (response.IsSuccessStatusCode)
                    return Ok(await response.Content.ReadAsStringAsync());

                logger.LogWarning("Error calling service for route: {Route}, StatusCode: {StatusCode}", lower_route, response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Error calling service for route: {Route}", lower_route);
                return StatusCode(500, ex.Message);
            }


        }

        private bool IsValidSession(string sessionId)
        {
            // Validate the session, e.g., check in a session store or database
            return PsudoUserDbService.Instance().Get().Values.Any(user => user.SessionId == sessionId);
        }
    }
}
