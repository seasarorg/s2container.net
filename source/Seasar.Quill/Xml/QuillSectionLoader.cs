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

using System.Collections;
using System.IO;
using System.Xml;

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
            quillSection.DaoSetting = GetElementValue(section, QuillConstants.CONFIG_DAO_SETTING_KEY);
            quillSection.TransactionSetting = GetElementValue(section, QuillConstants.CONFIG_TX_SETTING_KEY);
            return quillSection;
        }

        #region LoadFromOuterConfig関連メソッド

        /// <summary>
        /// データソース設定情報の取得
        /// </summary>
        /// <param name="quillElement"></param>
        /// <returns></returns>
        private static IList GetDataSourceConfig(XmlNode quillElement)
        {
            return GetListConfig(quillElement, QuillConstants.CONFIG_DATASOURCES_KEY,
                QuillConstants.CONFIG_DATASOURCE_KEY, Invoke_GetDataSourceConfig);
        }


        /// <summary>
        /// アセンブリ設定情報の取得
        /// </summary>
        /// <param name="quillElement"></param>
        /// <returns></returns>
        private static IList GetAssemblyConfig(XmlNode quillElement)
        {
            return GetListConfig(quillElement, QuillConstants.CONFIG_ASSEMBLYS_KEY,
                QuillConstants.CONFIG_ASSEMBLY_KEY, Invoke_GetAssemblyConfig);
        }

        /// <summary>
        /// アセンブリ設定取得処理デリゲート
        /// </summary>
        /// <param name="list"></param>
        /// <param name="node"></param>
        protected delegate void GetNodeConfig(IList list, XmlNode node);

        /// <summary>
        /// データソース設定取得処理デリゲート
        /// </summary>
        /// <param name="list"></param>
        /// <param name="node"></param>
        private static void Invoke_GetDataSourceConfig(IList list, XmlNode node)
        {
            DataSourceSection dsSection = new DataSourceSection();

            //  データソース名
            XmlAttribute dsNameAttr = node.Attributes[QuillConstants.CONFIG_DATASOURCE_NAME_ATTR];
            string dataSourceName = null;
            if (dsNameAttr != null)
            {
                dataSourceName = dsNameAttr.Value;
            }
            dsSection.DataSourceName = dataSourceName;

            //  接続文字列
            dsSection.ConnectionString = GetElementValue(node, QuillConstants.CONFIG_CONNECTION_STRING_KEY);

            //  プロバイダ
            dsSection.ProviderName = GetElementValue(node, QuillConstants.CONFIG_PROVIDER);

            //  DataSourceクラス名
            dsSection.DataSourceClassName = GetElementValue(node, QuillConstants.CONFIG_DATASOURCE_CLASS_KEY);

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

        /// <summary>
        /// 子要素の値を取得する
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        private static string GetElementValue(XmlNode parentNode, string childName)
        {
            XmlElement retElement = parentNode[childName];
            string retString = null;
            if (retElement != null)
            {
                retString = retElement.InnerText;
            }
            return retString;
        }

        /// <summary>
        /// リストで定義された設定情報を取得する
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="groupName">親要素の名前</param>
        /// <param name="childName">子要素の名前</param>
        /// <param name="invoker"></param>
        /// <returns></returns>
        private static IList GetListConfig(XmlNode parentElement, string groupName,
            string childName, GetNodeConfig invoker)
        {
            XmlElement element = parentElement[groupName];
            if (element == null)
            {
                return null;
            }
            XmlNodeList nodeList = element.GetElementsByTagName(childName);
            if (nodeList.Count == 0)
            {
                return null;
            }

            ArrayList retList = new ArrayList();
            foreach (XmlNode node in nodeList)
            {
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    invoker(retList, node);
                }
            }
            return retList;
        }
        #endregion
    }
}
