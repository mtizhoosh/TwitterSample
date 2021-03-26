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
using Twitter_Statistics.Services;

namespace Twitter_Statistics.Repository
{

    public class TwitterFileProvider : ITwitProvider
    {
        private ILogger _logger;
        private string _fileFullName;

        private static bool _testSwitch = false;

        public TwitterFileProvider(ILogger logger, string filePath) 
        {
            this._fileFullName = filePath;
            this._logger = logger;
        }

        /// <summary>
        /// Mock test twitts taken from file 
        /// </summary>
        /// <returns></returns>
        public async Task<List<TwitInfo>> GetSampleTwitts()
        {
            List<TwitInfo> MockTwittes = null;
            try
            {
                await Task.Run(() =>
                {
                    string twitts = string.Empty;
                    if (TwitterFileProvider._testSwitch)
                    {
                        twitts = File.ReadAllText(this._fileFullName);
                    }
                    else
                    {
                        twitts = TwitterFileProvider.TestTwitts;
                    }

                    if (twitts != string.Empty) TwitterFileProvider._testSwitch = !TwitterFileProvider._testSwitch;

                    MockTwittes = JsonSerializer.Deserialize<List<TwitInfo>>(twitts);
                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Test file not found.{ex.Message}");
            }


            return MockTwittes;
        }
                
        private static string TestTwitts = @"
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
