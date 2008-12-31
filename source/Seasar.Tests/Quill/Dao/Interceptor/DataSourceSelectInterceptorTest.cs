#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using System.Reflection;
using MbUnit.Framework;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Impl;
using Seasar.Quill;
using Seasar.Quill.Dao.Interceptor;
using Seasar.Quill.Database.DataSource.Impl;

namespace Seasar.Tests.Quill.Dao.Interceptor
{
    [TestFixture]
    public class DataSourceSelectInterceptorTest
    {
        [Test]
        public void TestSetDataSourceName()
        {
            //  データソースの準備
            SelectableDataSourceProxyWithDictionary dataSourceProxy = new SelectableDataSourceProxyWithDictionary();
            dataSourceProxy.RegistDataSource(DummyDataSource1.Name, new DummyDataSource1());
            dataSourceProxy.RegistDataSource(DummyDataSource2.Name, new DummyDataSource2());
            dataSourceProxy.RegistDataSource(DummyDataSource3.Name, new DummyDataSource3());

            //  Interceptorの準備
            IDictionary<MemberInfo, string> memberDataSourceMap = new Dictionary<MemberInfo, string>();
            memberDataSourceMap[typeof(DummyDao1)] = DummyDataSource1.Name;
            memberDataSourceMap[typeof(DummyDao2)] = DummyDataSource2.Name;
            memberDataSourceMap[typeof(DummyDaoMethod).GetMethod("Hoge")] = DummyDataSource3.Name;
            DataSourceSelectInterceptor actualInterceptor = new DataSourceSelectInterceptor();
            actualInterceptor.DataSourceProxy = dataSourceProxy;
            actualInterceptor.DaoDataSourceMap = memberDataSourceMap;

            //  Aspectの準備
            IAspect[] aspects = new IAspect[] { new AspectImpl(actualInterceptor) };

            //  Aspect適用クラスの準備
            DummyDao1 dao1 = (DummyDao1)CreateProxyObject(typeof(DummyDao1), aspects);
            DummyDao2 dao2 = (DummyDao2)CreateProxyObject(typeof(DummyDao2), aspects);
            DummyDaoMethod daoMethod = (DummyDaoMethod)CreateProxyObject(
                typeof(DummyDaoMethod), aspects);

            //  実行、検証
            string originalDataSourcName = dataSourceProxy.GetDataSourceName();

            dao2.Hoge();
            string dao2DataSourceName = dataSourceProxy.GetDataSourceName();
            Assert.AreNotEqual(originalDataSourcName, dao2DataSourceName);
            Assert.AreEqual(DummyDataSource2.Name, dao2DataSourceName,
                "データソース名が変更されている1");

            dao1.Hoge();
            string dao1DataSourcName = dataSourceProxy.GetDataSourceName();
            Assert.AreNotEqual(dao2DataSourceName, dao1DataSourcName);
            Assert.AreEqual(DummyDataSource1.Name, dao1DataSourcName,
                "データソース名が変更されている2");

            daoMethod.Hoge();
            string daoMethodDataSourceName = dataSourceProxy.GetDataSourceName();
            Assert.AreNotEqual(dao1DataSourcName, daoMethodDataSourceName);
            Assert.AreEqual(DummyDataSource3.Name, daoMethodDataSourceName,
                "データソース名が変更されている3");
        }

        /// <summary>
        /// プロキシオブジェクトを作成する
        /// </summary>
        /// <param name="targetType">コンポーネントのType</param>
        /// <param name="aspects">適用するAspectの配列</param>
        /// <returns>作成されたプロキシオブジェクト</returns>
        private object CreateProxyObject(Type targetType, IAspect[] aspects)
        {
            QuillComponent component = new QuillComponent(targetType, targetType, aspects);
            return component.GetComponentObject(targetType);
        }
    }

    class DummyDataSource1 : DataSourceImpl
    {
        public const string Name = "Dummy1";
    }

    class DummyDataSource2 : DataSourceImpl
    {
        public const string Name = "Dummy2";
    }

    class DummyDataSource3 : DataSourceImpl
    {
        public const string Name = "Dummy3";
    }

    public class DummyDao1
    {
        public virtual void Hoge()
        {
        }
    }

    public class DummyDao2
    {
        public virtual void Hoge()
        {
        }
    }

    public class DummyDaoMethod
    {
        public virtual void Hoge()
        {
        }
    }
}
