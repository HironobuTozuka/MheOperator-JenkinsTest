using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Linq;
using Common.JsonConverters;
using Microsoft.Extensions.Caching.Memory;
using Common.Models;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Tote;
using Common.Models.Transfer;
using Data;
using Newtonsoft.Json;
using RcsLogic.Models;

namespace Tests
{
    public class SimpleVoToStringConvertTest
    {


        [Test]
        public void TestRoutingFromCNV1_5ToORDER1()
        {
            var obj = new ToteTransferRequestModel()
            {
                Id = new TransferId("Lorem_Ipsum"),
                DestLocationId = "CNV1",
                MaxAcc = 10,
                SourceLocationId = "CNV1",
                ToteBarcode = "00000001",
                ToteType = new RequestToteType(ToteHeight.high, TotePartitioning.bipartite),
                Weight = 10
            };
            var converter = new JsonSimpleVoToStringConverter();
            var canConvert = converter.CanConvert(obj.Id.GetType());
            var serialisedObj = JsonConvert.SerializeObject(obj);
            Console.WriteLine(serialisedObj);
            var deserializedObj = JsonConvert.DeserializeObject(serialisedObj, typeof(ToteTransferRequestModel));
            if (deserializedObj is ToteTransferRequestModel castedObj)
            {
                if (canConvert
                    && castedObj.Id.Equals(obj.Id) 
                    && castedObj.Weight.Equals(obj.Weight) 
                    && castedObj.MaxAcc.Equals(obj.MaxAcc)
                    && castedObj.ToteBarcode.Equals(obj.ToteBarcode)
                    && castedObj.ToteType.Equals(obj.ToteType)
                    && castedObj.DestLocationId.Equals(obj.DestLocationId)
                    && castedObj.SourceLocationId.Equals(obj.SourceLocationId))
                    Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void TestTransferDone()
        {
            var serialisedObj =
                "{" +
                    "\"transferRequest1Done\":" +
                    "{" +
                        "\"requestId\":\"3d424320-3fee-4903-95a2-f90aa515f8b8\"," +
                        "\"sourceLocationId\":\"LOAD1_2\"," +
                        "\"sourceToteBarcode\":\"00000007\"," +
                        "\"requestedDestLocationId\":\"LOAD1_3\"," +
                        "\"actualDestLocationId\":\"LOAD1_3\"," +
                        "\"sortCode\":\"1\"" +
                    "}," +
                    "\"transferRequest2Done\":null" +
                "}";
            var deserializedObj = JsonConvert.DeserializeObject(serialisedObj, typeof(TransferRequestDoneModel));
            if (deserializedObj is TransferRequestDoneModel castedObj)
            {
                if (castedObj.transferRequest1Done.requestId.Id.Equals("3d424320-3fee-4903-95a2-f90aa515f8b8")
                    && castedObj.transferRequest1Done.sourceLocationId.Equals("LOAD1_2")
                    && castedObj.transferRequest1Done.sourceToteBarcode.Equals("00000007")
                    && castedObj.transferRequest1Done.requestedDestLocationId.Equals("LOAD1_3")
                    && castedObj.transferRequest1Done.actualDestLocationId.Equals("LOAD1_3")
                    && castedObj.transferRequest1Done.sortCode == 1)
                    Assert.Pass();
            }
            Assert.Fail();
        }
    }
}