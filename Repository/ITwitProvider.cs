using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter_Statistics.Models;

namespace Twitter_Statistics.Repository
{
    public interface ITwitProvider
    {
        Task<List<TwitInfo>> GetSampleTwitts();
    }
}
