using System;
using System.Threading.Tasks;
using Common.Models.Led;
using Common.Models.Plc;
using PlcCommunicationService.SystemPlc.Models;

namespace PlcCommunicationService.SystemPlc
{
    public class LedPlcActionExecutor : IPlcActionExecutor
    {
        public async Task ExecuteAction(OutSignals outSignals, PlcAction plcAction)
        {
            if (plcAction.ActionType != PlcActionType.Led) return;

            var ledState = plcAction.Parameters.LedState;

            switch (plcAction.Parameters.LedId)
            {
                case LedId.Led1:
                    await outSignals.ExternalIO.Led1(ledState == LedState.On);
                    break;
                case LedId.Led2:
                    await outSignals.ExternalIO.Led2(ledState == LedState.On);
                    break;
                case LedId.Led3:
                    await outSignals.ExternalIO.Led3(ledState == LedState.On);
                    break;
                default:
                    throw new Exception("Not implemented Led ID");
            }
        }
    }
}