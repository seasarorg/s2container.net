#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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

using System;
using MbUnit.Framework;
using Seasar.Framework.Beans;
using Seasar.Framework.Beans.Factory;
using Seasar.Framework.Log;

namespace Seasar.Tests.Framework.Beans.Factory
{
    [TestFixture]
    public class BeanDescFactoryTest
    {
        private readonly Logger _log = Logger.GetLogger(typeof (BeanDescFactoryTest));

        [SetUp]
        public void SetUp()
        {
            BeanDescFactory.Clear();
        }

        [Test]
        public void TestGetBeanDesc()
        {
            //  BeanDescの初取得時と２回目以降で実行してみる
            _log.Debug("1st");
            {
                ExecuteAndAssertGetBeanDesc();
            }
            _log.Debug("2nd");
            {
                ExecuteAndAssertGetBeanDesc();
            }
        }

        #region テスト用補助メソッド

        /// <summary>
        /// GetBeanDescメソッドの実行と検証
        /// </summary>
        private static void ExecuteAndAssertGetBeanDesc()
        {
            Type testType = typeof(BeanDescFactory);
            IBeanDesc actual = BeanDescFactory.GetBeanDesc(testType);

            Assert.IsNotNull(actual);
            Assert.AreEqual(testType, actual.BeanType);
            Assert.IsTrue(actual.IsAssignableFrom(testType));
            Assert.IsFalse(actual.IsNullable());
        }

        #endregion
    }
}
