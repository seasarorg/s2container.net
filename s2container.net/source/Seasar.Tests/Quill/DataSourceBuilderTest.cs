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

using System.Collections.Generic;
using MbUnit.Framework;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Quill;
using Seasar.Quill.Database.DataSource.Connection;
using Seasar.Quill.Database.Provider;
using Seasar.Quill.Util;

namespace Seasar.Tests.Quill
{
    [TestFixture]
    public class DataSourceBuilderTest
    {
        [Test]
        public void TestCreateDataSources()
        {
            const string CONNECTION_STRING_FOR_HOGEX = "from DummyConnectionString";

            QuillContainer container = new QuillContainer();
            //  Test2で使用
            DummyConnectionString cs = (DummyConnectionString)ComponentUtil.GetComponent(
                container, typeof(DummyConnectionString));
            cs.ConnectionName = CONNECTION_STRING_FOR_HOGEX;
            DataSourceBuilder builder = new DataSourceBuilder(container);
            IDictionary<string, IDataSource> dsMap = builder.CreateDataSources();

            Assert.IsNotNull(dsMap);

            {
                //  *** Test1
                Assert.IsTrue(dsMap.ContainsKey("Hoge2"));
                IDataSource ds = dsMap["Hoge2"];
                Assert.IsTrue(ds is DataSourceImpl);
                Assert.IsTrue(((DataSourceImpl)ds).DataProvider is Seasar.Quill.Database.Provider.Oracle);
                Assert.AreEqual(@"(local)\Hoge2", ((DataSourceImpl)ds).ConnectionString);
            }
            { 
                //  *** Test2
                Assert.IsTrue(dsMap.ContainsKey("HogeX"));
                IDataSource ds = dsMap["HogeX"];
                Assert.IsTrue(ds is DataSourceImpl);
                Assert.IsTrue(((DataSourceImpl)ds).DataProvider is SqlServer);
                //  SingletonなDummyConnectionStringのインスタンスに設定した接続文字列が使われる
                Assert.AreEqual(CONNECTION_STRING_FOR_HOGEX, ((DataSourceImpl)ds).ConnectionString);
            }
        }
    }

    /// <summary>
    /// クラスから接続文字列を取得する場合のテスト用クラス
    /// </summary>
    public class DummyConnectionString : IConnectionString
    {
        public string ConnectionName = null;

        #region IConnectionString メンバ

        public string GetConnectionString()
        {
            return ConnectionName == null ? "null" : ConnectionName;
        }

        #endregion
    }
}
