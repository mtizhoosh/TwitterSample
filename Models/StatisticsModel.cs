using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter_Statistics.Models
{
    /// <summary>
    /// Total number of tweets received
    /// Average tweets per hour/minute/second 
    /// Top emojis in tweets* 
    /// Percent of tweets that contains emojis
    /// Top hashtags
    /// Percent of tweets that contain a url 
    /// Percent of tweets that contain a photo url(pic.twitter.com or Instagram)
    /// Top domains of urls in tweets
    /// </summary>
    public class TwittsStatisticsModel
    {
        public int Total { get; set; }
        public int AvgHourly { get; set; }
        public int AvgMinutes { get; set; }
        public int AvgSeconds { get; set; }
        public string TopEmoji { get; set; }
        public int PercentEmoji { get; set; }
        public string TopHashTag { get; set; }
        public int PercentUrl { get; set; }
        public int PercentPhotoUrl { get; set; }
        public string TopDomain { get; set; }
    }
}