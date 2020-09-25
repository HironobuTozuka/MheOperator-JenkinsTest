using System;
using RcsLogic.Models;
using RcsLogic.Robot;

namespace RcsLogic.RcsController.Exceptions
{
    public class NoDeviceCanHandlePickingPreparationException : Exception
    {
        public PrepareForPicking PrepareForPicking { get; set; }
    }
}