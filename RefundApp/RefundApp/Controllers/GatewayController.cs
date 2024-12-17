using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; // Ensure this is included
using Microsoft.IdentityModel.Tokens;
using RefundApp.Models;
using RefundApp.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace RefundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string secretKey;

        private static Dictionary<string, string> serviceEndpoints = new()
        {
            { "register", "https://localhost:7017/Login/Register" },
            { "login", "https://localhost:7017/Login" }
        };

        private static Dictionary<string, string> jwtRequiredServiceEndpoints = new()
        {
            { "add-refund", "https://localhost:7017/Refund" },
            { "jsons-compare", "http://localhost:5000/compare" },
            { "pipi", "https://localhost:7017/pipi" }
        };

        public GatewayController(ILogger<GatewayController> _logger, IHttpClientFactory _httpClientFactory, IConfiguration configuration)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory;
            secretKey = configuration["Jwt:SecretKey"]; // Get the secret key from configuration
        }

        [HttpPost("ProcessRequest")]
        public async Task<IActionResult> ProcessRequest(string route, [FromBody] object payload, [FromHeader(Name = "Authorization")] string token)
        {

            /**** test endpoiint ******/

            var kaki_httpClient = httpClientFactory.CreateClient();
            kaki_httpClient.DefaultRequestHeaders.Accept.Clear();
            kaki_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var kaki_response = await kaki_httpClient.PostAsJsonAsync(jwtRequiredServiceEndpoints[route], payload);

            if (kaki_response.IsSuccessStatusCode)
            {
                return Ok(await kaki_response.Content.ReadAsStringAsync());
            }
            logger.LogWarning("Error calling service for route: {Route}, StatusCode: {StatusCode}", route, kaki_response.StatusCode);
            return StatusCode((int)kaki_response.StatusCode, await kaki_response.Content.ReadAsStringAsync());

            /**************************/

            logger.LogInformation("Processing request for route: {Route}", route);
            var serviceEndpoint = string.Empty;
            string lower_route = route.ToLowerInvariant();

            // Validate JWT only for the routes that require it
            if (jwtRequiredServiceEndpoints.ContainsKey(route))
            {
                if (!JwtUtils.ValidateJwtToken(token, secretKey, out string userEmail))
                {
                    logger.LogWarning("Invalid JWT token.");
                    return Unauthorized("Invalid token.");
                }
                serviceEndpoint = jwtRequiredServiceEndpoints[route];
                logger.LogInformation("JWT token validated for user: {UserEmail}", userEmail);
            }
            else if (!serviceEndpoints.ContainsKey(route))
            {
                return BadRequest("Invalid service.");
            }
            else
            {
                serviceEndpoint = serviceEndpoints[route];
            }

            try
            {

                var httpClient = httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsJsonAsync(serviceEndpoint, payload);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(await response.Content.ReadAsStringAsync());
                }

                logger.LogWarning("Error calling service for route: {Route}, StatusCode: {StatusCode}", lower_route, response.StatusCode);
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Error calling service for route: {Route}", lower_route);
                return StatusCode(500, ex.Message);
            }
        }
    }
}


/*
 site: ValueKind = Object : "{"uName":"pipi","uEmail":"","uPassword":"123","sessionId":0}"
 */