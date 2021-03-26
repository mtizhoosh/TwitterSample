using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Twitter_Statistics.Models;
using Twitter_Statistics.Services;

namespace Twitter_Statistics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TwitterFeedController : ControllerBase
    {
        private readonly ITwiterService _service;
        private readonly ILogger<TwitterFeedController> _logger;

        public TwitterFeedController(ILogger<TwitterFeedController> logger, ITwiterService service)
        {
            this._logger = logger;
            this._service = service;
        }


        /// <summary>
        /// Return list of twitts and calculated statistics
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //public ActionResult<TwittStatistics> GetTwittStatistics()
        public ActionResult<TwittStatistics> Get()
        {
            try
            {
                TwittStatistics MockTwittes = this._service.GetTwittStatistics();

                if (MockTwittes == null) return new NoContentResult();

                return Ok(MockTwittes);
            }
            catch (Exception ex)
            {
                string error = "Failed to get twitter sample feed.";
                this._logger.LogError(ex, error);
                return BadRequest(error);
            }
           
        }     
    }
}

    
   
