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

using System.Collections;
using System.IO;
using System.Xml;
using Seasar.Framework.Util;

namespace Seasar.Quill.Xml
{
    /// <summary>
    /// Quill設定の読込・取得を行う
    /// </summary>
    public class QuillSectionLoader
    {
        /// <summary>
        /// 外部設定ファイルからQuill設定情報を取得
        /// </summary>
        /// <param name="path">設定ファイルのパス</param>
        /// <returns>Quill設定</returns>
        public static QuillSection LoadFromOuterConfig(string path)
        {
            XmlDocument quillDoc = new XmlDocument();
            //  設定ファイルが存在しない場合はnullとする
            if (path == null || File.Exists(path) == false)
            {
                return null;
            }
            quillDoc.Load(path);

            //  quillセクションが含まれない場合もnullとする
            XmlElement quillElement = quillDoc[QuillConstants.SECTION_ROOT][QuillConstants.QUILL_CONFIG];
            if (quillElement == null)
            {
                return null;
            }

            return CreateQuillSection(quillElement);
        }

        /// <summary>
        /// 外部設定ファイルからQuill設定情報を取得
        /// </summary>
        /// <param name="section">XML形式の設定情報</param>
        /// <returns>Quill設定</returns>
        public static QuillSection CreateQuillSection(XmlNode section)
        {
            QuillSection quillSection = new QuillSection();
            quillSection.Assemblys = GetAssemblyConfig(section);
            quillSection.DataSources = GetDataSourceConfig(section);
            quillSection.DaoSetting = ConfigSectionUtil.GetElementValue(
                section, QuillConstants.CONFIG_DAO_SETTING_KEY);
            quillSection.TransactionSetting = ConfigSectionUtil.GetElementValue(
                section, QuillConstants.CONFIG_TX_SETTING_KEY);
            return quillSection;
        }

        #region CreateQuillSection関連メソッド

        /// <summary>
        /// データソース設定情報の取得
        /// </summary>
        /// <param name="quillElement"></param>
        /// <returns></returns>
        private static IList GetDataSourceConfig(XmlNode quillElement)
        {
            return ConfigSectionUtil.GetListConfig(quillElement, QuillConstants.CONFIG_DATASOURCES_KEY,
                QuillConstants.CONFIG_DATASOURCE_KEY, Invoke_GetDataSourceConfig);
        }


        /// <summary>
        /// アセンブリ設定情報の取得
        /// </summary>
        /// <param name="quillElement"></param>
        /// <returns></returns>
        private static IList GetAssemblyConfig(XmlNode quillElement)
        {
            return ConfigSectionUtil.GetListConfig(quillElement, QuillConstants.CONFIG_ASSEMBLYS_KEY,
                QuillConstants.CONFIG_ASSEMBLY_KEY, Invoke_GetAssemblyConfig);
        }

        /// <summary>
        /// データソース設定取得処理デリゲート
        /// </summary>
        /// <param name="list"></param>
        /// <param name="node"></param>
        private static void Invoke_GetDataSourceConfig(IList list, XmlNode node)
        {
            DataSourceSection dsSection = new DataSourceSection();

            //  データソース名
            dsSection.DataSourceName = ConfigSectionUtil.GetAttributeValue(
                node, QuillConstants.CONFIG_DATASOURCE_NAME_ATTR);

            //  接続文字列
            dsSection.ConnectionString = ConfigSectionUtil.GetElementValue(
                node, QuillConstants.CONFIG_CONNECTION_STRING_KEY);

            //  プロバイダ
            dsSection.ProviderName = ConfigSectionUtil.GetElementValue(
                node, QuillConstants.CONFIG_PROVIDER);

            //  DataSourceクラス名
            dsSection.DataSourceClassName = ConfigSectionUtil.GetElementValue(
                node, QuillConstants.CONFIG_DATASOURCE_CLASS_KEY);

            list.Add(dsSection);
        }

        /// <summary>
        /// アセンブリ設定取得処理デリゲート
        /// </summary>
        /// <param name="list"></param>
        /// <param name="node"></param>
        private static void Invoke_GetAssemblyConfig(IList list, XmlNode node)
        {
            list.Add(node.InnerText);
        }

        
        #endregion
    }
}
