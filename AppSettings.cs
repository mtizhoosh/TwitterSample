using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter_Statistics.Services;

namespace Twitter_Statistics
{
  
    public class AppSettings
    {
        public const string APP_CONFIG = "AppConfig";

        public string    TwitDataProvider    { get; set; }
        public MockDataConfig  MockDataSettings    { get; set; } 
        public TwitterConfig   TwitterSettings     { get; set; }
               
    }

    public class MockDataConfig
    {
        public string FileName { get; set; }
    }

    
    public class TwitterConfig
    {
        public string BaseAddress { get; set; }
        public string APIKey { get; set; }
        public string APISecretKey { get; set; }
        public string BearerToken { get; set; }

    }
}
