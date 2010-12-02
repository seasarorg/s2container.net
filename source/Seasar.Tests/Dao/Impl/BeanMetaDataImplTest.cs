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
using System.Data.SqlTypes;
using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Dao;
using Seasar.Dao.Attrs;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class BeanMetaDataImplTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestSetup()
        {
            // Java版だとプロパティ名の先頭1文字は、英小文字。.NET版は、英小文字。
            IBeanMetaData bmd = CreateBeanMetaData(typeof(MyBean));
            Assert.AreEqual("MyBean", bmd.TableName, "1");
            Assert.AreEqual(3, bmd.PropertyTypeSize, "2");
            IPropertyType aaa = bmd.GetPropertyType("aaa");
            Assert.AreEqual("Aaa", aaa.ColumnName, "3");    // Java : aaa
            IPropertyType bbb = bmd.GetPropertyType("bbb");
            Assert.AreEqual("myBbb", bbb.ColumnName, "4");
            Assert.AreEqual(1, bmd.RelationPropertyTypeSize, "5");
            IRelationPropertyType rpt = bmd.GetRelationPropertyType(0);
            Assert.AreEqual(1, rpt.KeySize, "6");
            Assert.AreEqual("ddd", rpt.GetMyKey(0), "7");
            Assert.AreEqual("id", rpt.GetYourKey(0), "8");
            Assert.IsNotNull(bmd.IdentifierGenerator, "9");
            Assert.AreEqual(1, bmd.PrimaryKeySize, "10");
            Assert.AreEqual("Aaa", bmd.GetPrimaryKey(0), "11");
        }

        [Test, S2]
        public void TestSetupDatabaseMetaData()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Employee));
            IPropertyType empno = bmd.GetPropertyType("Empno");
            Assert.AreEqual(true, empno.IsPrimaryKey, "1");
            Assert.AreEqual(true, empno.IsPersistent, "2");
            IPropertyType ename = bmd.GetPropertyType("ename");
            Assert.AreEqual(false, ename.IsPrimaryKey, "3");
            IPropertyType dummy = bmd.GetPropertyType("dummy");
            Assert.AreEqual(false, dummy.IsPersistent, "4");
        }

        [Test, S2]
        public void TestSetupAutoSelectList()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Department));
            IBeanMetaData bmd2 = CreateBeanMetaData(typeof(Employee));
            string sql = bmd.AutoSelectList;
            string sql2 = bmd2.AutoSelectList;
            Trace.WriteLine(sql);
            Trace.WriteLine(sql2);

            Assert.IsTrue(sql2.IndexOf("EMP.DEPTNO") > 0, sql2);
            Assert.IsTrue(sql2.IndexOf("Department.DEPTNO AS DEPTNO_0") > 0, "2");
            Assert.IsTrue(sql2.IndexOf("dummy_0") < 0, "3");
        }

        [Test, S2]
        public void TestConvertFullColumnName()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Employee));
            Assert.AreEqual("EMP.empno", bmd.ConvertFullColumnName("empno"), "1");
            Assert.AreEqual("Department.dname", bmd.ConvertFullColumnName("dname_0"), "2");
        }

        [Test, S2]
        public void TestHasPropertyTypeByAliasName()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Employee));
            Assert.AreEqual(true, bmd.HasPropertyTypeByAliasName("empno"), "1");
            Assert.AreEqual(true, bmd.HasPropertyTypeByAliasName("dname_0"), "2");
            Assert.AreEqual(false, bmd.HasPropertyTypeByAliasName("xxx"), "3");
            Assert.AreEqual(false, bmd.HasPropertyTypeByAliasName("xxx_10"), "4");
            Assert.AreEqual(false, bmd.HasPropertyTypeByAliasName("xxx_0"), "5");
        }

        [Test, S2]
        public void TestGetPropertyTypeByAliasName()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Employee));
            Assert.IsNotNull(bmd.GetPropertyTypeByAliasName("empno"), "1");
            Assert.IsNotNull(bmd.GetPropertyTypeByAliasName("dname_0"), "2");
        }

        [Test, S2]
        public void TestSelfReference()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Employee4));
            IRelationPropertyType rpt = bmd.GetRelationPropertyType("parent");
            Assert.AreEqual(typeof(Employee4), rpt.BeanMetaData.BeanType, "1");
        }

        [Test, S2]
        public void TestNoPersistentPropsEmpty()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Ddd));
            IPropertyType pt = bmd.GetPropertyType("Name");
            Assert.AreEqual(false, pt.IsPersistent, "1");
        }

        [Test, S2]
        public void TestNoPersistentPropsDefined()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Eee));
            IPropertyType pt = bmd.GetPropertyType("name");
            Assert.AreEqual(false, pt.IsPersistent, "1");
        }

        [Test, S2]
        public void TestPrimaryKeyForIdentifier()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(IdTable));
            Assert.AreEqual("ID", bmd.GetPrimaryKey(0), "1");
        }

        [Test, S2]
        public void TestGetVersionNoPropertyName()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Fff));
            Assert.AreEqual("Version", bmd.VersionNoPropertyName, "1");
        }

        [Test, S2]
        public void TestGetTimestampPropertyName()
        {
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Fff));
            Assert.AreEqual("Updated", bmd.TimestampPropertyName, "1");
        }

        [Table("MyBean")]
        public class MyBean
        {
            private SqlInt32 _aaa;
            private string _bbb;
            private Ccc _ccc;
            private SqlInt32 _ddd;

            [ID(IDType.ASSIGNED)]
            public SqlInt32 Aaa
            {
                get { return _aaa; }
                set { _aaa = value; }
            }

            [Seasar.Dao.Attrs.Column("myBbb")]
            public string Bbb
            {
                get { return _bbb; }
                set { _bbb = value; }
            }

            [Relno(0), Relkeys("ddd:id")]
            public Ccc Cccc
            {
                get { return _ccc; }
                set { _ccc = value; }
            }

            public SqlInt32 Ddd
            {
                get { return _ddd; }
                set { _ddd = value; }
            }
        }

        public class Ccc
        {
            private SqlInt32 _id;

            [ID(IDType.ASSIGNED)]
            public SqlInt32 Id
            {
                get { return _id; }
                set { _id = value; }
            }
        }

        [NoPersistentProps("")]
        public class Ddd : Ccc
        {
            private string _name;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
        }

        [NoPersistentProps("name")]
        public class Eee : Ccc
        {
            private string _name;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
        }

        [VersionNoProperty("Version")]
        [TimestampProperty("Updated")]
        public class Fff
        {
            private int _version;
            private SqlInt32 _id;
            private DateTime _updated;

            public SqlInt32 Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public int Version
            {
                get { return _version; }
                set { _version = value; }
            }

            public DateTime Updated
            {
                get { return _updated; }
                set { _updated = value; }
            }
        }
    }
}
