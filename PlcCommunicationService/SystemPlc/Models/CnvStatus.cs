using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;

namespace PlcCommunicationService.SystemPlc.Models
{
    public class CnvStatus : SignalReader
    {
        public async Task<bool> CNV1_1Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_1Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV1_1ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_1ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV1_2Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_2Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV1_2ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_2ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV1_3Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_3Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV1_3ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_3ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV1_4Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_4Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV1_4ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_4ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV1_5Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_5Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV1_5ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV1_5ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV2_1Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_1Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV2_1ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_1ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV2_2Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_2Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV2_2ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_2ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV2_3Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_3Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV2_3ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_3ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV2_4Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_4Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV2_4ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_4ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV2_5Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_5Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV2_5ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV2_5ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV3_2Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV3_2Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV3_2ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV3_2ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> CNV4_2Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV4_2Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> CNV4_2ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.CNV4_2ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> LOAD1_1Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.LOAD1_1Occupied"))
                .GetValueOrDefault<bool>();
        }

        public async Task<string> LOAD1_1ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.LOAD1_1ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> LOAD1_2Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.LOAD1_2Occupied"))
                .GetValueOrDefault<bool>();
        }

        public async Task<string> LOAD1_2ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.LOAD1_2ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> ORDER1Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.ORDER1Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> ORDER1ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.ORDER1ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public async Task<bool> ORDER2Occupied()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.ORDER2Occupied")).GetValueOrDefault<bool>();
        }

        public async Task<string> ORDER2ToteBarcode()
        {
            return (await ReadVariable($"{Declarations.NodePaths.CNVStatus}.ORDER2ToteBarcode"))
                .GetValueOrDefault<string>();
        }

        public CnvStatus(ILoggerFactory loggerFactory, UaTcpSessionChannel channel) : base(channel,
            loggerFactory.CreateLogger<CnvStatus>())
        {
        }
    }
}