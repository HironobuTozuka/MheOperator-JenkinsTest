using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Models.Plc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlcRequestQueueService;

namespace PlcCommunicationService
{
    public class PlcInformationCommunication : IPlcInformationResponseListener
    {
        private readonly PlcInformationRequestProducer _plcInformationRequestProducer;
        private readonly ILogger<PlcInformationCommunication> _logger;
        private readonly List<PlcInformationResponse> _plcInformationResponses = new List<PlcInformationResponse>();
        
        public PlcInformationResponseConsumer PlcInformationResponseConsumer { get; }


        public PlcInformationCommunication(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<PlcInformationCommunication>();
            _plcInformationRequestProducer = new PlcInformationRequestProducer(loggerFactory, configuration);
            PlcInformationResponseConsumer = new PlcInformationResponseConsumer(loggerFactory, this);
            _logger.LogInformation("PlcInformationCommunication created");
        }

        public PlcInformationResponse RetrieveInformation(PlcInformationRequest request)
        {
            _logger.LogDebug("Retrieving {0} information from PLC", request.Key);
            _plcInformationRequestProducer.Produce(request).Wait();
            _logger.LogDebug("Request {0} information sent to PLC", request.Key);
            PlcInformationResponse response = null;
            var failTime = DateTime.Now.Add(TimeSpan.FromSeconds(10));
            while (response == null && DateTime.Now < failTime)
            {
                response = _plcInformationResponses.FirstOrDefault(res => res.Key.Equals(request.Key));
                Thread.Sleep(10);
            }

            _logger.LogDebug("Retrieved information from PLC {0}", response);
            if (response != null) _plcInformationResponses.Remove(response);
            if (response == null) _logger.LogError("Unable to retrieve information from PLC");

            return response;
        }

        public async Task NotifyListener(PlcInformationResponse plcInformationResponse)
        {
            _plcInformationResponses.Add(plcInformationResponse);
        }
    }
}