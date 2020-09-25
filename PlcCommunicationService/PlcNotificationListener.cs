using System;
using System.Threading.Tasks;
using Common.Models.Plc;
using Microsoft.Extensions.Logging;
using PlcCommunicationService.Models;

namespace PlcCommunicationService
{
    public abstract class PlcNotificationListener : IPlcReadNotificationListener
    {
        protected readonly ILogger Logger;
        private readonly IMoveRequestRedStateChangeListener _moveRequestRedStateChangeListener;

        protected PlcNotificationListener(ILogger logger,
            IMoveRequestRedStateChangeListener moveRequestRedStateChangeListener)
        {
            Logger = logger;
            _moveRequestRedStateChangeListener = moveRequestRedStateChangeListener;
        }

        protected abstract Task HandlePlcSpecificNotification(PlcReadNotification plcReadNotification, OpcClient opcClient);
        protected abstract void MoveRequestDone(IMoveRequestConf moveRequestConf);

        public async Task NotifyListener(PlcReadNotification plcReadNotification, OpcClient opcClient)
        {
            try
            {
                switch (plcReadNotification.Key)
                {
                    case "ReadMoveRequestConf":
                        Logger.LogInformation("ReadMoveRequestConf: {0}", plcReadNotification.Value);
                        if (await opcClient.InSignals.GetReadMoveRequestConfAsync())
                        {
                            try
                            {
                                var moveRequestConf = await opcClient.InSignals.GetMoveRequestConfAsync();
                                Logger.LogInformation("Move Request Conf: " + moveRequestConf.ToString());
                                MoveRequestDone(moveRequestConf);
                                await opcClient.OutSignals.SetMoveRequestConfRead(true);
                            }
                            catch
                            {
                                Logger.LogInformation("Unable to read MoveRequestConf from PLC");
                            }
                        }
                        else
                        {
                            await opcClient.OutSignals.SetMoveRequestConfRead(false);
                        }

                        break;

                    case "MoveRequestRead":
                        Logger.LogInformation("MoveRequestRead: {0}", plcReadNotification.Value.ToString());
                        if (await opcClient.InSignals.GetMoveRequestReadAsync())
                        {
                            _moveRequestRedStateChangeListener.NotifyListener(true);
                            await opcClient.OutSignals.SetReadMoveRequest(false);
                        }
                        else
                        {
                            _moveRequestRedStateChangeListener.NotifyListener(false);
                        }

                        break;
                    default:
                        await HandlePlcSpecificNotification(plcReadNotification, opcClient);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception while notifying listeners!");
            }
        }
    }
}