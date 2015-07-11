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
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Log;
using Seasar.Framework.Util;
using Seasar.Quill.Database.DataSource.Connection;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;
using Seasar.Quill.Xml;

namespace Seasar.Quill
{
    /// <summary>
    /// Quill設定情報取得クラス
    /// </summary>
    public class QuillConfig
    {
        private static readonly Logger _log = Logger.GetLogger(typeof(QuillConfig));

        #region static

        /// <summary>
        /// Quill設定ファイルパス
        /// </summary>
        private static string _configPath = null;
        /// <summary>
        /// Quill設定ファイルパス
        /// （QuillContainerインスタンス生成、
        /// QuillConfig.InitializeQuillConfig呼び出し前に設定して下さい）
        /// </summary>
        public static string ConfigPath
        {
            set { _configPath = value; }
            get { return _configPath; }
        }

        /// <summary>
        /// Quill設定インスタンス
        /// </summary>
        private static QuillConfig _instance = null;

        /// <summary>
        /// Quill設定情報の生成（呼ばれる度にインスタンスを作り直します）
        /// </summary>
        /// <param name="container">Quillコンテナ</param>
        public static void InitializeQuillConfig(QuillContainer container)
        {
            _instance = new QuillConfig(container, ConfigPath);
        }

        /// <summary>
        /// Quill設定情報インスタンスの取得
        /// </summary>
        /// <returns></returns>
        public static QuillConfig GetInstance()
        {
            if (_instance == null)
            {
                throw new QuillApplicationException("EQLL0029");
            }
            return _instance;
        }

        #endregion

        protected const string DEFALT_DATASOURCE_NAME = "DataSource";

        /// <summary>
        /// 文字列("で囲まれているか)判定するための正規表現
        /// </summary>
        protected readonly Regex _regexIsString = new Regex("^\".*\"$");

        /// <summary>
        /// Quill設定情報
        /// </summary>
        protected QuillSection _section = null;

        /// <summary>
        /// DIコンテナ
        /// </summary>
        protected readonly QuillContainer _container;

        /// <summary>
        /// コンストラクタ
        /// :コンテナの設定とQuill設定ファイルの読み込み
        /// </summary>
        /// <param name="container"></param>
        /// <param name="configPath"></param>
        protected QuillConfig(QuillContainer container, string configPath)
        {
            _container = container;
            _section = LoadQuillSection(configPath);
        }

        #region 設定情報の有無判定
        /// <summary>
        /// Quill設定情報の有無
        /// </summary>
        /// <returns>存在する場合はtrue,存在しない場合はfalse</returns>
        public virtual bool HasQuillConfig()
        {
            return (_section != null);
        }

        /// <summary>
        /// Quill設定ファイル内のアセンブリ設定の有無
        /// </summary>
        /// <returns>存在する場合はtrue,存在しない場合はfalse</returns>
        public virtual bool HasAssemblyConfig()
        {
            return (HasQuillConfig() && _section.Assemblys != null && _section.Assemblys.Count > 0);
        }

        /// <summary>
        /// Quill設定ファイル内のDao設定の有無
        /// </summary>
        /// <returns>存在する場合はtrue,存在しない場合はfalse</returns>
        public virtual bool HasDaoSetting()
        {
            return (HasQuillConfig() && !string.IsNullOrEmpty(_section.DaoSetting));
        }

        /// <summary>
        /// Quill設定ファイル内のトランザクション設定の有無
        /// </summary>
        /// <returns>存在する場合はtrue,存在しない場合はfalse</returns>
        public virtual bool HasTransactionSetting()
        {
            return (HasQuillConfig() && !string.IsNullOrEmpty(_section.TransactionSetting));
        }

        /// <summary>
        /// Quill設定ファイル内のデータソース設定の有無
        /// </summary>
        /// <returns>存在する場合はtrue,存在しない場合はfalse</returns>
        public virtual bool HasDataSourceConfig()
        {
            return (HasQuillConfig() && _section.DataSources != null && _section.DataSources.Count > 0);
        }
        #endregion

        /// <summary>
        /// Dao設定クラスの型を取得する
        /// </summary>
        /// <returns></returns>
        public virtual Type GetDaoSettingType()
        {
            Type retType;
            if (HasDaoSetting())
            {
                string typeName = _section.DaoSetting;
                if (TypeUtil.HasNamespace(typeName) == false)
                {
                    //  名前空間の指定がなければ既定の名前空間を使う
                    typeName = string.Format("{0}.{1}",
                        QuillConstants.NAMESPACE_DAOSETTING, typeName);
                }
                retType = ClassUtil.ForName(typeName);

                if (retType == null)
                {
                    throw new QuillApplicationException("EQLL0034", new object[] { typeName });
                }

                SettingUtil.ValidateDaoSettingType(retType);
            }
            else
            {
                //  設定がない場合は既定のDao設定を使う
                retType = SettingUtil.GetDefaultDaoSettingType();
            }

            return retType;
        }

        /// <summary>
        /// Transaction設定クラスの型を取得する
        /// </summary>
        /// <returns></returns>
        public virtual Type GetTransationSettingType()
        {
            Type retType;
            if (HasTransactionSetting())
            {
                //  トランザクション設定クラスが設定ファイルで指定されていればそちらを使う
                string typeName = _section.TransactionSetting;
                if (TypeUtil.HasNamespace(typeName) == false)
                {
                    //  名前空間なしの場合は既定の名前空間から
                    typeName = string.Format("{0}.{1}",
                        QuillConstants.NAMESPACE_TXSETTING, typeName);
                }
                retType = ClassUtil.ForName(typeName);
                if (retType == null)
                {
                    throw new QuillApplicationException("EQLL0035", new object[] { typeName });
                }

                SettingUtil.ValidateTransactionSettingType(retType);
            }
            else
            {
                //  属性引数による指定もapp.configにも設定がなければ
                //  デフォルトのトランザクション設定を使う
                retType = SettingUtil.GetDefaultTransactionType();
            }
            return retType;
        }

        /// <summary>
        /// アセンブリをロードする
        /// </summary>
        public virtual void RegisterAssembly()
        {
            if (HasAssemblyConfig() == false)
            {
                //  アセンブリ設定がなければ処理を抜ける
                LogUtil.Output(_log, "IQLL0005");
                return;
            }

            //  設定ファイルに書かれたアセンブリ名を取得する
            foreach (object item in _section.Assemblys)
            {
                string assemblyName = item as string;
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    //  指定されたアセンブリをロードする
                    AppDomain.CurrentDomain.Load(assemblyName);
                    LogUtil.Output(_log, "IQLL0006", assemblyName);
                }
            }
        }

        /// <summary>
        /// 設定情報からIDataSourceのCollectionを生成
        /// </summary>
        /// <returns>DataSourceコレクション</returns>
        public virtual IDictionary<string, IDataSource> CreateDataSources()
        {
            IDictionary<string, IDataSource> dataSources = new Dictionary<string, IDataSource>();

            //  App.configにquillセクションの記述があればそこから取得
            SetupDataSourceByQuillSection(dataSources);
            if (dataSources.Count > 0)
            {
                LogUtil.Output(_log, "IQLL0007");
                return dataSources;
            }

            //  App.configにConnectionStringの記述があればそこから取得
            SetupByConnectionStringSection(dataSources);
            if (dataSources.Count > 0)
            {
                LogUtil.Output(_log, "IQLL0008");
                return dataSources;
            }

            //  App.configにデータがなければdiconから取得
            if (SingletonS2ContainerConnector.HasComponentDef(DEFALT_DATASOURCE_NAME))
            {
                dataSources[DEFALT_DATASOURCE_NAME] =
                    (IDataSource)SingletonS2ContainerConnector.GetComponent(
                    DEFALT_DATASOURCE_NAME, typeof(IDataSource));
                LogUtil.Output(_log, "IQLL0009");
            }
            else if (SingletonS2ContainerConnector.HasComponentDef(typeof(IDataSource)))
            {
                dataSources[typeof(IDataSource).Name] =
                    (IDataSource)SingletonS2ContainerConnector.GetComponent(typeof(IDataSource));
                LogUtil.Output(_log, "IQLL0010");
            }
            else
            {
                LogUtil.Output(_log, "IQLL0011");
            }

            return dataSources;
        }

        #region DataSource生成補助メソッド

        /// <summary>
        /// app.configのconnectionStringセクション情報からデータソースを生成する
        /// </summary>
        /// <param name="dataSources"></param>
        protected virtual void SetupByConnectionStringSection(
            IDictionary<string, IDataSource> dataSources)
        {
            if (ConfigurationManager.ConnectionStrings.Count > 0)
            {
                foreach (ConnectionStringSettings setting in ConfigurationManager.ConnectionStrings)
                {
                    IDataSource ds = new TxDataSource(setting.Name);
                    dataSources[setting.Name] = ds;
                }
            }
        }

        /// <summary>
        /// app.configのQuillセクション情報からデータソースを生成する
        /// </summary>
        /// <param name="dataSources"></param>
        protected virtual void SetupDataSourceByQuillSection(
            IDictionary<string, IDataSource> dataSources)
        {
            if (HasDataSourceConfig() == false)
            {
                //  データソースの設定がない場合は設定を行わない
                return;
            }
            
            foreach (object item in _section.DataSources)
            {
                SetupDataSourceByQuillSection(dataSources, item);
            }
        }

        /// <summary>
        /// app.configのQuillセクション情報からデータソースを生成する
        /// </summary>
        /// <param name="dataSources"></param>
        /// <param name="dataSourceItem"></param>
        protected virtual void SetupDataSourceByQuillSection(
            IDictionary<string, IDataSource> dataSources, object dataSourceItem)
        {
            if (dataSourceItem is DataSourceSection)
            {
                DataSourceSection dsSection = (DataSourceSection)dataSourceItem;

                //  データソース
                string dataSourceClassName = GetDataSourceClassName(dsSection);
                Type dataSourceClass = ClassUtil.ForName(dataSourceClassName);
                if (dataSourceClass == null)
                {
                    throw new ClassNotFoundRuntimeException(dataSourceClassName);
                }

                //  プロバイダ
                string providerName = GetProviderName(dsSection);
                Type providerClass = ClassUtil.ForName(providerName);
                if (providerClass == null)
                {
                    throw new QuillInvalidClassException("EQLL0032", new object[] { providerName });
                }

                //  接続文字列
                string connectionString = GetConnectionString(dsSection);

                //  クラス組み立て
                DataProvider provider = (DataProvider)ClassUtil.NewInstance(providerClass);
                ConstructorInfo constructorInfo = ClassUtil.GetConstructorInfo(
                    dataSourceClass, new Type[] { typeof(DataProvider), typeof(string) });
                IDataSource dataSource = (IDataSource)constructorInfo.Invoke(
                     new object[] { provider, connectionString });
                dataSources[dsSection.DataSourceName] = dataSource;
            }
        }

        /// <summary>
        /// プロバイダ名の取得
        /// </summary>
        /// <param name="dataSourceSection"></param>
        /// <returns></returns>
        protected virtual string GetProviderName(DataSourceSection dataSourceSection)
        {
            string providerName = dataSourceSection.ProviderName;
            if (string.IsNullOrEmpty(providerName))
            {
                //  空配列はわざと
                throw new QuillConfigNotFoundException("EQLL0031", new object[] { });
            }

            if (TypeUtil.HasNamespace(providerName) == false)
            {
                //  名前空間が指定されていない場合は既定の
                //  名前空間を使用する
                providerName = string.Format("{0}.{1}",
                    QuillConstants.NAMESPACE_PROVIDER, providerName);
            }
            return providerName;
        }

        /// <summary>
        /// データソースクラス名の取得
        /// </summary>
        /// <param name="dataSourceSection"></param>
        /// <returns></returns>
        protected virtual string GetDataSourceClassName(DataSourceSection dataSourceSection)
        {
            string dataSourceClassName = dataSourceSection.DataSourceClassName;
            if (string.IsNullOrEmpty(dataSourceClassName))
            {
                //  空配列はわざと
                throw new QuillConfigNotFoundException("EQLL0030", new object[] { });
            }

            if (TypeUtil.HasNamespace(dataSourceClassName) == false)
            {
                //  名前空間が指定されていない場合は既定の
                //  名前空間を使用する
                dataSourceClassName = string.Format("{0}.{1}",
                    QuillConstants.NAMESPACE_DATASOURCE, dataSourceClassName);
            }
            return dataSourceClassName;
        }

        /// <summary>
        /// 接続文字列の取得
        /// </summary>
        /// <param name="dataSourceSection"></param>
        /// <returns></returns>
        protected virtual string GetConnectionString(DataSourceSection dataSourceSection)
        {
            //  接続文字列
            string configString = dataSourceSection.ConnectionString;
            if (string.IsNullOrEmpty(configString))
            {
                //  空配列はわざと
                throw new QuillConfigNotFoundException("EQLL0033", new object[] { });
            }

            string connectionString = null;

            if (_regexIsString.IsMatch(configString))
            {
                //  最初と最後の「"」を取り除く
                connectionString = configString.Substring(1, configString.Length - 2);
            }
            else
            {
                //  「"」で囲まれていない場合はクラスが指定されていると見なす
                Type connectionStringType = ClassUtil.ForName(configString);
                if (typeof(IConnectionString).IsAssignableFrom(connectionStringType))
                {
                    IConnectionString cs = (IConnectionString)ComponentUtil.GetComponent(
                        _container, connectionStringType);
                    connectionString = cs.GetConnectionString();
                }
                else
                {
                    throw new QuillInvalidClassException(
                        connectionStringType, typeof(IConnectionString));
                }
            }
            //  接続文字列が設定されていない
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("(ConnectionString=Empty)");
            }

            return connectionString;
        }
        #endregion

        /// <summary>
        /// Quill設定情報のロード
        /// </summary>
        /// <param name="configPath">設定ファイルパス(Nullable)</param>
        /// <returns></returns>
        protected virtual QuillSection LoadQuillSection(string configPath)
        {
            //  設定ファイルのパスが明示されている場合は
            //  指定されたパスの設定を使用する
            if (string.IsNullOrEmpty(configPath) == false)
            {
                LogUtil.Output(_log, "IQLL0004", configPath);
                return QuillSectionLoader.LoadFromOuterConfig(configPath);
            }

            //  アプリケーション構成ファイルからQuill設定読み込み
            QuillSection section = QuillSectionHandler.GetQuillSection();
            //  アプリケーション構成ファイルになければ外部ファイルがないか確認
            if (section == null)
            {
                //  外部ファイルのパスを設定
                string outerConfigPath = SettingUtil.GetDefaultQuillConfigPath();

                LogUtil.Output(_log, "IQLL0004", outerConfigPath);
                section = QuillSectionLoader.LoadFromOuterConfig(outerConfigPath);
            }

            return section;
        }

        /// <summary>
        /// Quill設定情報を文字列の形で出力
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("HasQuillConfig=[{0}]", HasQuillConfig());
            if (HasQuillConfig())
            {
                builder.AppendFormat(", QuillSection=[{0}]", _section.ToString());
            }
            return builder.ToString();
        }
    }
}
