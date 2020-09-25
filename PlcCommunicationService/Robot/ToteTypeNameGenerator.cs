using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;
using Common.Models.Tote;

namespace PlcCommunicationService.Robot
{
    public static class ToteTypeNameGenerator
    {
        public static string Generate(ToteType toteType, int slotId)
        {
            string toteTypeName = "";
            switch (toteType.toteHeight)
            {
                case ToteHeight.high:
                    toteTypeName = "H";
                    break;
                case ToteHeight.low:
                    toteTypeName = "L";
                    break;
                default:
                    throw new Exception("Tote type not implemented!!");
            }

            switch (toteType.totePartitioning)
            {
                case TotePartitioning.bipartite:
                    toteTypeName += "2";
                    break;
                case TotePartitioning.tripartite:
                    toteTypeName += "3";
                    break;
                default:
                    throw new Exception("Tote type not implemented!!");
            }
            toteTypeName += slotId.ToString();
            return toteTypeName;
        }
    }
}
