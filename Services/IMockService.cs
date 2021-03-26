using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter_Statistics.Models;

namespace Twitter_Statistics.Services
{
    public interface ITwitterFileService 
    {
        TwittStatistics GetFileData();
    }
}
