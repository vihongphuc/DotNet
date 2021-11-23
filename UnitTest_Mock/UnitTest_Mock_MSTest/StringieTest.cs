using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest_Mock.Services;

namespace UnitTest_Mock_MSTest
{
    [TestClass]
    public class StringieTest
    {
        [TestMethod]
        public void Remove_ASubstring_RemovesThatSubstring()
        {
            string str = "Hello, world!";

            string transformed = str.Remove();

            Assert.AreEqual(0, transformed.Length);
        }

        [TestMethod]
        public void Remove_NoParameters_ReturnsEmpty()
        {
            string str = "Hello, world!";

            string transformed = str.Remove(0,5);

            var position = str.IndexOf("Hello");
            var expected = str.Substring(position + 5);
            Assert.AreEqual(expected, transformed);
        }
    }
}
