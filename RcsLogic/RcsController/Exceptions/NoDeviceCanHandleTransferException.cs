using System;
using RcsLogic.Models;

namespace RcsLogic.RcsController.Exceptions
{
    public class NoDeviceCanHandleTransferException : Exception
    {
        public Transfer Transfer { get; set; }
    }
}