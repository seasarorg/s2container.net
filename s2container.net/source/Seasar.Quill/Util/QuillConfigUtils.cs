using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Quill.Xml;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Quill.Database.Tx;
using Seasar.Quill.Database.Tx.Impl;
using Seasar.Framework.Util;
using Seasar.Quill.Exception;
using Seasar.Extension.ADO;
using System.Configuration;
using Seasar.Extension.Tx.Impl;
using System.Text.RegularExpressions;
using System.Reflection;
using Seasar.Framework.Container.Factory;
using Seasar.Quill.Database.DataSource.Connection;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// Quill設定関連ユーティリティ
    /// </summary>
    public class QuillConfigUtils
    {
        private const string DEFALT_DATASOURCE_NAME = "DataSource";

        /// <summary>
        /// 文字列("で囲まれているか)判定するための正規表現
        /// </summary>
        private static readonly Regex _regexIsString = new Regex("^\".*\"$");

        /// <summary>
        /// 設定ファイル上に定義されている
        /// トランザクション設定クラスの型情報を取得する
        /// </summary>
        /// <param name="section">Quill設定情報</param>
        /// <returns></returns>
        public static Type GetTxSettingType(QuillSection section)
        {
            if (section == null || string.IsNullOrEmpty(section.TransactionSetting))
            {
                return typeof(TypicalTransactionSetting);
            }
            else
            {
                //  トランザクション設定クラスが設定ファイルで指定されていればそちらを使う
                Type txSettingType = ClassUtil.ForName(section.TransactionSetting);
                if (txSettingType == null || typeof(ITransactionSetting).IsAssignableFrom(txSettingType) == false)
                {
                    throw new QuillInvalidClassException(txSettingType, typeof(ITransactionSetting));
                }
                return txSettingType;
            }
        }

        /// <summary>
        /// アセンブリをロードする
        /// </summary>
        /// <param name="section">Quill設定情報</param>
        public static void RegistAssembly(QuillSection section)
        {
            if (section != null && section.Assemblys != null && section.Assemblys.Count > 0)
            {
                //  設定ファイルに書かれたアセンブリ名を取得する
                foreach (object item in section.Assemblys)
                {
                    string assemblyName = item as string;
                    if (!string.IsNullOrEmpty(assemblyName))
                    {
                        //  指定されたアセンブリをロードする
                        AppDomain.CurrentDomain.Load(assemblyName);
                    }
                }
            }
        }

        /// <summary>
        /// 設定情報からIDataSourceのCollectionを生成
        /// </summary>
        /// <param name="section">Quill設定情報</param>
        /// <returns>DataSourceコレクション</returns>
        public static IDictionary<string, IDataSource> CreateDataSources(QuillSection section)
        {
            IDictionary<string, IDataSource> dataSources = new Dictionary<string, IDataSource>();

            //  App.configにquillセクションの記述があればそこから取得
            SetupByQuillSection(section, dataSources);
            if (dataSources.Count > 0)
            {
                return dataSources;
            }

            //  App.configにConnectionStringの記述があればそこから取得
            SetupByConnectionStringSection(dataSources);
            if (dataSources.Count > 0)
            {
                return dataSources;
            }

            //  App.configにデータがなければdiconから取得
            if ( SingletonS2ContainerConnector.HasComponentDef(DEFALT_DATASOURCE_NAME) )
            {
                dataSources[DEFALT_DATASOURCE_NAME] = 
                    (IDataSource)SingletonS2ContainerConnector.GetComponent(
                    DEFALT_DATASOURCE_NAME, typeof(IDataSource));
            }
            else if ( SingletonS2ContainerConnector.HasComponentDef(typeof(IDataSource)) )
            {
                dataSources[typeof(IDataSource).Name] =
                    (IDataSource)SingletonS2ContainerConnector.GetComponent(typeof(IDataSource));
            }

            return dataSources;
        }

        #region DataSource生成補助メソッド

        /// <summary>
        /// app.configのconnectionStringセクション情報からデータソースを生成する
        /// </summary>
        /// <param name="dataSources"></param>
        private static void SetupByConnectionStringSection(
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
        /// <param name="section">Quill設定情報</param>
        /// <param name="dataSources"></param>
        private static void SetupByQuillSection(QuillSection section,
            IDictionary<string, IDataSource> dataSources)
        {
            if (section != null && section.DataSources.Count > 0)
            {
                foreach (object item in section.DataSources)
                {
                    if(item is DataSourceSection)
                    {
                        DataSourceSection dsSection = (DataSourceSection)item;

                        //  データソース
                        string dataSourceClassName = GetDataSourceClassName(dsSection);

                        //  プロバイダ
                        string providerName = GetProviderName(dsSection);

                        //  接続文字列
                        string connectionString = GetConnectionString(dsSection);

                        //  クラス組み立て
                        DataProvider provider = (DataProvider)ClassUtil.NewInstance(
                            ClassUtil.ForName(providerName));
                        ConstructorInfo constructorInfo = ClassUtil.GetConstructorInfo(ClassUtil.ForName(
                            dataSourceClassName), new Type[] { typeof(DataProvider), typeof(string) });
                        IDataSource dataSource = (IDataSource)constructorInfo.Invoke(
                             new object[] { provider, connectionString });
                        dataSources[dsSection.DataSourceName] = dataSource;
                    }     
                }
            }
        }

        /// <summary>
        /// プロバイダ名の取得
        /// </summary>
        /// <param name="dataSourceSection"></param>
        /// <returns></returns>
        private static string GetProviderName(DataSourceSection dataSourceSection)
        {
            string providerName = dataSourceSection.ProviderName;
            if (string.IsNullOrEmpty(providerName))
            {
                throw new ClassNotFoundRuntimeException("(ProviderName=Empty)");
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
        private static string GetDataSourceClassName(DataSourceSection dataSourceSection)
        {
            string dataSourceClassName = dataSourceSection.DataSourceClassName;
            if (string.IsNullOrEmpty(dataSourceClassName))
            {
                throw new ClassNotFoundRuntimeException("(DataSourceClass=Empty)");
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
        private static string GetConnectionString(DataSourceSection dataSourceSection)
        {
            //  接続文字列
            string configString = dataSourceSection.ConnectionString;
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
    }
}
