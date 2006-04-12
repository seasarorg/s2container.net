using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Transactions;

using log4net;
using log4net.Config;
using log4net.Util;

using NUnit.Framework;

using Seasar.Framework.Container.Factory;
using Seasar.Framework.Unit;

namespace Seasar.Tests.Extension.Tx.Impl
{
    [TestFixture]
	public class RequiredInterceptorTest : S2FrameworkTestCaseBase
	{
        private ITxTest txTest;

        public ITxTest TxTest
        {
            get { return txTest; }
            set { txTest = value; }
        }

        public RequiredInterceptorTest()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
            base.Container = S2ContainerFactory.Create(base.ConvertPath("RequiredInterceptorTest.dicon"));
            base.Container.Init();
            this.TxTest = base.GetComponent(typeof(ITxTest)) as ITxTest;
        }

        [Test]
        public void StartTx()
        {
            Assert.AreEqual(true, this.TxTest.IsInTransaction());
        }

        [Test]
        public void StartTxInTx()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Transaction tx = Transaction.Current;
                Assert.AreEqual(tx.TransactionInformation.LocalIdentifier,TxTest.TxId);
            }
        }

        [Test]
        public void ThrowException()
        {
            try
            {
                this.TxTest.throwException();
                Assert.Fail();
            }
            catch (TxException e)
            {
                Assert.IsTrue(e.WasInTx);
            }
        }
    
        [Test]
        public void ThrowExceptionInTx()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Transaction tx = Transaction.Current;
                try
                {
                    this.TxTest.throwException();
                    Assert.Fail();
                }
                catch (TxException e)
                {
                    Assert.IsTrue(e.WasInTx);
                    Assert.AreEqual(tx.TransactionInformation.LocalIdentifier, e.TxId);
                }

                Assert.IsTrue(tx.TransactionInformation.Status == TransactionStatus.Aborted);
            }

        }
    }
}
