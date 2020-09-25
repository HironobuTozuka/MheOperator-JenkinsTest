using System;
using Common.Models;

namespace RcsLogic.RcsController.Exceptions
{
    public class NoDeviceCanHandleTransferDoneException : Exception
    {
        public ToteTransferRequestDoneModel RequestDoneModel { get; set; }
    }
}