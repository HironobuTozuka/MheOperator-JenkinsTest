using System;
using RcsLogic.Models;
using RcsLogic.Robot;

namespace RcsLogic.RcsController.Exceptions
{
    public class NoDeviceCanHandleSlotExposingException : Exception
    {
        public SlotsToExpose SlotsToExpose { get; set; }
    }
}