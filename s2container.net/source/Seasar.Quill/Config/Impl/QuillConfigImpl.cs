using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Quill.Exception;
using QM = Quill.QuillManager;

namespace Quill.Config.Impl {
    /// <summary>
    /// Quill設定実装クラス
    /// </summary>
    public class QuillConfigImpl : IQuillConfig {
        /// <summary>
        /// 基底設定セクション名
        /// </summary>
        protected const string BASE_SECTION_NAME = "quill";

        /// <summary>
        /// 基底設定セクション名
        /// </summary>
        public virtual XElement BaseElement { get; protected set; }

        /// <summary>
        /// コンストラクタ（内部からのみ呼び出し可能）
        /// </summary>
        protected QuillConfigImpl(XElement baseElement) {
            BaseElement = baseElement;
        }

        /// <summary>
        /// 設定の読み込み
        /// </summary>
        /// <param name="path">設定ファイルパス</param>
        /// <returns>Quill設定情報</returns>
        public static IQuillConfig Load(string path) {
            if(path == null) {
                throw new ArgumentNullException("path");
            }

            if(!File.Exists(path)) {
                throw new FileNotFoundException(QM.Message.GetFileNotFound(), path);
            }

            try {
                XDocument doc = XDocument.Load(path);
                return new QuillConfigImpl(GetBaseElement(doc, path));
            } catch(System.Exception ex) {
                throw new QuillException(QM.Message.GetErrorLoadingConfig(path), ex);
            }
        }

        /// <summary>
        /// ノードパスに該当する設定値を取得
        /// </summary>
        /// <param name="nodePath">階層を"."で区切ったノード階層パス（XXX.XXX.XXX）</param>
        /// <returns>設定値</returns>
        public virtual string GetValue(string nodePath) {
            return GetValue(nodePath, e => true);
        }

        /// <summary>
        /// ノードパスに該当する設定値を取得
        /// </summary>
        /// <param name="nodePath">階層を"."で区切ったノード階層パス（XXX.XXX.XXX）</param>
        /// <param name="isTarget">ノードパス以外のノード検索条件</param>
        /// <returns>設定値</returns>
        public virtual string GetValue(string nodePath, Func<XElement, bool> isTarget) {
            return BaseElement.GetChildValue(nodePath, isTarget);
        }

        /// <summary>
        /// ノードパスに該当する設定値を取得（複数）
        /// </summary>
        /// <param name="nodePath">階層を"."で区切ったノード階層パス（XXX.XXX.XXX）</param>
        /// <returns>設定値</returns>
        public virtual List<string> GetValues(string nodePath) {
            return GetValues(nodePath, e => true);
        }

        /// <summary>
        /// ノードパスに該当する設定値を取得（複数）
        /// </summary>
        /// <param name="nodePath">階層を"."で区切ったノード階層パス（XXX.XXX.XXX）</param>
        /// <param name="isTarget">ノードパス以外のノード検索条件</param>
        /// <returns>設定値</returns>
        public virtual List<string> GetValues(string nodePath, Func<XElement, bool> isTarget) {
            return BaseElement.GetChildValues(nodePath, isTarget);
        }

        /// <summary>
        /// ノードパスに該当するノード情報を取得
        /// </summary>
        /// <param name="nodePath">階層を"."で区切ったノード階層パス（XXX.XXX.XXX）</param>
        /// <returns>ノード情報</returns>
        public virtual XElement GetElement(string nodePath) {
            return GetElement(nodePath, e => true);
        }

        /// <summary>
        /// ノードパスに該当するノード情報を取得
        /// </summary>
        /// <param name="nodePath">階層を"."で区切ったノード階層パス（XXX.XXX.XXX）</param>
        /// <param name="isTarget">ノードパス以外のノード検索条件</param>
        /// <returns>ノード情報</returns>
        public virtual XElement GetElement(string nodePath, Func<XElement, bool> isTarget) {
            return BaseElement.GetChildElement(nodePath, isTarget);
        }

        /// <summary>
        /// ノードパスに該当するノード情報を取得（複数）
        /// </summary>
        /// <param name="nodePath">階層を"."で区切ったノード階層パス（XXX.XXX.XXX）</param>
        /// <returns>ノード情報</returns>
        public virtual List<XElement> GetElements(string nodePath) {
            return GetElements(nodePath, e => true);
        }

        /// <summary>
        /// ノードパスに該当するノード情報を取得（複数）
        /// </summary>
        /// <param name="nodePath">階層を"."で区切ったノード階層パス（XXX.XXX.XXX）</param>
        /// <param name="isTarget">ノードパス以外のノード検索条件</param>
        /// <returns>ノード情報</returns>
        public virtual List<XElement> GetElements(string nodePath, Func<XElement, bool> isTarget) {
            return BaseElement.GetChildElements(nodePath, isTarget);
        }

        /// <summary>
        /// Quill設定の最上位ノードを取得
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path">設定ファイルパス</param>
        /// <returns>Quill設定の最上位ノード</returns>
        private static XElement GetBaseElement(XDocument doc, string path) {
            IEnumerable<XElement> quillElements = doc.Elements(BASE_SECTION_NAME);
            if(quillElements == null || quillElements.Count() == 0) {
                throw new QuillException(QM.Message.GetNotFoundRequireSection(
                    BASE_SECTION_NAME, path));
            }

            return quillElements.First();
        }
    }
}
