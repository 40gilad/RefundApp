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
        private readonly RefundRepository repository;


        public RefundController(ILogger<RefundController> _logger)
        {
            logger = _logger;
            repository = new RefundRepository();
        }

        [HttpGet("{Qstring}")]
        public ActionResult<List<RefundModel>> Get()
        {
            return Ok(repository.GetAll());
        }

        [HttpPost]
        public ActionResult<List<RefundModel>> Post([FromBody] RefundModel refund)
        {
            if (refund == null)
            {
                return BadRequest("Refund data is null.");
            }

            repository.Add(refund);
            return CreatedAtAction(nameof(Get), new { id = refund.Id }, refund);
        }

    }
}
