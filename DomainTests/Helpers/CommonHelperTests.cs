using Armin.Suitsupply.Domain.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainTests.Helpers
{
    [TestClass]
    public class CommonHelperTests
    {
        [TestMethod]
        public void GetRandomString()
        {
            var rnd1 = CommonHelper.GetRandomString(10, false, false);
            var rnd2 = CommonHelper.GetRandomString(10, false, false);

            Assert.AreEqual(rnd1.Length, 10);
            Assert.AreNotEqual(rnd1, rnd2);

            var  rnd3 = CommonHelper.GetRandomString(16, true, true);
            Assert.AreEqual(rnd3.Length, 16);

            var rnd4 = CommonHelper.GetRandomString(3, true, false);
            Assert.AreEqual(rnd4.Length, 3);
        }
    }
}
