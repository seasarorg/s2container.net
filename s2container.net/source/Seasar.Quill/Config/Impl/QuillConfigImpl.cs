using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Quill.Exception;

namespace Quill.Config.Impl {
    public class QuillConfigImpl : IQuillConfig {
        protected const string BASE_SECTION_NAME = "quill";

        public virtual XElement BaseElement { get; protected set; }

        public virtual void Load(string path) {
            if(path == null) {
                // TODO 例外メッセージ
                throw new ArgumentNullException("path");
            }

            if(!File.Exists(path)) {
                // TODO 例外メッセージ
                throw new FileNotFoundException("", path);
            }

            
            try {
                XDocument doc = XDocument.Load(path);
                BaseElement = GetBaseElement(doc);
            } catch(System.Exception ex) {
                // TODO 例外メッセージの設定
                throw new QuillException("", ex);
            }
        }

        public virtual string GetValue(string nodePath) {
            return GetValue(nodePath, e => true);
        }

        public virtual string GetValue(string nodePath, Func<XElement, bool> isTarget) {
            ValidateExistsBaseElement();
            return BaseElement.GetChildValue(nodePath, isTarget);
        }

        public virtual List<string> GetValues(string nodePath) {
            return GetValues(nodePath, e => true);
        }

        public virtual List<string> GetValues(string nodePath, Func<XElement, bool> isTarget) {
            ValidateExistsBaseElement();
            return BaseElement.GetChildValues(nodePath, isTarget);
        }

        public virtual XElement GetElement(string nodePath) {
            return GetElement(nodePath, e => true);
        }

        public virtual XElement GetElement(string nodePath, Func<XElement, bool> isTarget) {
            ValidateExistsBaseElement();
            return BaseElement.GetChildElement(nodePath, isTarget);
        }

        protected virtual XElement GetBaseElement(XDocument doc) {
            return doc.Elements(BASE_SECTION_NAME).First();
        }

        protected virtual void ValidateExistsBaseElement() {
            if(BaseElement == null) {
                // TODO 未初期化例外
                throw new QuillException("");
            }
        }
    }
}
