using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;
using Common.Models.Plc;
using Common.Models.Tote;

namespace PlcCommunicationService.SystemPlc
{
    public static class ToteTypeName
    {
        public static string Generate(ToteType toteType)
        {
            return Generate(toteType.toteHeight, toteType.totePartitioning);
        }
        
        public static string Generate(RequestToteType toteType)
        {
            return Generate(toteType.ToteHeight, toteType.TotePartitioning);
        }
        public static string Generate(ToteHeight toteHeight, TotePartitioning totePartitioning)
        {
            string toteTypeName = "";
            switch (toteHeight)
            {
                case ToteHeight.high:
                    toteTypeName = "H";
                    break;
                case ToteHeight.low:
                    toteTypeName = "L";
                    break;
                case ToteHeight.unknown:
                    toteTypeName = "UN";
                    break;
                default:
                    throw new Exception("Tote type not implemented!!");
            }

            switch (totePartitioning)
            {
                case TotePartitioning.bipartite:
                    toteTypeName += "2";
                    break;
                case TotePartitioning.tripartite:
                    toteTypeName += "3";
                    break;
                case TotePartitioning.unknown:
                    toteTypeName = "UN";
                    break;
                default:
                    throw new Exception("Tote type not implemented!!");
            }

            return toteTypeName;
        }

        public static RequestToteType GetToteType (string toteName)
        {
            TotePartitioning totePartitioning;
            ToteHeight toteHeight;

            if (toteName == null || toteName.Length < 2)
            {
                return new RequestToteType(ToteHeight.unknown, TotePartitioning.unknown); 
            }

            switch (toteName[0])
            {
                case 'H':
                    toteHeight = ToteHeight.high;
                    break;
                case 'L':
                    toteHeight = ToteHeight.low;
                    break;
                default:
                    toteHeight = ToteHeight.unknown;
                    break;
            }

            switch (toteName[1])
            {
                case '2':
                    totePartitioning = TotePartitioning.bipartite;
                    break;
                case '3':
                    totePartitioning = TotePartitioning.tripartite;
                    break;
                default:
                    totePartitioning = TotePartitioning.unknown;
                    break;
            }

            return new RequestToteType(toteHeight, totePartitioning);
        }
    }
}
