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

using System;
using System.Collections;
using MbUnit.Framework;
using Seasar.Dao;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class AbstractBeanMetaDataDataReaderHandlerTest : S2DaoTestCase
    {
        [Test, S2]
        public void TestCreateColumnMetaData()
        {
            TestBeanMetaData beanMetaData = new TestBeanMetaData(typeof(TestBean));

            TestDataReaderHandler handler = new TestDataReaderHandler(beanMetaData, new RowCreatorImpl());

            IList columnNames = new CaseInsentiveSet();
            columnNames.Add("emp_no");
            columnNames.Add("empname");

            IColumnMetaData[] columnMetaDatas = handler.TestCreateColumnMetaData(columnNames);

            Assert.AreEqual(2, columnMetaDatas.Length);

            Assert.AreEqual("empname", columnMetaDatas[0].ColumnName);
            Assert.AreEqual("emp_no", columnMetaDatas[1].ColumnName);

        }

        private class TestDataReaderHandler : AbstractBeanMetaDataDataReaderHandler
        {
            public TestDataReaderHandler(IBeanMetaData beanMetaData, IRowCreator rowCreator)
                : base(beanMetaData, rowCreator)
            {
            }

            public IColumnMetaData[] TestCreateColumnMetaData(IList columnNames)
            {
                return base.CreateColumnMetaData(columnNames);
            }
        }

        private class TestBeanMetaData : BeanMetaDataImpl
        {
            public TestBeanMetaData(Type type)
            {
                BeanType = type;
                BeanAnnotationReader = new FieldBeanAnnotationReader(type);
                Initialize();

                IPropertyType pt = GetPropertyType("Empno");
                pt.IsPersistent = false;
            }
        }

        private class TestBean
        {
            private int _empno;
            private string _empname;

            public int Empno
            {
                set { _empno = value; }
                get { return _empno; }
            }

            public string Empname
            {
                set { _empname = value; }
                get { return _empname; }
            }
        }
    }
}
