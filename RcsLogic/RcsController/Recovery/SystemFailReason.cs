namespace RcsLogic.RcsController.Recovery
{
    public enum SystemFailReason
    {
        NoRead,
        ToteOnPlatform,
        Pick,
        Place,
        PlaceLocationOccupied,
        Overfill,
        DeviceBusy,
        Other,
        NoTote
    }
}