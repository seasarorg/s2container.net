#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using Seasar.Dao;
using Seasar.Dao.Attrs;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Quill;
using Seasar.Quill.Attrs;
using Seasar.Quill.Dao.Impl;
using Seasar.Quill.Exception;
using Seasar.Tests.Dao.Impl;

namespace Seasar.Tests.Quill.Attr
{
    [TestFixture]
    public class S2DaoAttributeTest : S2DaoTestCase
    {
        [Test]
        public void TestSetTypicalDaoSetting()
        {
            using (QuillContainer container = new QuillContainer())
            {
                object[] attrs = typeof(IWithS2DaoAttr0).GetCustomAttributes(false);
                Assert.AreEqual(2, attrs.Length);
                Assert.IsTrue(attrs[0] is S2DaoAttribute || attrs[1] is S2DaoAttribute);
            }
        }

        [Test]
        public void TestS2DaoAttribute_正常()
        {
            //  ## Arrange ##
            QuillContainer container = new QuillContainer();
            {
                //  ## Act ##
                QuillComponent component = container.GetComponent(typeof(IWithS2DaoAttr1));
                //  ## Assert ##
                IWithS2DaoAttr1 actual = (IWithS2DaoAttr1)component.GetComponentObject(
                    typeof(IWithS2DaoAttr1));
                Assert.IsNotNull(actual, "01");
                Employee emp = actual.GetEmployee();
                Assert.IsNotNull(emp, "02");
                Console.WriteLine(emp.Ename);
            }
            {
                //  ## Act ##
                QuillComponent component = container.GetComponent(typeof(IWithS2DaoAttr2));
                //  ## Assert ##
                IWithS2DaoAttr2 actual = (IWithS2DaoAttr2)component.GetComponentObject(
                    typeof(IWithS2DaoAttr2));
                Assert.IsNotNull(actual, "11");
                string ret = actual.Hoge();
                Assert.AreEqual("InterceptorCalled", ret, "12");
            }
            {
                //  ## Act ##
                QuillComponent component = container.GetComponent(typeof(IWithS2DaoMethod));
                //  ## Assert ##
                IWithS2DaoMethod actual = (IWithS2DaoMethod)component.GetComponentObject(
                    typeof(IWithS2DaoMethod));
                Assert.IsNotNull(actual, "21");
                Employee emp = actual.GetEmployee();
                Assert.IsNotNull(emp, "22");
                string ret = actual.Hoge2();
                Assert.AreEqual("InterceptorCalled", ret, "23");
            }
        }

        [Test]
        public void TestS2DaoAttribute_使用不可な型を指定()
        {
            //  ## Arrange ##
            QuillContainer container = new QuillContainer();
            try
            {
                container.GetComponent(typeof(IIllegalStrring));
                Assert.Fail("1");
            }
            catch (QuillApplicationException)
            {
            }
        }
    }

    [Transaction]
    [S2Dao]
    public interface IWithS2DaoAttr0
    {
        Employee GetEmployee();
    }

    [Transaction]
    [S2Dao]
    [Bean(typeof(Employee))]
    public interface IWithS2DaoAttr1
    {
        Employee GetEmployee();
    }

    [Transaction]
    [S2Dao(typeof(CustomDaoSetting))]
    [Bean(typeof(Employee))]
    public interface IWithS2DaoAttr2
    {
        string Hoge();
    }

    [Transaction]
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
        public override string DataSourceName
        {
            get
            {
                return "Hoge1";
            }
        }

        protected override void SetupDao(IDataSource dataSource)
        {
            Console.WriteLine("Setup is called.");
            base.SetupDao(dataSource);
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
