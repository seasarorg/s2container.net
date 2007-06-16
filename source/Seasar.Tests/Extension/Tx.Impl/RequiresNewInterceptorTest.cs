#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

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
    public class RequiresNewInterceptorTest : S2FrameworkTestCaseBase
    {
        private ITxTest _txTest;

        public ITxTest TxTest
        {
            get { return _txTest; }
            set { _txTest = value; }
        }

        public RequiresNewInterceptorTest()
        {
            FileInfo info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
            base.Container = S2ContainerFactory.Create(base.ConvertPath("RequiresNewInterceptorTest.dicon"));
            base.Container.Init();
            TxTest = base.GetComponent(typeof(ITxTest)) as ITxTest;
        }

        [Test]
        public void StartTx()
        {
            Assert.AreEqual(true, TxTest.IsInTransaction());
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
                TxTest.throwException();
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
                    TxTest.throwException();
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
                    Assert.IsTrue(tx.TransactionInformation.Status == TransactionStatus.Active, "0");
                    Assert.IsTrue(tx2.TransactionInformation.Status == TransactionStatus.Active, "1");
                    scope2.Complete();
                }

                Assert.IsTrue(tx.TransactionInformation.Status == TransactionStatus.Active, "2");
                scope.Complete();
            }
        }
    }
}
