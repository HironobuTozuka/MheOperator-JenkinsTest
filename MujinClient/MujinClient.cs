using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MujinClient.Models;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Common;
using Common.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace MujinClient
{
    public class MujinClient : IMujinClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MujinClient> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly string _login = "mujin";

        public MujinClient(ILoggerFactory loggerFactory, IMemoryCache memoryCache, IConfiguration configuration,
            HttpClient httpClient)
        {
            _logger = loggerFactory.CreateLogger<MujinClient>();
            _memoryCache = memoryCache;
            _httpClient = httpClient;
            _logger.LogInformation("Initializig Mujin Client");
        }


        public void CheckSkuBarcodes(List<string> barcodes)
        {
            List<string> nonExistingBarcodes = new List<string>();

            try
            {
                foreach (var barcode in barcodes)
                {
                    try
                    {
                        GetSkuName(barcode);
                    }
                    catch (Exception e)
                    {
                        var exception = e as FindSkuException;
                        nonExistingBarcodes.Add(barcode);
                    }
                }

                if (nonExistingBarcodes.Count > 0)
                {
                    throw new UnknownSkuException(){SkuIds = nonExistingBarcodes};
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
        }

        private async Task<KeyValuePair<string, COLLADA>> _GetMasterObject(string barcode)
        {
            string partFileName;

            using (var response = await _httpClient.GetAsync("query/barcodes/?barcodes=" + barcode))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var fileList = JsonConvert.DeserializeObject<string[]>(apiResponse);

                List<KeyValuePair<string, COLLADA>> groupFiles = new List<KeyValuePair<string, COLLADA>>();

                foreach (var fileName in fileList)
                {
                    using (var partResponse = await _httpClient.GetAsync("u/" + _login + "/" + fileName))
                    {
                        string apiPartResponse = await partResponse.Content.ReadAsStringAsync();
                        TextReader reader = new StringReader(apiPartResponse);
                        var collada = new XmlSerializer(typeof(COLLADA)).Deserialize(reader) as COLLADA;
                        if (collada.asset.keywords.Contains("set"))
                        {
                            groupFiles.Add(new KeyValuePair<string, COLLADA>(fileName, collada));
                        }
                    }
                }

                if (groupFiles.Count == 0)
                    throw new FindSkuException("No master objectc found for barcode " + barcode, false);
                if (groupFiles.Count > 1)
                    throw new FindSkuException("There is not only one master object for barcode " + barcode, true);

                return groupFiles.FirstOrDefault();
            }
        }

        private async Task<string> _GetSkuName(string barcode)
        {
            try
            {
                var masterObject = await _GetMasterObject(barcode);

                return masterObject.Key.Substring(0, masterObject.Key.LastIndexOf(".mujin.dae"));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
        }

        public string GetSkuName(string barcode)
        {
            return _memoryCache.GetOrCreate<string>(barcode, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                var awaiter = _GetSkuName(barcode);
                awaiter.Wait();

                return awaiter.Result;
            });
        }
    }
}