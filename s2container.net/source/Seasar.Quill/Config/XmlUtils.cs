using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Quill.Config {
    /// <summary>
    /// XML読み取りユーティリティクラス
    /// </summary>
    public static class XmlUtils {
        /// <summary>
        /// XMLノードパスの区切り文字
        /// </summary>
        private const char DELIMITER_NODE_PATH = '.';

        /// <summary>
        /// 子ノードの文字列取得
        /// </summary>
        /// <param name="el">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <returns>取得文字列</returns>
        /// <exception cref="ArgumentException">パスに該当するノードがない</exception>
        public static string GetChildValue(this XElement el, string nodePath) {
            return GetChildValue(el, nodePath, targetElement => true);
        }

        /// <summary>
        /// 子ノードの文字列取得
        /// </summary>
        /// <param name="el">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <param name="isTarget">取得条件</param>
        /// <returns>取得文字列（取得条件に該当しない場合は空文字列）</returns>
        /// <exception cref="ArgumentException">パスに該当するノードがない</exception>
        public static string GetChildValue(this XElement el, string nodePath, Func<XElement, bool> isTarget) {
            string[] pathParts = nodePath.Split(DELIMITER_NODE_PATH);
            XElement currentElement = el;
            foreach(string pathPart in pathParts) {
                var elements = currentElement.Elements().Where(childElement => (childElement.Name.LocalName == pathPart));
                if(elements.Count() == 0) {
                    throw new ArgumentException(nodePath, "nodePath");
                }

                currentElement = elements.First();
            }

            if(isTarget(currentElement)) {
                return currentElement.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 子ノード取得
        /// </summary>
        /// <param name="el">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <returns>子ノード（取得条件に該当しない場合はnull）</returns>
        /// <exception cref="ArgumentException">パスに該当するノードがない</exception>
        public static XElement GetChildElement(this XElement el, string nodePath) {
            return GetChildElement(el, nodePath, targetElement => true);
        }

        /// <summary>
        /// 子ノード取得
        /// </summary>
        /// <param name="el">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <param name="isTarget">取得条件</param>
        /// <returns>子ノード（取得条件に該当しない場合はnull）</returns>
        /// <exception cref="ArgumentException">パスに該当するノードがない</exception>
        public static XElement GetChildElement(this XElement el, string nodePath, Func<XElement, bool> isTarget) {
            string[] pathParts = nodePath.Split(DELIMITER_NODE_PATH);
            XElement currentElement = el;
            foreach(string pathPart in pathParts) {
                var elements = currentElement.Elements().Where(childElement => (childElement.Name.LocalName == pathPart));
                if(elements.Count() == 0) {
                    throw new ArgumentException(nodePath, "nodePath");
                }

                currentElement = elements.First();
            }

            if(isTarget(currentElement)) {
                return currentElement;
            }
            return null;
        }

        /// <summary>
        /// 子ノードの文字列リスト取得
        /// </summary>
        /// <param name="el">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <returns>取得文字列リスト</returns>
        /// <exception cref="ArgumentException">パスに該当するノードがない</exception>
        public static List<string> GetChildValues(this XElement el, string nodePath) {
            return GetChildValues(el, nodePath, targetElement => true);
        }

        /// <summary>
        /// 子ノードの文字列リスト取得
        /// </summary>
        /// <param name="el">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <param name="isTarget">取得条件</param>
        /// <returns>取得文字列リスト（取得条件に該当しない場合は空リスト）</returns>
        /// <exception cref="ArgumentException">パスに該当するノードがない</exception>
        public static List<string> GetChildValues(this XElement el, string nodePath, Func<XElement, bool> isTarget) {
            string[] pathParts = nodePath.Split(DELIMITER_NODE_PATH);
            XElement currentElement = el;
            // 最後の一つ手前まで
            for(int i = 0; i < pathParts.Length; i++) {
                string pathPart = pathParts[i];
                var elements = currentElement.Elements().Where(childElement => childElement.Name.LocalName == pathPart);
                if(elements.Count() == 0) {
                    throw new ArgumentException(nodePath, "nodePath");
                }

                if(i < pathPart.Length - 1) {
                    currentElement = elements.First();
                } else {
                    var targetElements = currentElement.Elements().Where(childElement => isTarget(childElement)); 
                    return targetElements.Select(targetElement => targetElement.Value).ToList();
                }
            }
            // 名前指定なし
            return el.Elements().Select(childElement => childElement.Value).ToList();
        }

        /// <summary>
        /// 属性値の取得
        /// </summary>
        /// <param name="el">取得対象ノード</param>
        /// <param name="attrName">属性名</param>
        /// <returns>属性値</returns>
        /// <exception cref="ArgumentException">属性名に該当する属性が定義されていない</exception>
        public static string GetAttrValue(this XElement el, string attrName) {
            var targetAttr = el.Attribute(attrName);
            if(targetAttr == null) {
                throw new ArgumentException(attrName, "attrName");
            }
            return targetAttr.Value;
        }
    }
}
