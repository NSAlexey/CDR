using CDR.Services;
using Microsoft.AspNetCore.Mvc;

namespace CDR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallController : ControllerBase
    {
        private readonly ICallService _callService;

        private readonly ILogger<CallController> _logger;

        public CallController(ILogger<CallController> logger, ICallService callService)
        {
            _logger = logger;
            _callService = callService;
        }

        /// <summary>
        /// Files Upload
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostFiles([FromForm] IFormFileCollection files)
        {
            try
            {
                var uploadedCount = 0;
                foreach (var file in files)
                {
                    uploadedCount += await _callService.AddDetailsFromFile(file.OpenReadStream());
                }
                return Ok(uploadedCount);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
