using System.Collections.Generic;
using System.Linq;
using RcsLogic.Models;

namespace RcsLogic
{
    public class CraneTransfersPlan
    {
        private readonly Dictionary<CraneState.Shelf, Transfer> _transfersForShelves;

        public CraneTransfersPlan(Dictionary<CraneState.Shelf, Transfer> transfersForShelves)
        {
            _transfersForShelves = transfersForShelves;
        }

        public List<CraneState.Shelf> GetShelves()
        {
            return _transfersForShelves.Keys.ToList();
        }

        public Transfer GetRequestForShelfOrDefault(CraneState.Shelf first)
        {
            return _transfersForShelves.GetValueOrDefault(first);
        }

        public bool IsEmpty()
        {
            return _transfersForShelves.Count == 0;
        }

        public override string ToString()
        {
            var formattableString = $"{nameof(_transfersForShelves)}: ";
            foreach (var transfersForShelf in _transfersForShelves)
            {
                formattableString += " " + transfersForShelf.Key + ":" + transfersForShelf.Value + "; ";
            }

            return formattableString;
        }
    }
}