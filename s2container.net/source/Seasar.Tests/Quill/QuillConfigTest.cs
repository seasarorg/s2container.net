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

using System.Diagnostics;
using MbUnit.Framework;
using Seasar.Quill;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;
using System;
using Seasar.Quill.Dao;
using Seasar.Quill.Dao.Impl;
using Seasar.Quill.Database.Tx.Impl;
using Seasar.Extension.ADO;
using System.Collections.Generic;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Container.Factory;
using System.Data;
using System.Data.SqlClient;

namespace Seasar.Tests.Quill
{
    /// <summary>
    /// Quill設定クラスのテスト
    /// </summary>
    [TestFixture]
    public class QuillConfigTest
    {
        private const string CONFIG_NOT_EXISTS = "hogeHoge";
        private const string CONFIG_NO_EXIST_CLASS = "GetXxxSetting_IllegalClassName";
        private const string CONFIG_EMPTY = "QuillConfigTest_Empty";
        private const string CONFIG_FULL = "QuillConfigTest_Full";

        [Test]
        public void TestChangeConnection()
        {
            using (IDbConnection connection = new SqlConnection("Server=localhost;database=s2dotnetdemo;Integrated Security=SSPI"))
            {
                try
                {
                    connection.Open();
                    Assert.Fail();
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Exception occured.");
                }


                connection.ConnectionString = "Server=localhost\\SQLEXPRESS;database=s2dotnetdemo;Integrated Security=SSPI";
                connection.Open();
                connection.Close();

            }
        }

        #region HasXxx 設定有無判定
        [Test]
        public void TestHasQuillConfig()
        {
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NOT_EXISTS);
                Assert.IsFalse(config.HasQuillConfig(), "設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_EMPTY);
                Assert.IsFalse(config.HasQuillConfig(), "設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_FULL);
                Assert.IsTrue(config.HasQuillConfig(), "設定あり");
            }
        }

        [Test]
        public void TestHasAssemblyConfig()
        {
            const string ASSEMBLY_NOTHING = "HasAssemblyConfig_Nothing";
            const string ASSEMBLY_EMPTY = "HasAssemblyConfig_Empty";

            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NOT_EXISTS);
                Assert.IsFalse(config.HasAssemblyConfig(), "Quill設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_EMPTY);
                Assert.IsFalse(config.HasAssemblyConfig(), "Quill設定空");
            }
            {
                QuillConfig config = GetTestQuillConfig(ASSEMBLY_NOTHING);
                Assert.IsFalse(config.HasAssemblyConfig(), "アセンブリ設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(ASSEMBLY_EMPTY);
                Assert.IsFalse(config.HasAssemblyConfig(), "アセンブリ設定空");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_FULL);
                Assert.IsTrue(config.HasAssemblyConfig(), "アセンブリ設定あり");
            }
        }

        [Test]
        public void TestHasDaoSetting()
        {
            const string DAO_SETTING_NOTHING = "HasDaoSetting_Nothing";
            const string DAO_SETTING_EMPTY = "HasDaoSeting_Empty";

            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NOT_EXISTS);
                Assert.IsFalse(config.HasDaoSetting(), "Quill設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_EMPTY);
                Assert.IsFalse(config.HasDaoSetting(), "Quill設定空");
            }
            {
                QuillConfig config = GetTestQuillConfig(DAO_SETTING_NOTHING);
                Assert.IsFalse(config.HasDaoSetting(), "Dao設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(DAO_SETTING_EMPTY);
                Assert.IsFalse(config.HasDaoSetting(), "Dao設定空");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_FULL);
                Assert.IsTrue(config.HasDaoSetting(), "Dao設定あり");
            }
        }

        [Test]
        public void TestHasTransactionSetting()
        {
            const string TX_SETTING_NOTHING = "HasTransactionSetting_Nothing";
            const string TX_SETTING_EMPTY = "HasTransactionSeting_Empty";

            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NOT_EXISTS);
                Assert.IsFalse(config.HasTransactionSetting(), "Quill設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_EMPTY);
                Assert.IsFalse(config.HasTransactionSetting(), "Quill設定空");
            }
            {
                QuillConfig config = GetTestQuillConfig(TX_SETTING_NOTHING);
                Assert.IsFalse(config.HasTransactionSetting(), "トランザクション設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(TX_SETTING_EMPTY);
                Assert.IsFalse(config.HasTransactionSetting(), "トランザクション設定空");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_FULL);
                Assert.IsTrue(config.HasTransactionSetting(), "トランザクション設定あり");
            }
        }

        [Test]
        public void TestHasDataSourceConfig()
        {
            const string DS_CONFIG_NOTHING = "HasDataSourceConfig_Nothing";
            const string DS_CONFIG_EMPTY = "HasDataSourceConfig_Empty";

            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NOT_EXISTS);
                Assert.IsFalse(config.HasDataSourceConfig(), "Quill設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_EMPTY);
                Assert.IsFalse(config.HasDataSourceConfig(), "Quill設定空");
            }
            {
                QuillConfig config = GetTestQuillConfig(DS_CONFIG_NOTHING);
                Assert.IsFalse(config.HasDataSourceConfig(), "データソース設定なし");
            }
            {
                QuillConfig config = GetTestQuillConfig(DS_CONFIG_EMPTY);
                Assert.IsFalse(config.HasDataSourceConfig(), "データソース設定空");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_FULL);
                Assert.IsTrue(config.HasDataSourceConfig(), "データソース設定あり");
            }
        }

        #endregion

        [Test]
        public void TestGetDaoSettingType()
        {
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NOT_EXISTS);
                Assert.AreEqual(typeof(TypicalDaoSetting), config.GetDaoSettingType(),
                    "設定ファイルの指定がない場合は既定ファイルの設定を使う");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_EMPTY);
                Assert.AreEqual(typeof(TypicalDaoSetting), config.GetDaoSettingType(),
                    "設定がない場合はデフォルト設定");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_FULL);
                Assert.AreEqual(typeof(DaoSetting4Test), config.GetDaoSettingType(),
                    "設定がある場合は指定された型");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NO_EXIST_CLASS);
                try
                {
                    config.GetDaoSettingType();
                }
                catch (QuillApplicationException ex)
                {
                    Assert.AreEqual("EQLL0034", ex.MessageCode);
                }
            }
        }

        [Test]
        public void TestGetTransactionSettingType()
        {
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NOT_EXISTS);
                Assert.AreEqual(typeof(TypicalTransactionSetting), config.GetTransationSettingType(),
                    "設定ファイルの指定がない場合は既定ファイルの設定を使う");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_EMPTY);
                Assert.AreEqual(typeof(TypicalTransactionSetting), config.GetTransationSettingType(),
                    "設定がない場合はデフォルト設定");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_FULL);
                Assert.AreEqual(typeof(TxSetting4Test), config.GetTransationSettingType(),
                    "設定がある場合は指定された型");
            }
            {
                QuillConfig config = GetTestQuillConfig(CONFIG_NO_EXIST_CLASS);
                try
                {
                    config.GetTransationSettingType();
                }
                catch (QuillApplicationException ex)
                {
                    Assert.AreEqual("EQLL0035", ex.MessageCode);
                }
            }
        }

        [Test]
        public void TestCreateDataSources()
        {
            const string DS_NOTHING = "CreateDataSources_Nothing";
            const string DS_EMPTY = "CreateDataSources_Empty";
            const string DS_NO_DATASOURCE = "CreateDataSources_NoDataSourceClass";
            const string DS_ILLEGAL_DATASOURCE = "CreateDataSources_IllegalDataSource";
            const string DS_NO_PROVIDER = "CreateDataSources_NoProvider";
            const string DS_ILLEGAL_PROVIDER = "CreateDataSources_IllegalProvider";
            const string DS_NO_CONNECTION_STRING = "CreateDataSources_NoConnectionString";

            {
                //  Quill設定にデータソースがない場合
                QuillConfig config = GetTestQuillConfig(DS_NOTHING);
                IDictionary<string, IDataSource> dataSources = config.CreateDataSources();
                Assert.IsNotNull(dataSources, "インスタンスは生成されている");
                Assert.AreEqual(2, dataSources.Count, "App.configのConnectionStringセクションに設定されている数");
                Assert.IsTrue(dataSources.ContainsKey("providerEx"), "設定１");
                Assert.IsTrue(dataSources.ContainsKey("provider"), "設定２");
                IDataSource ds1 = dataSources["providerEx"];
                Assert.IsTrue(ds1 is TxDataSource, ds1.GetType().Name);
                IDataSource ds2 = dataSources["provider"];
                Assert.IsTrue(ds2 is TxDataSource, ds2.GetType().Name); 
            }
            {
                //  Quill設定にあるデータソースが空の場合
                QuillConfig config = GetTestQuillConfig(DS_EMPTY);
                IDictionary<string, IDataSource> dataSources = config.CreateDataSources();
                Assert.IsNotNull(dataSources, "インスタンスは生成されている");
                Assert.AreEqual(2, dataSources.Count, "App.configのConnectionStringセクションに設定されている数");
                Assert.IsTrue(dataSources.ContainsKey("providerEx"), "設定１");
                Assert.IsTrue(dataSources.ContainsKey("provider"), "設定２");
                IDataSource ds1 = dataSources["providerEx"];
                Assert.IsTrue(ds1 is TxDataSource, ds1.GetType().Name);
                IDataSource ds2 = dataSources["provider"];
                Assert.IsTrue(ds2 is TxDataSource, ds2.GetType().Name); 
            }
            {
                //  データソースクラス設定がない
                QuillConfig config = GetTestQuillConfig(DS_NO_DATASOURCE);
                try
                {
                    IDictionary<string, IDataSource> dataSources = config.CreateDataSources();
                    Assert.Fail("データソース設定がないことを知らせる例外が出るはず");
                }
                catch (QuillConfigNotFoundException ex)
                {
                    Trace.WriteLine(ex);
                }
            }
            {
                //  誤ったデータソース
                QuillConfig config = GetTestQuillConfig(DS_ILLEGAL_DATASOURCE);
                try
                {
                    IDictionary<string, IDataSource> dataSources = config.CreateDataSources();
                    Assert.Fail("データソースクラス名を間違えていることを知らせる例外が出るはず");
                }
                catch (ClassNotFoundRuntimeException ex)
                {
                    Trace.WriteLine(ex);
                }
            }
            {
                //  プロバイダ設定がない
                QuillConfig config = GetTestQuillConfig(DS_NO_PROVIDER);
                try
                {
                    IDictionary<string, IDataSource> dataSources = config.CreateDataSources();
                    Assert.Fail("プロバイダ設定がないことを知らせる例外が出るはず");
                }
                catch (QuillConfigNotFoundException ex)
                {
                    Trace.WriteLine(ex);
                }
            }
            {
                //  誤ったプロバイダ
                QuillConfig config = GetTestQuillConfig(DS_ILLEGAL_PROVIDER);
                try
                {
                    IDictionary<string, IDataSource> dataSources = config.CreateDataSources();
                    Assert.Fail("プロバイダ名を間違えていることを知らせる例外が出るはず");
                }
                catch (QuillInvalidClassException ex)
                {
                    Trace.WriteLine(ex);
                }
            }
            {
                //  接続文字列設定がない
                QuillConfig config = GetTestQuillConfig(DS_NO_CONNECTION_STRING);
                try
                {
                    IDictionary<string, IDataSource> dataSources = config.CreateDataSources();
                    Assert.Fail("接続文字列設定がないことを知らせる例外が出るはず");
                }
                catch (QuillConfigNotFoundException ex)
                {
                    Trace.WriteLine(ex);
                }
            }
        }

        [Test]
        public void TestLoadQuillSection_Default()
        {
            //  既定箇所にあるQuill設定ファイルを読みにいくかテスト
            //  ## Arrange / Act ##
            QuillConfig config = GetTestQuillConfig(null);

            //  ## Assert ##
            Assert.IsNotNull(config, "1");
            Assert.IsTrue(config.HasDataSourceConfig(), "2");
            Console.WriteLine(QuillConfig.ConfigPath);
            IDictionary<string, IDataSource> dataSources = config.CreateDataSources();
            Assert.IsNotNull(dataSources, "3");
            Assert.IsTrue(dataSources.ContainsKey("for_Quill.dll.config"), 
                "このテストプロジェクトでは既定のQuill設定ファイルにのみ書かれている情報");
        }

        [Test]
        public void Test設定ファイルなしでQuillContainerを使えるか()
        {
            QuillConfig.ConfigPath = CONFIG_NOT_EXISTS;
            QuillContainer container = new QuillContainer();
            Assert.IsNotNull(container);
        }

        #region Helper

        /// <summary>
        /// テスト用QuillConfigのインスタンスを取得
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private QuillConfig GetTestQuillConfig(string fileName)
        {
            QuillContainer container = QuillInjector.GetInstance().Container;
            //  MbUnitの場合テストを実行するとstatic変数の値が維持されてしまうため
            //  ここで初期化
            QuillConfig.ConfigPath = null; 
            if (!string.IsNullOrEmpty(fileName))
            {
                QuillConfig.ConfigPath = SettingUtil.GetQuillConfigPath(
                    string.Format("Quill\\ResourcesForQuillConfig\\{0}.config", fileName));
            }
            
            Trace.WriteLine(QuillConfig.ConfigPath);
            QuillConfig.InitializeQuillConfig(container);   
            return QuillConfig.GetInstance();
        }

        #endregion
    }

    /// <summary>
    /// テスト用Dao設定クラス
    /// </summary>
    public class DaoSetting4Test : AbstractDaoSetting
    {
        protected override void SetupDao(Seasar.Extension.ADO.IDataSource dataSource)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// テスト用Transaction設定クラス
    /// </summary>
    public class TxSetting4Test : AbstractTransactionSetting
    {
        protected override void SetupTransaction(Seasar.Extension.ADO.IDataSource dataSource)
        {
            throw new NotImplementedException();
        }
    }
}
