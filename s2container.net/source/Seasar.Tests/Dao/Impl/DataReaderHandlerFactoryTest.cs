#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using MbUnit.Framework;
using Seasar.Dao;
using Seasar.Dao.Attrs;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class DataReaderHandlerFactoryTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestDataReaderGenericCollection()
        {
            DataReaderHandlerFactory = new GenericCollectionDataReaderHandlerFactory();
            DaoMetaDataImpl dmd = (DaoMetaDataImpl) CreateDaoMetaData(typeof(IEmployeeDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetBySalReturnCollectionGeneric");
            Collection<Employee> ret = cmd.Execute(new object[] { 100.0f }) as Collection<Employee>;
            Assert.IsNotNull(ret, "Collection<T>型に対応したIDataReaderHandlerFactoryで取得。");
            Assert.AreEqual(14, ret.Count);
        }

        [Test, S2]
        public void TestDataReaderList()
        {
            DataReaderHandlerFactory = new GenericCollectionDataReaderHandlerFactory();
            DaoMetaDataImpl dmd = (DaoMetaDataImpl) CreateDaoMetaData(typeof(IEmployeeDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetBySalReturnIListGeneric");
            IList<Employee> ret = cmd.Execute(new object[] { 100.0f }) as IList<Employee>;
            Assert.IsNotNull(ret, "S2Dao.NET標準対応のデータ型でも取得。");
            Assert.AreEqual(14, ret.Count);
        }

        [Bean(typeof(Employee))]
        private interface IEmployeeDao
        {
            [Query("sal >= /*sal*/")]
            Collection<Employee> GetBySalReturnCollectionGeneric(float sal);

            [Query("sal >= /*sal*/")]
            IList<Employee> GetBySalReturnIListGeneric(float sal);
        }

        private class BeanGenericCollectionMetaDataDataReaderHandler
            : BeanListMetaDataDataReaderHandler
        {
            public BeanGenericCollectionMetaDataDataReaderHandler(
                IBeanMetaData beanMetaData, IRowCreator rowCreator, IRelationRowCreator relationRowCreator)
                : base(beanMetaData, rowCreator, relationRowCreator)
            {
            }

            public override object Handle(IDataReader dataReader)
            {
                Type generic = typeof(Collection<>);
                Type constructed = generic.MakeGenericType(BeanMetaData.BeanType);

                object list = Activator.CreateInstance(constructed);

                Handle(dataReader, (IList) list);

                return list;
            }
        }

        private class ObjectGenericCollectionDataReaderHandler : AbstractObjectListDataReaderHandler
        {
            private readonly Type _elementType;

            public ObjectGenericCollectionDataReaderHandler(Type elementType)
            {
                _elementType = elementType;
            }

            public override object Handle(IDataReader dataReader)
            {
                Type listType = typeof(List<>);
                Type genericType = listType.MakeGenericType(_elementType);
                object resultList = Activator.CreateInstance(genericType);
                Handle(dataReader, (IList) resultList);
                return resultList;
            }

            protected override object GetValue(object val)
            {
                return ConversionUtil.ConvertTargetType(val, _elementType);
            }
        }

        private class GenericCollectionDataReaderHandlerFactory : DataReaderHandlerFactory
        {
            public override IDataReaderHandler GetResultSetHandler(Type beanType, IBeanMetaData bmd, MethodInfo mi)
            {
                Type retType = mi.ReturnType;
                if (retType.IsGenericType && retType.GetGenericTypeDefinition().Equals(typeof(Collection<>)))
                {
                    Type elementType = retType.GetGenericArguments()[0];
                    if (AssignTypeUtil.IsSimpleType(elementType))
                    {
                        return new ObjectGenericCollectionDataReaderHandler(retType);
                    }
                    else
                    {
                        return new BeanGenericCollectionMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
                    }
                }
                return base.GetResultSetHandler(beanType, bmd, mi);
            }
        }
    }
}
