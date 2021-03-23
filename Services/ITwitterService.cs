using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter_Statistics.Models;

namespace Twitter_Statistics.Services
{
    public interface ITwitterService
    {
        Task<IEnumerable<TwitInfo>> GetSampleTwitts();
        Task<SampledStreamModel> GetById(int id);
        Task<TwittStatistics> GetStatistics();
        TwittStatistics GetTwittStatistics();
        TwittStatistics GetMock();
    }
}
