using Microsoft.AspNetCore.Mvc;
using RefundApp.Models;
using RefundApp.PsudoServices;
using RefundApp.Services;

namespace RefundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RefundController : ControllerBase
    {
        private readonly ILogger<RefundController> logger;
        private readonly RefundService refundService;

        public RefundController(ILogger<RefundController> _logger, RefundService _refundService)
        {
            logger = _logger;
            refundService = _refundService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RefundModel refund)
        {
            try
            {
                await refundService.Add(refund);
                return Ok($"Refund {refund.Id} added successfully with id {refund.Id}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
            {
                var l = await refundService.Get();
                if (l == null || l.Count == 0)
                    return NotFound("No Refunds Found");
                return Ok(l);
            }
            else
            {
                var u = await refundService.Get(id.Value);
                if (u is null)
                    return NotFound($"Refund {id} Not Found");
                return Ok(u);
            }
        }


        [HttpDelete("DeleteById")]
        public IActionResult Delete(string UMail, string OrderId)
        {
            try
            {
                PsudoRefundDbService.Instance().Remove(UMail,OrderId);
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
