using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter_Statistics.Models;

namespace Twitter_Statistics.Services
{
    public enum TwitProvider
    {
        None,
        File,
        TwitterAPI = 2,
        Database = 3
    }

    public interface IBaseService
    {
        TwittStatistics GetTwittStatistics();
    }
}
