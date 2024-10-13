using Microsoft.AspNetCore.Mvc;
using RefundApp.Models;
using RefundApp.PsudoServices;

namespace RefundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RefundController : ControllerBase
    {
        private readonly ILogger<RefundController> logger;

        public RefundController(ILogger<RefundController> _logger)
        {
            logger = _logger;
        }

        [HttpGet]
        public ActionResult<List<RefundModel>> Get()
        {
            try
            {
                var refund = PsudoRefundDbService.Instance().Get();
                return Ok(refund);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{OrderId}")]
        public ActionResult<RefundModel> Get(string OrderId)
        {
            try
            {
                var refund = PsudoRefundDbService.Instance().Get(OrderId);
                return Ok(refund);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] RefundModel refund)
        {
            
            if (refund == null)
                return BadRequest("Refund data is null.");
            PsudoRefundDbService.Instance().Add(refund);
            logger.LogInformation($"added new refund:\n{refund.ToString}\n");
            return Ok($"Refund {refund.OrderId} added successfully.");
        }

        [HttpDelete("{OrderId}")]
        public IActionResult Delete(string OrderId)
        {
            try
            {
                PsudoRefundDbService.Instance().Remove(OrderId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
