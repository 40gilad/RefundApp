using Microsoft.AspNetCore.Mvc;
using RefundApp.Models;
using RefundApp.Repositories;

namespace RefundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RefundController : ControllerBase
    {
        private readonly ILogger<RefundController> logger;
        private readonly PsudoRefundDbService repository;

        public RefundController(ILogger<RefundController> _logger)
        {
            logger = _logger;
            repository = new PsudoRefundDbService();
        }

        [HttpGet]
        public ActionResult<List<RefundModel>> Get()
        {
            try
            {
                var refund = repository.Get();
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
                var refund = repository.Get(OrderId);
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
            {
                logger.LogError("refund is null");
                return BadRequest("Refund data is null.");
            }

            repository.Add(refund);
            logger.LogInformation($"added new refund:\n{refund.ToString}\n");
            return Ok($"Refund {refund.OrderId} added successfully.");
        }

        [HttpDelete("{OrderId}")]
        public IActionResult Delete(string OrderId)
        {
            try
            {
                repository.Remove(OrderId);
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
