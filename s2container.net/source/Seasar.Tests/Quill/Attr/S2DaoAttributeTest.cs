#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using Seasar.Dao.Attrs;
using Seasar.Extension.ADO;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Quill;
using Seasar.Quill.Attrs;
using Seasar.Quill.Dao.Impl;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Quill.Exception;
using Seasar.Quill.Unit;
using Seasar.Quill.Util;
using Seasar.Tests.Dao.Impl;

namespace Seasar.Tests.Quill.Attr
{
    [TestFixture]
    public class S2DaoAttributeTest : QuillTestCase
    {
        [SetUp]
        public void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void TestSetTypicalDaoSetting()
        {
            using (var container = new QuillContainer())
            {
                var attrs = typeof(IWithS2DaoAttr0).GetCustomAttributes(false);
                Assert.AreEqual(1, attrs.Length);
                Assert.IsTrue(attrs[0] is S2DaoAttribute);
            }
        }

        [Test, Quill]
        public void TestS2DaoAttribute_正常()
        {
            //  ## Arrange ##
            var container = new QuillContainer();
            {
                //  ## Act ##
                var component = container.GetComponent(typeof(IWithS2DaoAttr1));
                //  ## Assert ##
                var actual = (IWithS2DaoAttr1)component.GetComponentObject(
                    typeof(IWithS2DaoAttr1));
                Assert.IsNotNull(actual, "01");
                var emp = actual.GetEmployee();
                Assert.IsNotNull(emp, "02");
                Console.WriteLine(emp.Ename);
            }
            {
                //  ## Act ##
                var component = container.GetComponent(typeof(IWithS2DaoAttr2));
                //  ## Assert ##
                var actual = (IWithS2DaoAttr2)component.GetComponentObject(
                    typeof(IWithS2DaoAttr2));
                Assert.IsNotNull(actual, "11");
                var ret = actual.Hoge();
                Assert.AreEqual("InterceptorCalled", ret, "12");
            }
            {
                //  ## Act ##
                var component = container.GetComponent(typeof(IWithS2DaoMethod));
                //  ## Assert ##
                var actual = (IWithS2DaoMethod)component.GetComponentObject(
                    typeof(IWithS2DaoMethod));
                Assert.IsNotNull(actual, "21");
                var emp = actual.GetEmployee();
                Assert.IsNotNull(emp, "22");
                var ret = actual.Hoge2();
                Assert.AreEqual("InterceptorCalled", ret, "23");
            }
        }

        [Test]
        public void TestS2DaoAttribute_使用不可な型を指定()
        {
            //  ## Arrange ##
            var container = new QuillContainer();
            try
            {
                container.GetComponent(typeof(IIllegalStrring));
                Assert.Fail("1");
            }
            catch (QuillApplicationException)
            {
            }
        }

        [Test, Quill]
        public void TestDataSourceNameChange_Class()
        {
            var container = new QuillContainer();
            var actual =
                (IWithS2DaoDataSourceNameChange_Class)ComponentUtil.GetComponent(
                container, typeof(IWithS2DaoDataSourceNameChange_Class));
            var proxy =
                (SelectableDataSourceProxyWithDictionary)ComponentUtil.GetComponent(
                container, typeof(SelectableDataSourceProxyWithDictionary));
            const string START_NAME = "Start";
            proxy.SetDataSourceName(START_NAME);
            Assert.AreEqual(START_NAME, proxy.GetDataSourceName(), "現在のデータソース名確認");

            actual.GetEmployee();   //  データソースが変更されるInterceptorがかかっているはず

            Assert.AreEqual(DataSourceNameChangeTxSetting.TEST_DATASOURCE_NAME,
                proxy.GetDataSourceName(), "データソース名が切り替わっているはず");
        }

        [Test, Quill]
        public void TestDataSourceNameChange_Method()
        {
            var container = new QuillContainer();
            var actual =
                (IWithS2DaoDataSourceNameChange_Method)ComponentUtil.GetComponent(
                container, typeof(IWithS2DaoDataSourceNameChange_Method));
            var proxy =
                (SelectableDataSourceProxyWithDictionary)ComponentUtil.GetComponent(
                container, typeof(SelectableDataSourceProxyWithDictionary));
            const string START_NAME = "Start";
            proxy.SetDataSourceName(START_NAME);
            Assert.AreEqual(START_NAME, proxy.GetDataSourceName(), "現在のデータソース名確認");

            actual.GetEmployee();   //  データソースが変更されるInterceptorがかかっているはず

            Assert.AreEqual(DataSourceNameChangeTxSetting.TEST_DATASOURCE_NAME,
                proxy.GetDataSourceName(), "データソース名が切り替わっているはず");
        }
    }

    [S2Dao]
    public interface IWithS2DaoAttr0
    {
        Employee GetEmployee();
    }

    [S2Dao]
    [Bean(typeof(Employee))]
    public interface IWithS2DaoAttr1
    {
        Employee GetEmployee();
    }

    [S2Dao(typeof(CustomDaoSetting))]
    [Bean(typeof(Employee))]
    public interface IWithS2DaoAttr2
    {
        string Hoge();
    }

    [S2Dao(typeof(DataSourceNameChangeDaoSetting))]
    [Bean(typeof(Employee))]
    public interface IWithS2DaoDataSourceNameChange_Class
    {
        object GetEmployee();
    }

    [Bean(typeof(Employee))]
    public interface IWithS2DaoDataSourceNameChange_Method
    {
        [S2Dao(typeof(DataSourceNameChangeDaoSetting))]
        object GetEmployee();
    }

    [Bean(typeof(Employee))]
    public interface IWithS2DaoMethod
    {
        [S2Dao]
        Employee GetEmployee();

        [S2Dao(typeof(CustomDaoSetting))]
        string Hoge2();
    }

    [S2Dao(typeof(Huga))]
    public interface IIllegalStrring
    {
        string Hoge3();
    }


    public class CustomDaoSetting : TypicalDaoSetting
    {
        public override IMethodInterceptor DaoInterceptor
        {
            get
            {
                return new TestInterceptor();
            }
        }

        protected override void SetupDao(IDataSource dataSource)
        {
            Console.WriteLine("Setup is called.");
            base.SetupDao(dataSource);
        }
    }

    public class DataSourceNameChangeDaoSetting : AbstractDaoSetting
    {
        public const string TEST_DATASOURCE_NAME = "ChangedDataSource";

        public override string DataSourceName
        {
            get
            {
                return TEST_DATASOURCE_NAME;
            }
        }

        protected override void SetupDao(IDataSource dataSource)
        {
            Console.WriteLine("Setup is called.");
            daoInterceptor = new TestInterceptor();
        }
    }

    public class Huga
    {
    }

    public class TestInterceptor : AbstractInterceptor
    {
        #region IMethodInterceptor メンバ

        public override object Invoke(IMethodInvocation invocation)
        {
            Console.WriteLine("hoge");
            return "InterceptorCalled";
        }

        #endregion
    }

}
