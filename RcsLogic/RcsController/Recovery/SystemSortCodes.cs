using System.Collections.Generic;
using System.Linq;
using Common.Models.Task;
using RcsLogic.Robot;

namespace RcsLogic.RcsController.Recovery
{
    public class SystemSortCodes
    {
        public static SystemSortCode Get(ushort code)
        {
            if (code == 1) return new SystemSortCode(code, null, "Success", null);
            return _sortCodes.FirstOrDefault(c => c.Code.Equals(code)) ?? new SystemSortCode(code, "Other", "Other", SystemFailReason.Other);
        }
        
        //Crane A
        //Pick
        
        public static readonly SystemSortCode PickError_CA_P_1110 = new SystemSortCode(11110, "YAxisAbsPosGreaterThan6",
            "When PLC started executing pick movement the shelf absolute Y position was greater than 6 (not in home position)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1111 = new SystemSortCode(11111, "NoHomeSensorSignal",
            "When PLC started executing pick movement both Home sensors ware not triggered (either the sensor is broken or shelf is not in home position)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1112 = new SystemSortCode(11112, "ToteBorderTriggered",
            "When PLC started executing pick movement tote border sensor was triggered (tote is sticking out of the platform)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1150 = new SystemSortCode(11150, "DestinationNotAllowed",
            "The destination position is not allowed, either the position is not reachable for platform or position is disabled from HMI)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1151_ToteOnPlatform = new SystemSortCode(11151, "ToteOnPlatform",
            "Tote sensor detected a tote on platform when starting pick)", SystemFailReason.ToteOnPlatform);
        
        public static readonly SystemSortCode PickError_CA_P_1210_SourceLocationEmpty = new SystemSortCode(11210, "LocationEmpty",
            "Pick position is empty", SystemFailReason.NoTote);
        
        public static readonly SystemSortCode PickError_CA_P_1211_SourceLocationEmpty = new SystemSortCode(11211, "LocationEmpty",
            "Pick position is empty", SystemFailReason.NoTote);

        public static readonly SystemSortCode PickError_CA_P_1231 = new SystemSortCode(11231, "CNV2_1LiftNotInUpPos",
            "When starting double pick the CNV2_1 lift was not in up position", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1232 = new SystemSortCode(11232, "CNV2_2LiftNotInUpPos",
            "When starting double pick the CNV2_2 lift was not in up position", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1257 = new SystemSortCode(11257, "ToteOverfill",
            "Low tote overfill sensor was triggered while picking", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PickError_CA_P_1258 = new SystemSortCode(11258, "ToteOverfill",
            "High tote overfill sensor was triggered while picking", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PickError_CA_P_1259 = new SystemSortCode(11259, "ToteOverfill",
            "Low tote overfill sensor was triggered while picking", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PickError_CA_P_1260 = new SystemSortCode(11260, "ToteOverfill",
            "High tote overfill sensor was triggered while picking", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PickError_CA_P_1261 = new SystemSortCode(11261, "ToteOverfillBadState",
            "Low tote overfill sensor was not triggered, but high tote overfill was while picking", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1262 = new SystemSortCode(11262, "ToteOverfillBadState",
            "Low tote overfill sensor was not triggered, but high tote overfill was while picking", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1263 = new SystemSortCode(11263, "NoRead",
            "NoRead was triggered while picking", SystemFailReason.NoRead);
        
        public static readonly SystemSortCode PickError_CA_P_1264 = new SystemSortCode(11264, "NoRead",
            "NoRead was triggered while picking", SystemFailReason.NoRead);
        
        public static readonly SystemSortCode PickError_CA_P_1265 = new SystemSortCode(11265, "CNV1_1LiftNotInUpPos",
            "When starting pick the CNV1_1 lift was not in up position", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1266 = new SystemSortCode(11266, "CNV1_2LiftNotInUpPos",
            "When starting pick the CNV1_2 lift was not in up position", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1267 = new SystemSortCode(11267, "CNV2_1LiftNotInUpPos",
            "When starting pick the CNV2_1 lift was not in up position", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1268 = new SystemSortCode(11268, "CNV2_2LiftNotInUpPos",
            "When starting pick the CNV2_2 lift was not in up position", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1292 = new SystemSortCode(11292, "ToteOverfill",
            "Low tote overfill sensor was triggered while picking", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PickError_CA_P_1293 = new SystemSortCode(11293, "ToteOverfill",
            "High tote overfill sensor was triggered while picking", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PickError_CA_P_1294 = new SystemSortCode(11294, "ToteOverfillBadState",
            "Low tote overfill sensor was not triggered, but high tote overfill was while picking", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1298 = new SystemSortCode(11298, "NoRead",
            "NoRead was triggered while picking", SystemFailReason.NoRead);
        
        public static readonly SystemSortCode PickError_CA_P_1990 = new SystemSortCode(11990, "PackMLStop",
            "PackML is in Stop state", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CA_P_1991 = new SystemSortCode(11991, "DeadManReleased",
            "Dead man switch was released", SystemFailReason.Pick);

        public static readonly SystemSortCode PickError_CA_P_1998 = new SystemSortCode(11998, "CNCError",
            "CNC system error", SystemFailReason.Pick);
        
        //Place
        public static readonly SystemSortCode PlaceError_CA_P_1310 = new SystemSortCode(11310, "YAxisAbsPosGreaterThan6",
            "When PLC started executing place movement the shelf absolute Y position was greater than 6 (not in home position)", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1311 = new SystemSortCode(11311, "NoHomeSensorSignal",
            "When PLC started executing place movement both Home sensors ware not triggered (either the sensor is broken or shelf is not in home position)", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1312 = new SystemSortCode(11312, "ToteBorderTriggered",
            "When PLC started executing place movement tote border sensor was triggered (tote is sticking out of the platform)", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1350 = new SystemSortCode(11350, "DestNotAllowed",
            "The dest position is not allowed, either the position is not reachable for platform or position is disabled from HMI)", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1351 = new SystemSortCode(11351, "NoToteOnPlatform",
            "Tote sensor did not detect tote on platform when starting place)", SystemFailReason.NoTote);
        
        public static readonly SystemSortCode PlaceError_CA_P_1352 = new SystemSortCode(11352, "ToteNotAllowedOnPosition",
            "Tote type is not allowed on this position (tote too high for this position or unknown type)", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1410 = new SystemSortCode(11410, "DestLocationOccupied",
            "Place position is occupied", SystemFailReason.PlaceLocationOccupied);
        
        public static readonly SystemSortCode PlaceError_CA_P_1411 = new SystemSortCode(11411, "DestLocationOccupied",
            "Place position is occupied", SystemFailReason.PlaceLocationOccupied);
        
        public static readonly SystemSortCode PlaceError_CA_P_1412 = new SystemSortCode(11412, "CNV1_2LiftNotInUpPos",
            "When trying to place on CNV1_2 lift was not in up position", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1413 = new SystemSortCode(11413, "CNV1_1LiftNotInUpPos",
            "When trying to place on CNV1_1 lift was not in up position", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1414 = new SystemSortCode(11414, "CNV2_1LiftNotInUpPos",
            "When trying to place on CNV2_1 lift was not in up position", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1415 = new SystemSortCode(11415, "CNV2_2LiftNotInUpPos",
            "When trying to place on CNV2_2 lift was not in up position", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1470 = new SystemSortCode(11470, "ToteOverfill",
            "Tote overfill sensor was triggered while placing. Tote placed on dest", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PlaceError_CA_P_1471 = new SystemSortCode(11471, "ToteOverfill",
            "Tote overfill sensor was triggered while placing. Tote left on platform", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PlaceError_CA_P_1510 = new SystemSortCode(11510, "DestLocationOccupied",
            "Dest location was not empty when platform 1 was placing during double place", SystemFailReason.PlaceLocationOccupied);
        
        public static readonly SystemSortCode PlaceError_CA_P_1511 = new SystemSortCode(11511, "DestLocationOccupied",
            "Dest location was not empty when platform 2 was placing during double place", SystemFailReason.PlaceLocationOccupied);
        
        public static readonly SystemSortCode PlaceError_CA_P_1512 = new SystemSortCode(11512, "CNV1_1LiftNotUp",
            "Dest location was not empty when platform 1 was placing during double place", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1513 = new SystemSortCode(11513, "CNV1_2LiftNotUp",
            "Dest location was not empty when platform 2 was placing during double place", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CA_P_1570 = new SystemSortCode(11570, "ToteOverfill",
            "Tote overfill sensor was triggered while placing. Tote placed on dest", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PlaceError_CA_P_1571 = new SystemSortCode(11571, "ToteOverfill",
            "Tote overfill sensor was triggered while placing. Tote left on platform", SystemFailReason.Overfill);
        
        //Crane B
        //Pick
        
        public static readonly SystemSortCode PickError_CB_P_1150 = new SystemSortCode(21150, "YAxisAbsPosGreaterThan6",
            "When PLC started executing pick movement the shelf absolute Y position was greater than 6 (not in home position)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CB_P_1151 = new SystemSortCode(21151, "NoHomeSensorSignal",
            "When PLC started executing pick movement both Home sensors ware not triggered (either the sensor is broken or shelf is not in home position)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CB_P_1152 = new SystemSortCode(21152, "ToteBorderTriggered",
            "When PLC started executing pick movement tote border sensor was triggered (tote is sticking out of the platform)", SystemFailReason.Pick);

        public static readonly SystemSortCode PickError_CB_P_1200 = new SystemSortCode(21200, "DestinationNotAllowed",
            "The destination position is not allowed, either the position is not reachable for platform or position is disabled from HMI)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CB_P_1201_ToteOnPlatform = new SystemSortCode(21201, "ToteOnPlatform",
            "Tote sensor detected a tote on platform when starting pick)", SystemFailReason.ToteOnPlatform);
        
        public static readonly SystemSortCode PickError_CB_P_1210_SourceLocationEmpty = new SystemSortCode(21210, "LocationEmpty",
            "Pick position is empty", SystemFailReason.NoTote);
        
        public static readonly SystemSortCode PickError_CB_P_1211_SourceLocationEmpty = new SystemSortCode(21211, "LocationEmpty",
            "Pick position is empty", SystemFailReason.NoTote);
        
        public static readonly SystemSortCode PickError_CB_P_1212 = new SystemSortCode(21212, "YPickPositionZero",
            "Pick position Y coordinate is zero (programmed position is wrong)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CB_P_1213 = new SystemSortCode(21213, "ORDER1ConveyorTimeout",
            "When picking from ORDER1 the tote didn't arrive to the sensor on conveyor and timeout was activated", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CB_P_1214 = new SystemSortCode(21214, "ORDER2ConveyorTimeout",
            "When picking from ORDER2 the tote didn't arrive to the sensor on conveyor and timeout was activated", SystemFailReason.Pick);

        public static readonly SystemSortCode PickError_CB_P_1215 = new SystemSortCode(21215, "CNV1_5ConveyorTimeout",
            "When picking from CNV1_5 the tote didn't arrive to the sensor on conveyor and timeout was activated", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CB_P_1216 = new SystemSortCode(21216, "CNV2_5ConveyorTimeout",
            "When picking from CNV2_5 the tote didn't arrive to the sensor on conveyor and timeout was activated", SystemFailReason.Pick);

        public static readonly SystemSortCode PickError_CB_P_1217_RPP1RobotReleaseTimeout = new SystemSortCode(21217, "RPP1RobotReleaseTimeout",
            "When picking from RPP1 robot hasn't released the location - location released from Mujin", SystemFailReason.Pick);

        public static readonly SystemSortCode PickError_CB_P_1420 = new SystemSortCode(21420, "ToteOverfill",
            "Low tote overfill sensor was triggered while picking", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PickError_CB_P_1421 = new SystemSortCode(21421, "ToteOverfill",
            "High tote overfill sensor was triggered while picking", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PickError_CB_P_1422 = new SystemSortCode(21422, "ToteOverfillBadState",
            "Low tote overfill sensor was not triggered, but high tote overfill was while picking", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CB_P_1990 = new SystemSortCode(21990, "PackMLStop",
            "PackML is in Stop state", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PickError_CB_P_1991 = new SystemSortCode(21991, "DeadManReleased",
            "Dead man switch was released", SystemFailReason.Pick);

        public static readonly SystemSortCode PickError_CB_P_1998 = new SystemSortCode(21998, "CNCError",
            "CNC system error", SystemFailReason.Pick);
        
        //Place
        
        public static readonly SystemSortCode PlaceError_CB_P_1400 = new SystemSortCode(21400, "DestNotAllowed",
            "The dest position is not allowed, either the position is not reachable for platform or position is disabled from HMI)", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CB_P_1401 = new SystemSortCode(21401, "NoToteOnPlatform",
            "Tote sensor did not detect tote on platform when starting place", SystemFailReason.NoTote);
        
        public static readonly SystemSortCode PlaceError_CB_P_1410 = new SystemSortCode(21410, "CNV1_5BlockedByRobot",
            "When crane was trying to place on CNV1_5 the location was blocked by robot", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CB_P_1411 = new SystemSortCode(21411, "CNV2_5BlockedByRobot",
            "When crane was trying to place on CNV2_5 the location was blocked by robot", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CB_P_1412 = new SystemSortCode(21412, "LocationOccupied",
            "Place position is occupied", SystemFailReason.PlaceLocationOccupied);
        
        public static readonly SystemSortCode PlaceError_CB_P_1413 = new SystemSortCode(21413, "LocationOccupied",
            "Place position is occupied", SystemFailReason.PlaceLocationOccupied);
        
        public static readonly SystemSortCode PlaceError_CB_P_1414 = new SystemSortCode(21414, "YPlacePositionZero",
            "Place position Y coordinate is zero (programmed position is wrong)", SystemFailReason.Pick);
        
        public static readonly SystemSortCode PlaceError_CB_P_1415 = new SystemSortCode(21415, "NoRead",
            "NoRead was triggered while picking", SystemFailReason.NoRead);
        
        public static readonly SystemSortCode PlaceError_CB_P_1416 = new SystemSortCode(21416, "ToteNotAllowedOnPosition",
            "Tote type is not allowed on this position (tote too high for this position or unknown type)", SystemFailReason.Place);
 
        public static readonly SystemSortCode PlaceError_CB_P_1417 = new SystemSortCode(21417, "YAxisAbsPosGreaterThan6",
            "When PLC started executing place movement the shelf absolute Y position was greater than 6 (not in home position)", SystemFailReason.Place);

        public static readonly SystemSortCode PlaceError_CB_P_1418 = new SystemSortCode(21418, "NoHomeSensorSignal",
            "When PLC started executing place movement both Home sensors ware not triggered (either the sensor is broken or shelf is not in home position)", SystemFailReason.Place);
        
        public static readonly SystemSortCode PlaceError_CB_P_1419 = new SystemSortCode(21419, "ToteBorderTriggered",
            "Whe PLC started executing place movement tote border sensor was triggered (tote is sticking out of the platform)", SystemFailReason.Place);

        public static readonly SystemSortCode PlaceError_CB_P_1470 = new SystemSortCode(21470, "ToteOverfill",
            "Tote overfill sensor was triggered while placing. Tote placed on dest", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PlaceError_CB_P_1471 = new SystemSortCode(21471, "ToteOverfill",
            "Tote overfill sensor was triggered while placing. Tote left on platform", SystemFailReason.Overfill);

        public static readonly SystemSortCode PlaceError_CB_P_1801 = new SystemSortCode(21801, "ToteAligningError",
            "Tote sensor did not detect tote at the beginning of the cycle", SystemFailReason.Overfill);
        
        public static readonly SystemSortCode PlaceError_CB_P_1802 = new SystemSortCode(21802, "ToteAligningError",
            "Tote sensor did not detect tote at the end of the cycle", SystemFailReason.Overfill);
        
        //Common errors
        
        public static readonly SystemSortCode Error_2_UnknownRoute = new SystemSortCode(2, "UnknownRoute",
            "This route is unknown", SystemFailReason.Pick);
        
        public static readonly SystemSortCode Error_3_RouteDisabled = new SystemSortCode(3, "RouteDisabled",
            "This route is disabled", SystemFailReason.Pick);
        
        public static readonly SystemSortCode Error_5_DeviceBusy = new SystemSortCode(5, "DeviceBusy",
            "Device was busy when received the request", SystemFailReason.DeviceBusy);
        
        public static readonly SystemSortCode Error_10_PLCNotInExecute = new SystemSortCode(10, "PLCNotInExecute",
            "Plc was not in execute when received the request", SystemFailReason.Pick);
        
        //Conveying
        
        public static readonly SystemSortCode Error_11_NoToteOnSourceOnConveying = new SystemSortCode(11, "NoToteOnSourceOnConveying",
            "When PLC started executing the request, there was no tote on source location", SystemFailReason.NoTote);
        
        public static readonly SystemSortCode Error_12_ConveyorTransferTimeout = new SystemSortCode(12, "ConveyorTransferTimeout",
            "Conveyor was executing a transfer, but the tote hasn't reached the destination within the timeout", SystemFailReason.Place);

        private static readonly List<SystemSortCode> _sortCodes = new List<SystemSortCode>
        {
            PickError_CA_P_1110,
            PickError_CA_P_1111,
            PickError_CA_P_1112,
            PickError_CA_P_1150,
            PickError_CA_P_1151_ToteOnPlatform,
            PickError_CA_P_1210_SourceLocationEmpty,
            PickError_CA_P_1211_SourceLocationEmpty,
            PickError_CA_P_1231,
            PickError_CA_P_1232,
            PickError_CA_P_1257,
            PickError_CA_P_1258,
            PickError_CA_P_1259,
            PickError_CA_P_1260,
            PickError_CA_P_1261,
            PickError_CA_P_1262,
            PickError_CA_P_1263,
            PickError_CA_P_1264,
            PickError_CA_P_1265,
            PickError_CA_P_1266,
            PickError_CA_P_1267,
            PickError_CA_P_1268,
            PickError_CA_P_1292,
            PickError_CA_P_1293,
            PickError_CA_P_1294,
            PickError_CA_P_1298,
            
            PlaceError_CA_P_1310,
            PlaceError_CA_P_1311,
            PlaceError_CA_P_1312,
            PlaceError_CA_P_1350,
            PlaceError_CA_P_1351,
            PlaceError_CA_P_1352,
            PlaceError_CA_P_1410,
            PlaceError_CA_P_1411,
            PlaceError_CA_P_1412,
            PlaceError_CA_P_1413,
            PlaceError_CA_P_1414,
            PlaceError_CA_P_1415,
            PlaceError_CA_P_1470,
            PlaceError_CA_P_1471,
            PlaceError_CA_P_1510,
            PlaceError_CA_P_1511,
            PlaceError_CA_P_1512,
            PlaceError_CA_P_1513,
            PlaceError_CA_P_1570,
            PlaceError_CA_P_1571,
            
            PickError_CA_P_1990,
            PickError_CA_P_1991,
            PickError_CA_P_1998,
            
            PickError_CB_P_1150,
            PickError_CB_P_1151,
            PickError_CB_P_1152,
            PickError_CB_P_1200,
            PickError_CB_P_1201_ToteOnPlatform,
            PickError_CB_P_1210_SourceLocationEmpty,
            PickError_CB_P_1211_SourceLocationEmpty,
            PickError_CB_P_1212,
            PickError_CB_P_1213,
            PickError_CB_P_1214,
            PickError_CB_P_1215,
            PickError_CB_P_1216,
            PickError_CB_P_1217_RPP1RobotReleaseTimeout,
            PickError_CB_P_1420,
            PickError_CB_P_1421,
            PickError_CB_P_1422,
            
            PlaceError_CB_P_1400,
            PlaceError_CB_P_1401,
            PlaceError_CB_P_1410,
            PlaceError_CB_P_1411,
            PlaceError_CB_P_1412,
            PlaceError_CB_P_1413,
            PlaceError_CB_P_1414,
            PlaceError_CB_P_1415,
            PlaceError_CB_P_1416,
            PlaceError_CB_P_1417,
            PlaceError_CB_P_1418,
            PlaceError_CB_P_1419,
            PlaceError_CB_P_1470,
            PlaceError_CB_P_1471,
            PlaceError_CB_P_1801,
            PlaceError_CB_P_1802,
            
            PickError_CB_P_1990,
            PickError_CB_P_1991,
            PickError_CB_P_1998,
            
            Error_2_UnknownRoute,
            Error_3_RouteDisabled,
            Error_5_DeviceBusy,
            Error_10_PLCNotInExecute,
            Error_11_NoToteOnSourceOnConveying,
            Error_12_ConveyorTransferTimeout
        };
        
        public static readonly SystemSortCode[] RecoverableErrors =
        {
            PickError_CA_P_1151_ToteOnPlatform,
            PickError_CB_P_1201_ToteOnPlatform,
            Error_5_DeviceBusy,
            Error_10_PLCNotInExecute,
            PickError_CB_P_1217_RPP1RobotReleaseTimeout
        };

        public static readonly SystemSortCode[] TaskRecoverableError =
        {
            PlaceError_CA_P_1410,
            PlaceError_CA_P_1411,
            PlaceError_CB_P_1412,
            PlaceError_CB_P_1413,
            PickError_CA_P_1990,
            PickError_CA_P_1991,
            PickError_CB_P_1990,
            PickError_CB_P_1991
        };
    }
}