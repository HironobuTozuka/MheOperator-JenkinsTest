using System.Collections.Generic;
using System.Linq;
using Common.Models.Task;

namespace RcsLogic.Robot
{
    public static class RobotSortCodes
    {
        public static RobotSortCode Get(ushort code)
        {
            if (code == 1) return new RobotSortCode(code, null, "Success", null);
            return _sortCodes.FirstOrDefault(c => c.Code.Equals(code)) 
                   ?? new RobotSortCode(code, "Other", "Other", FailReason.OtherPickError);
        }
        
        public static readonly RobotSortCode FinishedNoMoreTargets = new RobotSortCode(2, "FinishedNoMoreTargets",
            "Unable to complete transfer of grasp targets specified in orderNumber as no grasp targets " +
            "are detected in the grasp side location ([numLeftInLocation#]). " +
            "Alternatively, if the Mujin Controller is set this way, the container is empty.", FailReason.SourceToteError,
            shakingCanHelp: true);
        public static readonly RobotSortCode FinishedNoMoreTargetsNotEmpty = new RobotSortCode(3, "FinishedNoMoreTargetsNotEmpty",
            "Unable to complete transfer of grasp targets specified in [orderNumber] as no grasp targets " +
            "are detected in the grasp side location ([numLeftInLocation#]). However, the container is not empty. " +
            "This value will not be used unless the Mujin Controller is set to detect this state.", FailReason.SourceToteError,
            shakingCanHelp: true);
        
        public static readonly RobotSortCode FinishedNoMoreDest = new RobotSortCode(4, "FinishedNoMoreDest",
            "Unable to place the grasp targets specified by [orderNumber].", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedNoEnvironmentUpdate = new RobotSortCode(5, "FinishedNoEnvironmentUpdate",
            "Vision Processing did not occur and planning timed out. This can happen if Prohibited signals " +
            "are incorrect, or the sensors were not initialized.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedDropTargetFailure = new RobotSortCode(6, "FinishedDropTargetFailure",
            "The piece dropped during the transfer and it was necessary to abort transfer operation", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedTooManyPickFailures = new RobotSortCode(7, "FinishedTooManyPickFailures",
            "Picking failed repeatedly until the system gave up", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedRobotExecutionError = new RobotSortCode(8, "FinishedRobotExecutionError",
            "Something stopped the robot from executing, please check robot log. Perhaps EStop was pressed," +
            " or Auto Mode changed, or problem with robot controller.", FailReason.RobotError);
        
        public static readonly RobotSortCode FinishedNoDestObstacles = new RobotSortCode(9, "FinishedNoDestObstacles",
            "Planning did not get the dest obstacles before a timeout occurred. Probably a location " +
            "Prohibited signal was blocking the objects, or a race condition happened internally in the system.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedStopDueToTorqueLimit = new RobotSortCode(10, "FinishedStopDueToTorqueLimit",
            "f robot hit something on the way and the torque surpassed the limits controlled by " +
            "approachCurrentExceedThresholds/approachCurrentExceedThresholdsDelta/" +
            "destApproachCurrentExceedThresholds/destApproachCurrentExceedThresholdsDelta. " +
            "Whether the robot stops or not is controlled by parameters with the following substring" +
            " \"isStopOnTorqueLimits\".", FailReason.RobotError);
        
        public static readonly RobotSortCode FinishedGripperExecutionError = new RobotSortCode(11, "FinishedGripperExecutionError",
            "Something stopped the gripper from executing, please check robot log. " +
            "Perhaps EStop was pressed, or Auto Mode changed, or problem with gripper/robot controller.", FailReason.RobotError);
        
        public static readonly RobotSortCode FinishedCannotRecoverWhileGrabbingTarget = new RobotSortCode(12, "FinishedCannotRecoverWhileGrabbingTarget",
            "A problem occurred while executing the order cycle and the robot was grabbing a target object." +
            " Robot tried to recover, but failed.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedCannotRecoverWhileIntermediateCycles = new RobotSortCode(13, "FinishedUnexpectedPlacement",
                "A problem occurred while executing the order cycle which includes multiple intermediate cycles. " +
                "Robot tried to recover, but failed probably because there was a risk to leave part at intermediate destination.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedCannotRecover = new RobotSortCode(14, "FinishedCannotRecover",
                "Robot had a problem executing current cycle, and when trying to recover, " +
                "it failed, should look at other logs to find out the cause", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedBadIOConditions = new RobotSortCode(15, "FinishedBadIOConditions",
                "Given IO signal conditions were not met. ", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedGripperPositionNotReached = new RobotSortCode(16, "FinishedGripperPositionNotReached",
                "The gripper could not move to the given command. " +
                "Please check the gripper and sensors to make sure that everything is in order.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedExecutionError = new RobotSortCode(17, "FinishedExecutionError",
                "The execution stopped with an error.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedForceTorqueCalibrationError = new RobotSortCode(18, "FinishedForceTorqueCalibrationError",
                "Got an error trying to calibrate the force/torque sensor. Usage of force/torque estimation is" +
                " controlled by [binpickingparameters/forceTorqueBasedEstimatorParameters].", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedCannotReturnPartToContainer = new RobotSortCode(19, "FinishedCannotReturnPartToContainer",
                "A problem occurred while the robot was grasping a part, and robot could not place" +
                " the part cannot be placed in the destination goals, nor can it be placed back to the pick container.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedSourceVerificationIsLate = new RobotSortCode(20, "FinishedSourceVerificationIsLate",
                "Delays in the system caused source verification point cloud to be late," +
                " and the robot was expected to hit that point cloud. Since robot went passed the point of hitting," +
                " the integrity or safety of the scene is not clear. Please check and recover correctly.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedExcessiveExternalForce = new RobotSortCode(21, "FinishedExcessiveExternalForce",
                "The force/torque sensor detected excessive external force. " +
                "Probably something hit to the gripper. " +
                "Please identify where it hit and look at logs to check if the sensor was overloaded.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedToolNotAvailable = new RobotSortCode(22, "FinishedToolNotAvailable",
                "The specified tool was not defined on the robot or the robot failed to tool change correctly, " +
                "so could not enable the correct tool.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedNoMoreDestOutOfOrder = new RobotSortCode(23, "FinishedNoMoreDestOutOfOrder",
                "Unable to place the grasp targets specified by [orderNumber].", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPickContainerNotEmpty = new RobotSortCode(32, "FinishedPickContainerNotEmpty",
            "Expected pick container to be empty after last pick before departure, but was not empty after detection.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedPlaceContainerNotEmpty = new RobotSortCode(48, "FinishedPlaceContainerNotEmpty",
            "Expected place container to be empty upon arrival before first place, but was not empty after the first detection."
            , FailReason.DestToteError);
        
        public static readonly RobotSortCode FinishedUnexpectedPlacement = new RobotSortCode(49, "FinishedUnexpectedPlacement",
            "There is an unexpected placement in the place container, perhaps object dropped, " +
            "or was not placed correctly, and system cannot recover.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedGripperNotDefined = new RobotSortCode(135, "FinishedGripperNotDefined",
            "The gripperControlInfo for the gripper is not defined, so do not know how to control the gripper.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedGrabbingPartPositionBalanceError = new RobotSortCode(136, "FinishedGrabbingPartPositionBalanceError",
            "Position of the grabbing part is imbalanced according to sensor values.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedCycleStopped = new RobotSortCode(257, "FinishedCycleStopped",
            "[stopOrderCycle] command received", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedImmediatelyStopped = new RobotSortCode(258, "FinishedImmediatelyStopped",
            "Immediate stop executed as a [stopImmediately] command was received etc.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedInterlockWithLocation = new RobotSortCode(259, "FinishedInterlockWithLocation",
            "The got into interlock error because Prohibited signal of a Location is ON while robot was " +
            "in or about to go into the Location.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedEStopped = new RobotSortCode(260, "FinishedEStopped",
            "The system got E-Stop from either user, external device, " +
            "or other connected E-Stop switch.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedExecutionStopped = new RobotSortCode(261, "FinishedExecutionStopped",
                "The execution stopped because of user-triggered or external-triggered event or because of " +
                "error cleanup or other internal logic. Robot will try to recover to a good known position or go " +
                "to finish position before finishing the cycle.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedUnexpectedCancelCommand = new RobotSortCode(262, "FinishedUnexpectedCancelCommand",
                "The execution stopped because of unexpected cancel command from the robot bridge side. " +
                "Robot will not try to recover to home.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedImmediatelyStoppedByUI = new RobotSortCode(263, "FinishedImmediatelyStoppedByUI",
                "The execution stopped because of user-triggered even from the UI.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedImmediatelyStoppedByControlModeSwitch = new RobotSortCode(264, "FinishedImmediatelyStoppedByControlModeSwitch",
                "The execution stopped because of control mode switch from the Pendant or external auto enable signals.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedPlanningError = new RobotSortCode(4096, "FinishedPlanningError",
            "The motion planning could not be completed for reasons not classified below with 0x100X", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedNoValidGrasp = new RobotSortCode(4097, "FinishedNoValidGrasp",
            "No valid grasp positions were specified in the motion planning", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedNoValidDest = new RobotSortCode(4098, "FinishedNoValidDest",
            "No valid destination positions were specified in the motion planning", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedNoValidGraspDestPair = new RobotSortCode(4099, "FinishedNoValidGraspDestPair",
            "No valid grasp-destination pair positions were specified in the motion planning", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedNoValidPath = new RobotSortCode(4100, "FinishedNoValidPath",
            "No valid path from the grasp position to the destination position could be created for" +
            " the motion planning", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedNoValidTargets = new RobotSortCode(4101, "FinishedNoValidTargets",
            "Out of the detected targets, there were no targets that pass the overlap or pickable " +
            "conditions to be considered for grasping.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedComputePlanFailure = new RobotSortCode(4103, "FinishedComputePlanFailure",
            "Found valid grasps and dests for a target, but could not compute the full plan.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedCannotGenerateGraspingModel = new RobotSortCode(4104, "FinishedCannotGenerateGraspingModel",
            "Could not generate a valid grasp model for the current target", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedNotifySlaveTimeout = new RobotSortCode(4105, "FinishedNotifySlaveTimeout",
            "Interernal communication between slaves timedout, most likely because other " +
            "slave was busy and could not accept commands.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedPlanningServerStateError = new RobotSortCode(4224, "FinishedPlanningServerStateError",
                "Planning server had an incorrect state (like cycleIndex mismatch) and " +
                "therefore it could not execute the command.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedNoValidBarcodeScan = new RobotSortCode(4352, "FinishedNoValidBarcodeScan",
                "Could not find a scan solution that shows all of target barcodes to the scanners, " +
                "even though grasp and dest were found.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedBarcodeMismatch = new RobotSortCode(4353, "FinishedBarcodeMismatch",
                "Barcode was scanned, but it did not match the expected value.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedBarcodeScanFailed = new RobotSortCode(4354, "FinishedBarcodeScanFailed",
                "Barcode reader failed to scan.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedContainerNotDetected = new RobotSortCode(8193, "FinishedContainerNotDetected",
            "The pick container could not be detected", FailReason.SourceToteError);
        
        public static readonly RobotSortCode FinishedPlaceContainerNotDetected = new RobotSortCode(8194, "FinishedPlaceContainerNotDetected",
            "The place container could not be detected", FailReason.DestToteError);
        
        public static readonly RobotSortCode FinishedBadExpectedDetectionHeight = new RobotSortCode(8195, "FinishedBadExpectedDetectionHeight",
            "The top of the boxes are detected and are assumed to be stacked on top of each other so that " +
            "all layers have preditable z values. When the layer has a z value that is not consistent with" +
            " the current object, cycle will return this finish code.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedUnexpectedMeasuredTargetSize = new RobotSortCode(8196, "FinishedUnexpectedMeasuredTargetSize",
            "Robot has picked up a target object and put it close to a sensor for measurement, " +
            "and the sensor returned values that do not match the expected size of the target object size. " +
            "Most likely this is due to misdetection results, or because the target object deformed.", FailReason.SkuError);
        
        public static readonly RobotSortCode FinishedUnexpectedMeasuredTargetMassProperties = new RobotSortCode(8197, "FinishedUnexpectedMeasuredTargetMassProperties",
            "Robot has picked up a target object and measure it by a force/torque sensor," +
            " and the sensor returned values that do not match the expected mass or center of mass of" +
            " the target. Most likely this is due to mis-detection results, or bad registration..", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedInvalidOrderNumber = new RobotSortCode(12288, "FinishedInvalidOrderNumber",
            "[orderNumber] is invalid. It must be greater than 0.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedInvalidPickContainerType = new RobotSortCode(12289, "FinishedInvalidPickContainerType",
            "[orderPickContainerType] is invalid. It must be empty string or registered container name" +
            " in database.", FailReason.SourceToteError);
        
        public static readonly RobotSortCode FinishedInvalidPlaceContainerType = new RobotSortCode(12290, "FinishedInvalidPlaceContainerType",
            "[orderPlaceContainerType] is invalid. It must be empty string or registered container name" +
            " in database.", FailReason.DestToteError);
        
        public static readonly RobotSortCode FinishedInvalidOrderNumPartBarcodes = new RobotSortCode(12291, "FinishedInvalidOrderNumPartBarcodes",
            "[orderNumPartBarcodes] is invalid. It must be greater than 0.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackValidationFailed = new RobotSortCode(16384, "FinishedPackValidationFailed",
                "The computed pack formation cannot be executed for the currently detected container.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackingError = new RobotSortCode(16385, "FinishedPackingError",
                "Packing generic error.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackingPartTypeMismatch = new RobotSortCode(16386, "FinishedPackingPartTypeMismatch",
                "Packing PartType Mismatch of Order and Pack.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedPackingPartIndexMismatch = new RobotSortCode(16387, "FinishedPackingPartIndexMismatch",
                "Packing PartIndex is invalid.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackingInvalidNumParts = new RobotSortCode(16388, "FinishedPackingInvalidNumParts",
                "Packing Invalid number of parts.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackingCannotReadIO = new RobotSortCode(16389, "FinishedPackingCannotReadIO",
                "Packing Cannot Read Packing Data from IO.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackingNoCameras = new RobotSortCode(16390, "FinishedPackingNoCameras",
                "Packing no cameras specified.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackingCameraInvalid = new RobotSortCode(16391, "FinishedPackingCameraInvalid",
                "Packing specified cameras are invalid.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedPackingLongIdle = new RobotSortCode(16392, "FinishedPackingLongIdle",
            "Packing has long idle, perhaps not responding.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackingFailedToValidate = new RobotSortCode(16393, "FinishedPackingFailedToValidate",
                "Packing failed to validte the pack.", FailReason.OtherPickError);
            
        public static readonly RobotSortCode FinishedPackingIndexOutOfOrder = new RobotSortCode(16394, "FinishedPackingIndexOutOfOrder",
                "When getting a inputPartIndex for a pack, has to start at 1 and go to the end in order." +
                " If there are any indices out of order, then will throw this finish code to tell PLC to stop", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedITLExecutionError = new RobotSortCode(20480, "FinishedITLExecutionError",
                "Failed to execute ITL program during pick and place.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedPlanningLostConnection = new RobotSortCode(65520, "FinishedPlanningLostConnection",
                "Failed to execute ITL program during pick and place.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedResponsePlanningError = new RobotSortCode(65281, "FinishedResponsePlanningError",
                "A planing error in the response executor thread happened that system could not recover from. " +
                "Perhaps collision avoidance or tool constraints got in the way.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedPartWeightIsTooSmall = new RobotSortCode(65524, "FinishedPartWeightIsTooSmall",
                "Part weight sent from PLC is smaller than the error threshold.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedResponseExecutorError = new RobotSortCode(65525, "FinishedResponseExecutorError",
                "An error in the response executor thread happened that system could not recover from.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedExecutorError = new RobotSortCode(65526, "FinishedExecutorError",
                "An error in the executor thread happened that system could not recover from.", FailReason.OtherPickError);

        public static readonly RobotSortCode FinishedCannotComputeFinishPlan = new RobotSortCode(65527, "FinishedCannotComputeFinishPlan",
            "Once robot has picked up all targets, it could not plan to the designated finish position.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedUnknownReasonNoError = new RobotSortCode(65528, "FinishedUnknownReasonNoError",
            "Cycle finished for an unknown reason, but no error occurred.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedCannotGetState = new RobotSortCode(65529, "FinishedCannotGetState",
            "Error when the binpicking state cannot be received on cycle ending.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedCycleStopCanceled = new RobotSortCode(65530, "FinishedCycleStopCanceled",
            "Error when the StopPickPlaceThread on planning server ends in an error and system does not know" +
            " the status of the exit condition. should never happen.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedDropOffIsOn = new RobotSortCode(65531, "FinishedDropOffIsOn",
            "Error when [isDroppedOff] is 1 during a new cycle.", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedBadPartType = new RobotSortCode(65533, "FinishedBadPartType",
            "There was some problem with the piece type, such as an unregistered piece being specified", FailReason.SkuError);
        
        public static readonly RobotSortCode FinishedBadOrderCyclePrecondition = new RobotSortCode(65534, "FinishedBadOrderCyclePrecondition",
            "[startOrderCycle] cannot execute for some reason", FailReason.OtherPickError);
        
        public static readonly RobotSortCode FinishedGenericError = new RobotSortCode(65535, "FinishedGenericError",
            "Probable temporary problem not listed above", FailReason.OtherPickError);
        
        private static readonly List<RobotSortCode> _sortCodes = new List<RobotSortCode>
        {
            FinishedNoMoreTargets,
            FinishedNoMoreTargetsNotEmpty,
            FinishedNoMoreDest,
            FinishedNoEnvironmentUpdate,
            FinishedDropTargetFailure,
            FinishedTooManyPickFailures,
            FinishedRobotExecutionError,
            FinishedNoDestObstacles,
            FinishedStopDueToTorqueLimit,
            FinishedGripperExecutionError,
            FinishedCannotRecoverWhileGrabbingTarget,
            FinishedCycleStopped,
            FinishedImmediatelyStopped,
            FinishedInterlockWithLocation,
            FinishedPlanningError,
            FinishedNoValidGrasp,
            FinishedNoValidDest,
            FinishedNoValidGraspDestPair,
            FinishedNoValidPath,
            FinishedNoValidTargets,
            FinishedNoValidBarcodeScan,
            FinishedComputePlanFailure,
            FinishedCannotGenerateGraspingModel,
            FinishedContainerNotDetected,
            FinishedPlaceContainerNotDetected,
            FinishedBadExpectedDetectionHeight,
            FinishedUnexpectedMeasuredTargetSize,
            FinishedInvalidOrderNumber,
            FinishedInvalidPickContainerType,
            FinishedInvalidPlaceContainerType,
            FinishedInvalidOrderNumPartBarcodes,
            FinishedCannotComputeFinishPlan,
            FinishedUnknownReasonNoError,
            FinishedCannotGetState,
            FinishedCycleStopCanceled,
            FinishedBadPartType,
            FinishedBadOrderCyclePrecondition,
            FinishedGenericError,
            FinishedCannotRecoverWhileIntermediateCycles,
            FinishedDropOffIsOn,
            FinishedExecutorError,
            FinishedResponseExecutorError,
            FinishedPartWeightIsTooSmall,
            FinishedResponsePlanningError,
            FinishedPlanningLostConnection,
            FinishedITLExecutionError,
            FinishedPackingIndexOutOfOrder,
            FinishedPackingFailedToValidate,
            FinishedPackingLongIdle,
            FinishedPackingCameraInvalid,
            FinishedPackingNoCameras,
            FinishedPackingCannotReadIO,
            FinishedPackingInvalidNumParts,
            FinishedPackingPartIndexMismatch,
            FinishedPackingPartTypeMismatch,
            FinishedPackingError,
            FinishedPackValidationFailed,
            FinishedUnexpectedMeasuredTargetMassProperties,
            FinishedBarcodeScanFailed,
            FinishedBarcodeMismatch,
            FinishedPlanningServerStateError,
            FinishedNotifySlaveTimeout,
            FinishedImmediatelyStoppedByControlModeSwitch,
            FinishedImmediatelyStoppedByUI,
            FinishedUnexpectedCancelCommand,
            FinishedExecutionStopped,
            FinishedEStopped,
            FinishedGrabbingPartPositionBalanceError,
            FinishedGripperNotDefined,
            FinishedUnexpectedPlacement,
            FinishedPlaceContainerNotEmpty,
            FinishedPickContainerNotEmpty,
            FinishedNoMoreDestOutOfOrder,
            FinishedToolNotAvailable,
            FinishedExcessiveExternalForce,
            FinishedSourceVerificationIsLate,
            FinishedCannotReturnPartToContainer,
            FinishedForceTorqueCalibrationError,
            FinishedExecutionError,
            FinishedGripperPositionNotReached,
            FinishedBadIOConditions,
            FinishedCannotRecover, 
        };
    }
}