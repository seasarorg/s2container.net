using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Transactions;

using log4net;
using log4net.Config;
using log4net.Util;

using MbUnit.Framework;

using Seasar.Framework.Container.Factory;
using Seasar.Framework.Unit;

namespace Seasar.Tests.Extension.Tx.Impl
{
    [TestFixture]
	public class NotSupportedInterceptorTest : S2FrameworkTestCaseBase
	{
        private ITxTest txTest;

        public ITxTest TxTest
        {
            get { return txTest; }
            set { txTest = value; }
        }

        public NotSupportedInterceptorTest()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
            base.Container = S2ContainerFactory.Create(base.ConvertPath("NotSupportedInterceptorTest.dicon"));
            base.Container.Init();
            this.TxTest = base.GetComponent(typeof(ITxTest)) as ITxTest;
        }

        [Test]
        public void StartTx()
        {
            Assert.AreEqual(false, this.TxTest.IsInTransaction());
        }

        [Test]
        public void StartTxInTx()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Transaction tx = Transaction.Current;
                Assert.IsNull(TxTest.TxId);
            }
        }
	}
}
