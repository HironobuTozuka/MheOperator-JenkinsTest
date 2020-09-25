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
    public abstract class SignalWriter
    {
        protected UaTcpSessionChannel _channel;
        protected ILogger<SignalWriter> _logger;

        public SignalWriter(UaTcpSessionChannel channel, ILogger<SignalWriter> logger)
        {
            _channel = channel;
            _logger = logger;
        }

        public async Task<WriteResponse> WriteVar(string NodePath, object ValueToSet)
        {
            try
            {
                WriteValue[] valuesToWrite = new WriteValue[1];
                valuesToWrite[0] = new WriteValue
                {
                    // you can parse the nodeId from a string.
                    NodeId = NodeId.Parse(NodePath),
                    // variable class nodes have a Value attribute.
                    AttributeId = AttributeIds.Value,
                    Value = new DataValue(ValueToSet)
                };


                var writeRequest = new WriteRequest
                {
                    NodesToWrite = valuesToWrite
                };
                // send the ReadRequest to the server.
                WriteResponse writeResult = await _channel.WriteAsync(writeRequest);
                if (writeResult.Results.Any(result => result != StatusCodes.Good))
                {
                    string errors = "";
                    foreach (var error in writeResult.Results)
                    {
                        errors += StatusCodes.GetDefaultMessage(error);
                    }
                    _logger.LogError("Error writing variables: " + NodePath.ToString() + " error raised: " + errors);
                    throw new Exception("Error writing variables: " + NodePath.ToString() + " error raised: " + errors);
                    
                }
                else
                {
                    if(!NodePath.Contains("HeartBeat")) _logger.LogDebug($"Setting signal: {NodePath} to: {ValueToSet}");
                }
                return writeResult;
            }
            catch (Exception ex)
            {
                _logger.LogError("Opc error writing variables: " + NodePath.ToString() + " Error raised: " + ex.Message);
                throw new Exception("Opc error writing variables: " + NodePath.ToString() + " Error raised: " + ex.Message);
            }

        }

    }
}
