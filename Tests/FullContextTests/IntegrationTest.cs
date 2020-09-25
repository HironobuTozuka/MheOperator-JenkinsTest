using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading;


namespace Tests
{
    public class IntegrationTest : IntegrationTestBase
    {

        [Test]
        public void InductTest()
        {
            //Thread.Sleep(2000);
            var plcTranslator = ServiceProvider.GetRequiredService<Common.IPlcService>() as PlcServiceMock;
            var dbContext = ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();

            plcTranslator.MockScanNotification("LOAD1_2", "00000001", dbContext.toteTypes.First());
            
            Assert.Pass();
        }

    }
}