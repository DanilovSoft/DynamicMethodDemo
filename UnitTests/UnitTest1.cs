using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Contract;
using DynamicMethodsLib;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var testClass = new TestClass();
            ITest proxy = TypeProxy.Create<ITest, DynamicMethodProxy<TestClass>>(testClass);

            {
                string refStr = "";
                int result = proxy.Sum("CopyMe", 1, 2, out int outArg, ref refStr);
                Assert.AreEqual(3, result);
                Assert.AreEqual(3, outArg);
                Assert.AreEqual("CopyMe", refStr);
            }

            Assert.AreEqual("qwerty", proxy.Dummy("qwerty"));
            Assert.AreEqual("qwerty", proxy.RetObject("qwerty"));
            proxy.Empty();
            Assert.AreEqual(123, proxy.DummyValueArg(123));
            Assert.AreEqual(long.MaxValue, proxy.DummyValueArg(long.MaxValue));
        }
    }
}
