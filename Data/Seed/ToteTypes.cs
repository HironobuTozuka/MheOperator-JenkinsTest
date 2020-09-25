using System.Collections.Generic;
using Common.Models;
using Common.Models.Tote;

namespace Data.Seed
{
    public static class ToteTypes
    {
        public static List<ToteType> Seed()
        {
            return new List<ToteType>()
            {
                new ToteType(ToteHeight.low, TotePartitioning.bipartite){id = 1},
                new ToteType(ToteHeight.low, TotePartitioning.tripartite){id = 2},
                new ToteType(ToteHeight.high, TotePartitioning.bipartite){id = 3},
                new ToteType(ToteHeight.high, TotePartitioning.tripartite){id = 4},
                new ToteType(ToteHeight.unknown, TotePartitioning.unknown){id = 5},
            };
            
        }
    }
}