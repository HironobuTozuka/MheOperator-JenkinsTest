using System;
using System.Collections.Generic;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using RcsLogic.Robot;

namespace RcsLogic.Models
{
    public class SlotsToExpose
    {
        public Location Location { get; private set; }
        public Tote Tote { get; private set; }
        public List<int> Slots { get; private set; }

        public SlotsToExpose(DeliverTask deliverTask, Location location, ToteRotation rotation, Tote tote)
        {
            Location = location;
            Slots = GetSlotsToExpose(deliverTask, rotation);
            Tote = tote;
        }
        
        private List<int> GetSlotsToExpose(DeliverTask deliverTask, ToteRotation rotation)
        {
            List<int> slotsToExposeRotated = new List<int>();
            if (deliverTask?.slots != null)
            {
                foreach (var slot in deliverTask.slots)
                {
                    if (rotation == ToteRotation.normal)
                    {
                        if (slot == 0) slotsToExposeRotated.Add(1);
                        if (slot == 1) slotsToExposeRotated.Add(0);
                    }
                    else
                    {
                        slotsToExposeRotated.Add(slot);
                    }
                }
            }

            return slotsToExposeRotated;
        }

        public override string ToString()
        {
            return $"{nameof(Location)}: {Location}, {nameof(Tote)}: {Tote}, {nameof(Slots)}: {String.Join(",", Slots)}";
        }
    }
}