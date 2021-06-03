using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewDayChallenge.Domain;
using NewDayChallenge.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewDayChallenge.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ILogger<CandidatesController> _logger;
        private readonly ICandidateService _candidateService;

        public CandidatesController(ILogger<CandidatesController> logger, ICandidateService candidateService) =>
            (_logger, _candidateService) = (logger, candidateService);

        // GET: api/<CandidatesController>
        [HttpGet]
        [Route("search")]
        [Produces("application/json")]
        public IActionResult Get([FromQuery] string skills)
        {
            if (skills == null || skills.Length == 0)
            {
                _logger.LogError("Request missing skills");
                return BadRequest();
            }

            var skillsArray = skills.Split(","); 

            var result = _candidateService.Search(skillsArray);

            return result != null ? new JsonResult(result) : NotFound();
        }

        // POST api/<CandidatesController>
        [HttpPost]
        public IActionResult Post([FromBody] Candidate candidate)
        {
            if (candidate == null)
            {
                _logger.LogError("Post missing body");
                return BadRequest();
            }

            _candidateService.Add(candidate);

            return Ok();
        }
    }
}
