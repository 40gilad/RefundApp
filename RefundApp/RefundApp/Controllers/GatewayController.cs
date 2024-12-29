using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


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
            { "jsons-compare", "https://localhost:5000/compare" },
            { "pipi", "https://localhost:7017/pipi" }
        };

        public GatewayController(ILogger<GatewayController> _logger, IHttpClientFactory _httpClientFactory, IConfiguration configuration)
        {
            logger = _logger;
            httpClientFactory = _httpClientFactory;
            secretKey = configuration["Jwt:SecretKey"]; // Get the secret key from configuration
        }

        [HttpPost("ProcessRequest")]
        public async Task<IActionResult> ProcessRequest(
            string route, 
            [FromBody] object? payload,
            [FromHeader(Name = "Authorization")] string? token)
        {
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

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(
            [FromForm] string uEmail,
            [FromForm] IFormFile file,
            [FromHeader(Name = "Authorization")] string? token)
        {
            // refactor method for production!
            if (file == null)
                return BadRequest("Refund data is null.");

            string route = "json-compare";
            logger.LogInformation("Processing request for route: {Route}", route);

            var serviceEndpoint = "https://localhost:5000/compare"; // The Flask endpoint URL
            var refundServiceEndpoint = "https://localhost:7017/refund/RefundsByMail"; // The Refund API endpoint

            try
            {
                // Create an HttpClientHandler to disable SSL certificate verification
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                // Create the HttpClient with the custom handler
                var httpClient = httpClientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Now we use the custom handler in the httpClient
                var client = new HttpClient(handler);

                // Execute GET request for GetRefundsByMail method and get the result
                var getRefundResponse = await client.GetAsync($"{refundServiceEndpoint}?UMail={uEmail}");

                if (!getRefundResponse.IsSuccessStatusCode)
                {
                    logger.LogWarning("Error calling Refund service for route: {Route}, StatusCode: {StatusCode}", route, getRefundResponse.StatusCode);
                    return StatusCode((int)getRefundResponse.StatusCode, await getRefundResponse.Content.ReadAsStringAsync());
                }

                // Retrieve the refund data (assuming it's JSON)
                var refundData = await getRefundResponse.Content.ReadAsStringAsync();

                logger.LogInformation("Successfully fetched refund data for email: {UMail}", uEmail);

                // Prepare the content to forward (the file and the refund data)
                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileContent, "file", file.FileName);

                    // Add the refund data to the content
                    content.Add(new StringContent(refundData), "restaurant_data");

                    // Include the JWT token if necessary
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }

                    // Execute POST request to the Flask endpoint with the file and refund data
                    var response = await client.PostAsync(serviceEndpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(await response.Content.ReadAsStringAsync());
                    }

                    logger.LogWarning("Error calling Flask service for route: {Route}, StatusCode: {StatusCode}", route, response.StatusCode);
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Error calling service for route: {Route}", route);
                return StatusCode(500, ex.Message);
            }
        }



    }
}



// path to example pdf files: 
// C:\Users\40gil\Desktop\not_work\my_scipts\RefundApp\RefundApp\DataProcessAndPythonController\DataProcessors\PdfProcessor\example pdfs