using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Twitter_Statistics.Extensions;
using Twitter_Statistics.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Twitter_Statistics.Repository
{
    
    public class TwitterApiProvider : ITwitProvider
    {
        protected readonly HttpClient _httpClient;
        private TwitterConfig _configuartion { get; set; }

        public TwitterApiProvider(ILogger logger, HttpClient httpClient, TwitterConfig configs)
        {
            _httpClient = httpClient;
            _configuartion = configs;
        }

        public async Task<List<TwitInfo>> GetSampleTwitts()
        {
            List<TwitInfo> result;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this._configuartion.BearerToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.GetAsync(this._configuartion.BaseAddress);
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<List<TwitInfo>>();
            else
                throw new HttpRequestException(response.ReasonPhrase);

            return result;
        }
               
    }
    
}
