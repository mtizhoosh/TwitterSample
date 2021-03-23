using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Twitter_Statistics.Models;
using Twitter_Statistics.Services;

namespace Twitter_Statistics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TwitterFeedController : ControllerBase
    {
        private ITwitterService service;
        private readonly ILogger<TwitterFeedController> _logger;
        private readonly IConfiguration _twitterSettings;


        public TwitterFeedController(IConfiguration twitterSettings, ILogger<TwitterFeedController> logger, ITwitterService service)
        {
            this._twitterSettings = twitterSettings;
            this._logger = logger;
            this.service = service;
        }


        [HttpGet]
        public IActionResult Get()
        {           
            TwittStatistics MockTwittes = service.GetTwittStatistics();

            if (MockTwittes == null)  return new NoContentResult();

            return new JsonResult(MockTwittes);
        }

     
    }
}

    
   
