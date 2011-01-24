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
using System.Configuration;
using System.Xml;
using Seasar.Framework.Container;
using Seasar.Framework.Util;

namespace Seasar.Framework.Xml
{
    /// <summary>
    /// S2Container.NETの構成セクションハンドラクラスです。
    /// </summary>
    public class S2SectionHandler : IConfigurationSectionHandler
    {
        public static S2Section GetS2Section()
        {
#if NET_1_1
            return (S2Section) ConfigurationSettings.GetConfig(
                ContainerConstants.SEASAR_CONFIG);
#else
            return (S2Section) ConfigurationManager.GetSection(
                ContainerConstants.SEASAR_CONFIG);
#endif
        }

        #region IConfigurationSectionHandler メンバ

        public object Create(object parent, object configContext, XmlNode section)
        {
            return CreateS2Section(section);
        }

        #endregion

        /// <summary>
        /// 外部設定ファイルからQuill設定情報を取得
        /// </summary>
        /// <param name="section">XML形式の設定情報</param>
        /// <returns>Quill設定</returns>
        private static S2Section CreateS2Section(XmlNode section)
        {
            S2Section S2Section = new S2Section();
            S2Section.ConfigPath = ConfigSectionUtil.GetElementValue(
                section, ContainerConstants.CONFIG_PATH_KEY);
            S2Section.Assemblys = GetAssemblyConfig(section);
            return S2Section;
        }

        #region CreateS2Section関連メソッド

        /// <summary>
        /// アセンブリ設定情報の取得
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        private static IList GetAssemblyConfig(XmlNode section)
        {
            return ConfigSectionUtil.GetListConfig(section, 
                ContainerConstants.CONFIG_ASSEMBLYS_KEY,
                ContainerConstants.CONFIG_ASSEMBLY_KEY, 
                Invoke_GetAssemblyConfig);
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
