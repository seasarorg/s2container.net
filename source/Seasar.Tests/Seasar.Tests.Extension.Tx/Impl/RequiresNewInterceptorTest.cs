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
	public class RequiresNewInterceptorTest : S2FrameworkTestCaseBase
	{
        private ITxTest txTest;

        public ITxTest TxTest
        {
            get { return txTest; }
            set { txTest = value; }
        }

        public RequiresNewInterceptorTest()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
            base.Container = S2ContainerFactory.Create(base.ConvertPath("RequiresNewInterceptorTest.dicon"));
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
                Assert.AreEqual(false, tx.TransactionInformation.LocalIdentifier.Equals(TxTest.TxId));
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
                    Assert.AreEqual(false, tx.TransactionInformation.LocalIdentifier.Equals(e.TxId));
                }
                Assert.IsTrue(tx.TransactionInformation.Status == TransactionStatus.Active);
            }
        }

        [Test]
        public void ScopeTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Transaction tx = Transaction.Current;
                using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.Required))
                {
                    Transaction tx2 = Transaction.Current;
                    Assert.IsTrue(tx.TransactionInformation.Status == TransactionStatus.Active,"0");
                    Assert.IsTrue(tx2.TransactionInformation.Status == TransactionStatus.Active, "1");
                }

                Assert.IsTrue(tx.TransactionInformation.Status == TransactionStatus.Active, "2");
                scope.Complete();
            }
        }
    }
}
