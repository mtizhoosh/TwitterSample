using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Twitter_Statistics.Extensions;
using Twitter_Statistics.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;

namespace Twitter_Statistics.Services
{

    public class TwitterApiService : BaseApiService, ITwiterService
    {
        
        private TwitterConfig _configuartion { get; set; }

        public TwitterApiService(IWebHostEnvironment env, IOptions<AppSettings> options, ILogger<TwitterApiService> logger, HttpClient httpClient) : base(env, options, logger, httpClient)
        {
        }

        public TwittStatistics GetTwittStatistics()
        {
            TwittStatistics result = null;
            try
            {
                result  = this.GetStatistics();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to read test data.{ex.Message}");
            }

            return result;
        }

     
        private TwittStatistics GetStatistics()
        {
            TwittStatistics result = null;
            try
            {

                Task<List<TwitInfo>> twittes = this.TwitterRepository.GetSampleTwitts();

                if (twittes != null)
                {
                    result = new TwittStatistics();
                    result.Twitts = (new List<TwitInfo>(twittes.Result));

                    if (result.Twitts.Any())
                    {
                        List<int> allMinutes = result.Twitts.Select(item => Convert.ToDateTime(item.created_at).Minute).ToList();

                        var pl = from r in allMinutes
                                 orderby r
                                 group r by r into grp
                                 select new { key = grp.Key, cnt = grp.Count() };

                        double ave = pl.Average(r => r.cnt);

                        result.Total = twittes.Result.Count();
                        result.AveragePerMinute = ave;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to read test data.{e.Message}");
            }


            return result;
        }
    }
    
}
