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
using System.Xml;

namespace Seasar.Framework.Util
{
    /// <summary>
    /// XmlSerializerの代わりに設定ファイルから設定情報を読み込む
    /// </summary>
	public sealed class ConfigSectionUtil
	{
        /// <summary>
        /// リスト要素取得処理デリゲート
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="node"></param>
        public delegate void DelegateGetListElement(IList collection, XmlNode node);

        /// <summary>
        /// 属性値を取得する
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetAttributeValue(XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes[attributeName];
            string attributeValue = null;
            if (attribute != null)
            {
                attributeValue = attribute.Value;
            }
            return attributeValue;
        }

        /// <summary>
        /// 子要素の値を取得する
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static string GetElementValue(XmlNode parentNode, string childName)
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
        public static IList GetListConfig(XmlNode parentElement, string groupName,
            string childName, DelegateGetListElement invoker)
        {
            if (parentElement == null) throw new ArgumentNullException("parentElement");
            if (groupName == null) throw new ArgumentNullException("groupName");
            if (childName == null) throw new ArgumentNullException("childName");
            if (invoker == null) throw new ArgumentNullException("invoker");

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

            IList retList = new ArrayList();
            foreach (XmlNode node in nodeList)
            {
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    invoker(retList, node);
                }
            }
            return retList;
        }
	}
}
