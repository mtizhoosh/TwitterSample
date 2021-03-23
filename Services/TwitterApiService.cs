using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Twitter_Statistics.Extensions;
using Twitter_Statistics.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;

namespace Twitter_Statistics.Services
{

    public class TwitterApiService : ITwitterService
    {
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private string TweetsBaseUrl { get; set; }
        private string BearToken { get; set; }
        private readonly ILogger<TwitterApiService> _logger;

        private static bool TestSwitch = false;
        private static bool IsTwitterOnLine = false;


        public TwitterApiService(IConfiguration settings, HttpClient httpClient, ILogger<TwitterApiService> logger, IWebHostEnvironment env, IOptions<AppSettings> options)
        {
            _env = env;
            _client = httpClient;
            _config = settings;
            _logger = logger;

           // string fileName = options.Value.FileName;


            TweetsBaseUrl = _config.GetValue<string>("TwitterConfig:BaseAddress");
            BearToken = _config.GetValue<string>("TwitterConfig:BearerToken");
            httpClient.BaseAddress = new Uri(TweetsBaseUrl);
        }

        public async Task<IEnumerable<TwitInfo>> GetDefaultAll()
        {
            List<TwitInfo> result = null;
            var response = await _client.GetAsync(this.TweetsBaseUrl);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<List<TwitInfo>>();
            }
            else
            {
                _logger.LogError($"Failed to return the results.{response.ReasonPhrase}");

            }


            return result;
        }

        public async Task<IEnumerable<TwitInfo>> GetSampleTwitts()
        {
            List<TwitInfo> result;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.BearToken);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.GetAsync(this.TweetsBaseUrl);
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<List<TwitInfo>>();
            else
                throw new HttpRequestException(response.ReasonPhrase);

            return result;
        }

        public async Task<SampledStreamModel> GetById(int id)
        {
            var result = new SampledStreamModel();
            var response = await _client.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<SampledStreamModel>();

            return result;
        }

        public async Task<TwittStatistics> GetStatistics()
        {
           // Get twitter sample stream                         
            IEnumerable<TwitInfo> allTwitts = await this.GetSampleTwitts();
            TwitInfo[] feeds = allTwitts.ToArray();
            
            var pl = from r in feeds
                     orderby r
                     group r by r into grp
                     select new { key = grp.Key, cnt = grp.Count() };

            double ave = pl.Average(r => r.cnt);

            var result = new TwittStatistics
            {
                twitts = feeds,
                total = feeds.Count(),
                averagePerMinute = ave
            };
            
            return result;
        }

        public TwittStatistics GetTwittStatistics()
        {
            TwittStatistics result = null;
            try
            {
                if (!IsTwitterOnLine) result = this.GetMock();
                else
                {
                    result = this.GetStatistics().Result;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to read test data.{ex.Message}");
            }


            return result;
        }
       
        public TwittStatistics GetMock()
        {
            TwittStatistics result = null;
            try
            {
               
                Task<List<TwitInfo>> MockTwittes = this.CreateMockTwitts();

                if (MockTwittes != null)
                {
                    result = new TwittStatistics();
                    result.twitts = (new List<TwitInfo>(MockTwittes.Result)).ToArray();

                    List<int> allMinutes = new List<int>();
                    foreach (var item in MockTwittes.Result)
                    {
                        DateTime aDate = Convert.ToDateTime(item.created_at);

                        allMinutes.Add(aDate.Minute);
                    }

                    var pl = from r in allMinutes
                             orderby r
                             group r by r into grp
                             select new { key = grp.Key, cnt = grp.Count() };

                    double ave = pl.Average(r => r.cnt);


                    result.total = MockTwittes.Result.Count();
                    result.averagePerMinute = ave;
                }                
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to read test data.{e.Message}");
            }
            

            return result;
        }

        /// <summary>
        /// Mock test twitts taken from file or static data
        /// </summary>
        /// <returns></returns>
        public async Task<List<TwitInfo>> CreateMockTwitts()
        {
            List<TwitInfo> MockTwittes = null;
            try
            {
                string mockDatafileName = this._env.ContentRootPath + "\\" + _config.GetValue<string>("MockData:FileName");

                await Task.Run(() =>
                {
                    string twitts = string.Empty;
                    if (TwitterApiService.TestSwitch)
                    {
                        twitts = File.ReadAllText(mockDatafileName);
                    }
                    else
                    {
                        twitts = TwitterApiService.TestTwitts;
                    }

                    if (twitts != string.Empty) TwitterApiService.TestSwitch = !TwitterApiService.TestSwitch;

                    MockTwittes = JsonSerializer.Deserialize<List<TwitInfo>>(twitts);
                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Test file not found.{ex.Message}");
            }
           

            return MockTwittes;
        }
        public IEnumerable<string> ReadTestData(string fileName)
        {
            IEnumerable<string> lines = null;
            try
            {
                lines = File.ReadLines(fileName);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Test file not found.{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to read test data.{ex.Message}");
            }


            return lines;
        }


        public static string TestTwitts = @"
        [
        
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:33:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:49.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""},
                {""text"":""@cnslncn @iirmksyy @EftelyaDuygu Gt"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""1318945346928652290"",""id"":""1367121036580388864""},
                {""text"":""Domingo se pá tem toque toque 🚀"",""created_at"":""2021-03-03T14:34:50.000Z"",""author_id"":""2694014607"",""id"":""1367121036601360389""}
        ]";
    }
    
}
