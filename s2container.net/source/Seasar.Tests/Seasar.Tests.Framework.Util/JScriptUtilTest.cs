using System;
using System.Text;
using MbUnit.Framework;
using Seasar.Framework.Util;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Impl;
using System.Collections;

namespace Seasar.Tests.Framework.Util
{
    [TestFixture]
    public class JScriptUtilTest
    {
        [Test]
        public void TestString()
        {
            Assert.AreEqual("•¶Žš—ñ", JScriptUtil.Evaluate("\"•¶Žš—ñ\"", null));
        }

        [Test]
        public void TestInt32()
        {
            Assert.AreEqual(7, JScriptUtil.Evaluate("7", null));
        }

        [Test]
        public void TestFloat()
        {
            Assert.AreEqual(3.14, JScriptUtil.Evaluate("3.14", null));
        }

        [Test]
        public void TestContainer()
        {
            IS2Container container = new S2ContainerImpl();
            IComponentDef componentDef = new ComponentDefImpl(typeof(Hashtable), "Message");
            IInitMethodDef initMethodDef = new InitMethodDefImpl("Add");
            initMethodDef.AddArgDef(new ArgDefImpl("Hello"));
            initMethodDef.AddArgDef(new ArgDefImpl("‚±‚ñ‚É‚¿‚Í"));
            componentDef.AddInitMethodDef(initMethodDef);
            container.Register(componentDef);

            Assert.AreEqual("‚±‚ñ‚É‚¿‚Í", JScriptUtil.Evaluate(
                "container.GetComponent(\"Message\")[\"Hello\"]", container));
        }

        [Test]
        public void TestOut()
        {
            string outStr = "Out";
            Hashtable hashTable = new Hashtable();
            hashTable["out"] = outStr;

            Assert.AreEqual("Out", JScriptUtil.Evaluate("out", hashTable, null));
        }

        [Test]
        public void TestErr()
        {
            string errStr = "Error";
            Hashtable hashTable = new Hashtable();
            hashTable["err"] = errStr;

            Assert.AreEqual("Error", JScriptUtil.Evaluate("err", hashTable, null));
        }

        [Test]
        public void TestSelf()
        {
            string selfStr = "Self";
            Hashtable hashTable = new Hashtable();
            hashTable["self"] = selfStr;

            Assert.AreEqual("Self", JScriptUtil.Evaluate("self", hashTable, null));
        }

        [Test]
        public void TestAppSettings()
        {
            Assert.AreEqual("Hello", JScriptUtil.Evaluate(
                "appSettings['message']", null));
        }
    }
}
