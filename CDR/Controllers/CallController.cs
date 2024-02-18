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
        /// <param name="files">csv file to upload</param>
        /// <returns>counts of uploaded recoreds</returns>
        [HttpPost]
        [Route("files")]
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

        /// <summary>
        /// Caller's statistics
        /// </summary>
        /// <param name="startDate">Start period</param>
        /// <param name="endDate">End period</param>
        /// <returns>Journal of callings</returns>
        [HttpGet]
        [Route("callers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CallStatisticsByCaller(DateOnly startDate, DateOnly endDate)
        {
            if (endDate == DateOnly.MinValue)
            {
                endDate = DateOnly.FromDateTime(DateTime.Today);
            }
            var result = await _callService.CallStatisticsByCaller(startDate, endDate);

            return Ok(result);
        }

        /// <summary>
        /// Caller's journal
        /// </summary>
        /// <param name="callerId">Caller number</param>
        /// <param name="startDate">Start period</param>
        /// <param name="endDate">End period</param>
        /// <returns>Journal of callings</returns>
        [HttpGet]
        [Route("callers/{callerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CallListByCallerAndPeriod(long callerId, DateOnly startDate, DateOnly endDate)
        {
            if(endDate == DateOnly.MinValue) 
            { 
                endDate = DateOnly.FromDateTime(DateTime.Today); 
            }
            var result = await _callService.CallListByCallerAndPeriod(callerId, startDate, endDate);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Longest Call
        /// </summary>
        /// <param name="startDate">Start period</param>
        /// <param name="endDate">End period</param>
        /// <returns>Longest Call on period</returns>
        [HttpGet]
        [Route("callers/longest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> LongestCallByPeriod(DateOnly startDate, DateOnly endDate)
        {
            if (endDate == DateOnly.MinValue)
            {
                endDate = DateOnly.FromDateTime(DateTime.Today);
            }
            var result = await _callService.LongestCallByPeriod(startDate, endDate);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Cost of calls
        /// </summary>
        /// <param name="callerId">Caller number</param>
        /// <param name="startDate">Start period</param>
        /// <param name="endDate">End period</param>
        /// <returns>Cost of calls on period</returns>
        [HttpGet]
        [Route("callers/{callerId}/cost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CostByCallerOnPeriod(long callerId, DateOnly startDate, DateOnly endDate)
        {
            if (endDate == DateOnly.MinValue)
            {
                endDate = DateOnly.FromDateTime(DateTime.Today);
            }
            var result = await _callService.CostByCallerOnPeriod(callerId, startDate, endDate);
            
            return Ok(result);
        }

        /// <summary>
        /// Average duration
        /// </summary>
        /// <param name="callerId">Caller number</param>
        /// <param name="startDate">Start period</param>
        /// <param name="endDate">End period</param>
        /// <returns>Average duration by caller on period</returns>
        [HttpGet]
        [Route("callers/{callerId}/avg_duration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AverageDurationByCallerOnPeriod(long callerId, DateOnly startDate, DateOnly endDate)
        {
            if (endDate == DateOnly.MinValue)
            {
                endDate = DateOnly.FromDateTime(DateTime.Today);
            }
            var result = await _callService.AverageDurationByCallerOnPeriod(callerId, startDate, endDate);

            return Ok(result);
        }
    }
}
