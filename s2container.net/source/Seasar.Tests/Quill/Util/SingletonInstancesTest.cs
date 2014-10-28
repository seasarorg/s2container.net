using NUnit.Framework;
using Seasar.Quill.Util;

namespace Seasar.Tests.Quill.Util
{
    [TestFixture]
    public class SingletonInstancesTest : QuillTestBase
    {
        [Test]
        public void TestGetInstance()
        {
            var instance1 = SingletonInstances.GetInstance<Hoge>();
            var instance2 = SingletonInstances.GetInstance<Hoge>();

            Assert.IsNotNull(instance1);
            Assert.AreEqual(typeof(Hoge), instance1.GetType());
            Assert.AreSame(instance1, instance2);
        }

        private class Hoge
        { }
    }
}
