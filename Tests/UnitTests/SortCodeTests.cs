using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    public class SortCodeTests
    {
        [Test]
        public void TestSortCodes()
        {
            var code1 = RcsLogic.Robot.RobotSortCodes.Get(1);
            var code2 = RcsLogic.Robot.RobotSortCodes.Get(2);
            var code3 = RcsLogic.Robot.RobotSortCodes.Get(15);
            if(code1!= null && code2 != null && code3 != null) Assert.Pass();
            Assert.Fail();
        }
    }
}