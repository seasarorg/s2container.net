using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Quill.Exception;
using Quill.Message;

namespace Quill.Config.Impl {
    /// <summary>
    /// アプリケーション構成ファイルから読み取ったQuill設定クラス
    /// </summary>
    public class QuillAppConfig : QuillConfigImpl {
        /// <summary>
        /// app.configのルートセクション名
        /// </summary>
        private const string APP_ROOT_SECTION_NAME = "configuration";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="baseElement">quill設定の最上位要素</param>
        protected QuillAppConfig(XElement baseElement) : base(baseElement) { }

        /// <summary>
        /// 設定の読み込み
        /// </summary>
        /// <returns>Quill設定情報</returns>
        public static IQuillConfig Load() {
            string path = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            if(!File.Exists(path)) {
                throw new FileNotFoundException(QMsg.FileNotFound.Get(), path);
            }

            try {
                XDocument doc = XDocument.Load(path);
                return new QuillAppConfig(GetBaseElement(doc, path));
            } catch(System.Exception ex) {
                throw new QuillException(
                    string.Format("{0} path={1}", QMsg.ErrorLoadingConfig.Get(), path), ex);
            }
        }

        /// <summary>
        /// Quill設定の先頭ノードを取得
        /// </summary>
        /// <param name="doc">XMLドキュメント</param>
        /// <param name="path">XMLファイルパス</param>
        /// <returns>Quill設定要素</returns>
        private static XElement GetBaseElement(XDocument doc, string path) {
            IEnumerable<XElement> rootElements = doc.Elements(APP_ROOT_SECTION_NAME);
            if(rootElements == null || rootElements.Count() == 0) {
                throw new QuillException(
                    string.Format("{0} secion={1}, path={2}",
                    QMsg.NotFoundRequireSection.Get(),
                    APP_ROOT_SECTION_NAME, path));
            }

            IEnumerable<XElement> quillElements = 
                rootElements.First().Elements(BASE_SECTION_NAME);
            if(quillElements == null || quillElements.Count() == 0) {
                throw new QuillException(
                    string.Format("{0} secion={1}, path={2}",
                    QMsg.NotFoundRequireSection.Get(),
                    BASE_SECTION_NAME, path));
            }

            return quillElements.First();
        }
    }
}
