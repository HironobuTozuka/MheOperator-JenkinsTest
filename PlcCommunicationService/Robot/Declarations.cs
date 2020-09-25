using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;

namespace PlcCommunicationService.Robot
{
    public static class Declarations
    {
        public static Type[] Types = {typeof(InheritedModels.MoveRequest), typeof(InheritedModels.MoveRequestConf)};
        // public static class BinaryEncodings
        // {
        //     //Standard signals PLC OUT
        //     public const string MoveRequestConf = "ns=4;s=|enc|CODESYS Control for Raspberry Pi SL.Application.OPCUA_MoveRequestConf_typ";
        //
        //     //Standard signals PLC IN
        //     public const string MoveRequest = "ns=4;s=|enc|CODESYS Control for Raspberry Pi SL.Application.OPCUA_MoveRequest_typ";
        // }
        //
        // public static class NodePaths
        // {
        //     //Standard signals PLC OUT
        //     public const string MoveRequestConf = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.OutMoveRequestConf";
        //     public const string ReadMoveRequestConf = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.OutReadMoveRequestConf";
        //     public const string MoveRequestRead = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.OutMoveRequestRead";
        //
        //     //Application signals PLC OUT
        //     public const string CurrentPartName = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.OutCurrentPartName";
        //     public const string PartsPicked = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.OutPartsPicked";
        //     public const string OrderCycleRunning = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.OutOrderCycleRunning";
        //
        //     //Standard signals PLC IN
        //     public const string MoveRequest = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.InMoveRequest";
        //     public const string ReadMoveRequest = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.InReadMoveRequest";
        //     public const string MoveRequestConfRead = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.InMoveRequestConfRead";
        //     public const string HeartBeat = "ns=4;s=|var|CODESYS Control for Raspberry Pi SL.Application.GVL.InHeartBeat";
        //
        //     //Application signals PLC IN
        // }

        //For Omron

        public static class BinaryEncodings
        {
            //Standard signals PLC OUT
            public const string MoveRequestConf = "ns=4;i=5014";
        
            //Standard signals PLC IN
            public const string MoveRequest = "ns=4;i=5008";
        }
        
        public static class NodePaths
        {
            //Standard signals PLC OUT
            public const string MoveRequestConf = "ns=4;s=OutMoveRequestConf";
            public const string ReadMoveRequestConf = "ns=4;s=OutReadMoveRequestConf";
            public const string MoveRequestRead = "ns=4;s=OutMoveRequestRead";
        
            //Application signals PLC OUT
            public const string CurrentPartName = "ns=4;s=OutCurrentPartName";
            public const string PartsPicked = "ns=4;s=OutPartsPicked";
            public const string OrderCycleRunning = "ns=4;s=OutOrderCycleRunning";
        
            //Standard signals PLC IN
            public const string MoveRequest = "ns=4;s=InMoveRequest";
            public const string ReadMoveRequest = "ns=4;s=InReadMoveRequest";
            public const string MoveRequestConfRead = "ns=4;s=InMoveRequestConfRead";
            public const string HeartBeat = "ns=4;s=InHeartBeat";
        
            //Application signals PLC IN
        }
    }
}