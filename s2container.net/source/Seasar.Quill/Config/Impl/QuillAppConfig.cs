using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Quill.Config.Impl {
    public class QuillAppConfig : IQuillConfig {
        private readonly QuillConfigImpl _config = new QuillConfigImpl();
        public virtual XElement BaseElement {
            get { return _config.BaseElement; }
        }

        public virtual void Load() {
            _config.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        public virtual XElement GetElement(string nodePath) {
            return _config.GetElement(nodePath);
        }

        public virtual XElement GetElement(string nodePath, Func<XElement, bool> isTarget) {
            return _config.GetElement(nodePath, isTarget);
        }

        public virtual string GetValue(string nodePath) {
            return _config.GetValue(nodePath);
        }

        public virtual string GetValue(string nodePath, Func<XElement, bool> isTarget) {
            return _config.GetValue(nodePath, isTarget);
        }

        public virtual List<string> GetValues(string nodePath) {
            return _config.GetValues(nodePath);
        }

        public virtual List<string> GetValues(string nodePath, Func<XElement, bool> isTarget) {
            return _config.GetValues(nodePath, isTarget);
        }
    }
}
