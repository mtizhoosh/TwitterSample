using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Twitter_Statistics.Repository;

namespace Twitter_Statistics.Services
{

    public class BaseApiService 
    {
        protected readonly IWebHostEnvironment _appEnv;
        private readonly HttpClient _httpClient;

        protected ITwitProvider TwitterRepository { get; set; } = null;
        protected ILogger<TwitterApiService>    Logger { get; set; }
        protected AppSettings                   ApplicationSettings { get; set; }

      
        public BaseApiService(IWebHostEnvironment env, IOptions<AppSettings> options, ILogger<TwitterApiService> logger, HttpClient httpClient)
        {
            _appEnv = env;
            _httpClient = httpClient;
            Logger = logger;
            ApplicationSettings = options.Value;
            this.GetSettings();
        }
        
        
        private void GetSettings()
        {
            TwitProvider provider;
            if (Enum.TryParse<TwitProvider>(ApplicationSettings?.TwitDataProvider, out provider)) 
            {
                switch (provider)
                {
                    case TwitProvider.File:
                        MockDataConfig fileSettings = this.GetSection<MockDataConfig>();
                        string dataFileName = $"{this._appEnv.ContentRootPath}\\{fileSettings.FileName}";
                        this.TwitterRepository = new TwitterFileProvider(this.Logger, dataFileName) as ITwitProvider;
                        break;
                    case TwitProvider.TwitterAPI:
                        TwitterConfig twitSettings = this.GetSection<TwitterConfig>();
                        this.TwitterRepository = new TwitterApiProvider(this.Logger, this._httpClient, twitSettings) as ITwitProvider;
                        break;
                    default:
                        break;
                }
            }           
        }

        private dynamic GetSection<T>()
        {
            if (typeof(T) == typeof(MockDataConfig))
            {
                return ApplicationSettings?.MockDataSettings;
            }
            else if (typeof(T) == typeof(TwitterConfig))
            {
                return ApplicationSettings?.TwitterSettings;
            }            
            else
            {
                return null;
            }
        }
    }
    
}
