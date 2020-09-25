using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;
using System.ComponentModel;
using Workstation.ServiceModel.Ua.Channels;
using Microsoft.Extensions.Logging;

namespace PlcCommunicationService
{
    public class SignalReader
    {
        protected UaTcpSessionChannel _channel;
        protected ILogger<SignalReader> _logger;

        public SignalReader(UaTcpSessionChannel channel, ILogger<SignalReader> logger)
        {
            _channel = channel;
            _logger = logger;
        }

        public async Task<DataValue> ReadVariable(string varName)
        {
            var value = await ReadVariables(new string[] { varName });
            if (value != null) return value.FirstOrDefault();
            return null;
        }

        public async Task<DataValue[]> ReadVariables(string[] varNames)
        {
            return await ReadVariables(varNames.Select(NodeId.Parse).ToArray());
        }
        
        public async Task<DataValue[]> ReadVariables(NodeId[] nodeIds)
        {
            try
            {
                ReadValueId[] valuesToRead = new ReadValueId[nodeIds.Length];
                for (int i = 0; i < nodeIds.Length; i++)
                {
                    valuesToRead[i] = new ReadValueId
                    {
                        // you can parse the nodeId from a string.
                        NodeId = nodeIds[i],
                        // variable class nodes have a Value attribute.
                        AttributeId = AttributeIds.Value
                    };
                }

                var readRequest = new ReadRequest
                {
                    NodesToRead = valuesToRead
                };

                // send the ReadRequest to the server.
                var readResult = await _channel.ReadAsync(readRequest);
                return readResult.Results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Opc error reading variables: " + nodeIds + " Error raised: ");
                if (ex.Message.Contains("Timeout"))
                {
                    _logger.LogError("Opc Aborting on error reading variables: " + nodeIds);
                }
                else
                {

                }

                return null;
            }
        }

    }
}
