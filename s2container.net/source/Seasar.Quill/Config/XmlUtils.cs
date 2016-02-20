using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Quill.Exception;
using Quill.Message;
using QM = Quill.QuillManager;

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
            IEnumerable<XElement> childElements = GetChildElements(el, pathParts, 0, isTarget);
            if(childElements.Count() > 0) {
                return childElements.First().Value;
            }
            return null;
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
            IEnumerable<XElement> childElements = GetChildElements(el, pathParts, 0, isTarget);
            if(childElements.Count() > 0) {
                return childElements.First();
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
        /// <param name="targetElement">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <param name="isTarget">取得条件</param>
        /// <returns>取得文字列リスト（取得条件に該当しない場合は空リスト）</returns>
        /// <exception cref="QuillException">パスに該当するノードがない</exception>
        public static List<string> GetChildValues(this XElement targetElement, string nodePath, Func<XElement, bool> isTarget) {
            string[] pathParts = nodePath.Split(DELIMITER_NODE_PATH);
            if(pathParts.Length > 0) {
                var childElements = GetChildElements(targetElement, pathParts, 0, isTarget);
                return childElements.Select(el => el.Value).ToList();
            }

            throw new QuillException(
                string.Format("{0} nodePath={1}", QMsg.NotFoundNodePath.Get(), nodePath));
        }

        /// <summary>
        /// 子ノードの文字列リスト取得
        /// </summary>
        /// <param name="el">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <returns>取得文字列リスト</returns>
        /// <exception cref="ArgumentException">パスに該当するノードがない</exception>
        public static List<XElement> GetChildElements(this XElement el, string nodePath) {
            return GetChildElements(el, nodePath, targetElement => true);
        }

        /// <summary>
        /// 子ノードの文字列リスト取得
        /// </summary>
        /// <param name="targetElement">親ノード</param>
        /// <param name="nodePath">取得したい子ノードのパス（"XXX.XXX.XXX..."）</param>
        /// <param name="isTarget">取得条件</param>
        /// <returns>取得文字列リスト（取得条件に該当しない場合は空リスト）</returns>
        /// <exception cref="QuillException">パスに該当するノードがない</exception>
        public static List<XElement> GetChildElements(this XElement targetElement, string nodePath, Func<XElement, bool> isTarget) {
            string[] pathParts = nodePath.Split(DELIMITER_NODE_PATH);
            if(pathParts.Length > 0) {
                var childElements = GetChildElements(targetElement, pathParts, 0, isTarget);
                return childElements.ToList();
            }

            throw new QuillException(string.Format("{0} nodePath={1}",
                QMsg.NotFoundNodePath.Get(), nodePath));
        }

        /// <summary>
        /// 子ノードの文字列リスト取得（再帰）
        /// </summary>
        /// <param name="currentElement"></param>
        /// <param name="nodePath"></param>
        /// <param name="currentPathIndex"></param>
        /// <param name="isTarget"></param>
        /// <returns></returns>
        private static IEnumerable<XElement> GetChildElements(XElement currentElement, string[] nodePath, int currentPathIndex, Func<XElement, bool> isTarget) {
            string currentNodeName = nodePath[currentPathIndex];
            var childElements = currentElement.Elements().Where(el => el.Name.LocalName == currentNodeName);
            
            bool isLast = (currentPathIndex + 1 >= nodePath.Length);
            if(isLast) {
                return childElements.Where(el => isTarget(el)).ToList();
            }

            var retValues = new List<XElement>();
            foreach(var childElement in childElements) {
                // 再帰的に子孫ノードを探索
                var posterityElements = GetChildElements(childElement, nodePath, currentPathIndex + 1, isTarget);
                if(posterityElements.Count() > 0) {
                    retValues.AddRange(posterityElements);
                }
            }
            return retValues;
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
