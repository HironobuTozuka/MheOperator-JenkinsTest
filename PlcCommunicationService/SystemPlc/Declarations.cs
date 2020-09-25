using System;

namespace PlcCommunicationService.SystemPlc
{
    public static class Declarations
    {
        public static Type[] Types =
        {
            typeof(InheritedModels.MoveRequest), typeof(InheritedModels.MoveRequestConf),
            typeof(InheritedModels.ScanNotification), typeof(Models.ExternalIO)
        };

        public static class BinaryEncodings
        {
            //Standard signals PLC OUT
            public const string MoveRequestConf = "ns=6;i=100041";
            public const string ScanNotification = "ns=6;i=100061";

            //Application signals PLC OUT
            public const string CNVStatus = "ns=6;i=100031";
            public const string Status = "ns=6;i=100071";

            //Standard signals PLC IN
            public const string MoveRequest = "ns=6;i=100011";
        }

        public static class NodePaths
        {
            public const string Prefix = "ns=6;s=::AsGlobalPV:";

            //Standard signals PLC OUT
            public const string MoveRequestConf = Prefix + "OutMoveRequestConf";
            public const string ReadMoveRequestConf = Prefix + "OutReadMoveRequestConf";
            public const string ScanNotification = Prefix + "OutScanNotification";
            public const string ReadScanNotification = Prefix + "OutReadScanNotification";
            public const string MoveRequestRead = Prefix + "OutMoveRequestRead";

            //Application signals PLC OUT
            public const string CNVStatus = Prefix + "OutCNVStatus";
            public const string Status = Prefix + "OutStatus";
            public const string CA_P_Idle = Prefix + "OutStatus.CraneA_Idle";
            public const string CB_P_Idle = Prefix + "OutStatus.CraneB_Idle";

            //Standard signals PLC IN
            public const string MoveRequest = Prefix + "InMoveRequest";
            public const string ScanNotificationRead = Prefix + "InScanNotificationRead";
            public const string ReadMoveRequest = Prefix + "InReadMoveRequest";
            public const string MoveRequestConfRead = Prefix + "InMoveRequestConfRead";
            public const string HeartBeat = Prefix + "InHeartBeat";

            //Application signals PLC IN
            public const string ExternalIO = Prefix + "InExternalIO";
        }
    }
}