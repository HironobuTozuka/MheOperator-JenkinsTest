using System;
using System.Threading.Tasks;
using Common.Models.Gate;
using Common.Models.Plc;
using PlcCommunicationService.SystemPlc.Models;

namespace PlcCommunicationService.SystemPlc
{
    public class GatePlcActionExecutor : IPlcActionExecutor
    {
        public async Task ExecuteAction(OutSignals outSignals, PlcAction plcAction)
        {
            if (plcAction.ActionType != PlcActionType.Gate) return;

            var gate = plcAction.Parameters.GateDescription;
            var gateAction = plcAction.Parameters.GateAction;

            if (gate.slotIndexes != null)
            {
                gate.slotIndexes.ForEach(async slot =>
                    await outSignals.ExternalIO.OpenGate(gate.gateId, gateAction == GateAction.Open, slot));
            }
            else
            {
                await outSignals.ExternalIO.OpenGate(gate.gateId, gateAction == GateAction.Open);
            }
        }
    }
}