using System;
using System.Collections.Generic;
using System.Reflection;
using QM = Quill.QuillManager;

namespace Quill.Inject.Impl {
    /// <summary>
    /// 既定のInjectionフィルタークラス
    /// </summary>
    public class InjectionFilterBase : IInjectionFilter {
        private const string SOURCE_IS_TARGET_TYPE = "InjectionFilterBase#IsTargetType";

        /// <summary>
        /// Injection除外対象型セット
        /// </summary>
        public virtual ISet<Type> NotInjectionTargetTypes { get; protected set; }

        /// <summary>
        /// Injection対象型セット
        /// </summary>
        public virtual ISet<Type> InjectionTargetTypes { get; protected set; }

        /// <summary>
        /// デフォルトでInjection対象に含めるか？（true（既定値）:含める, false:含めない)
        /// </summary>
        public virtual bool IsTargetTypeDefault { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InjectionFilterBase() {
            NotInjectionTargetTypes = new HashSet<Type>();
            InjectionTargetTypes = new HashSet<Type>();
            IsTargetTypeDefault = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual BindingFlags GetTargetFieldBindinFlags() {
            return BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentType"></param>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public virtual bool IsTargetField(Type componentType, FieldInfo fieldInfo) {
            return IsTargetType(fieldInfo.FieldType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public virtual bool IsTargetType(Type componentType) {
            if(InjectionTargetTypes.Contains(componentType)) {
                QM.OutputLog(SOURCE_IS_TARGET_TYPE, Message.EnumMsgCategory.DEBUG,
                    string.Format("[{0}] is injection target.", 
                    componentType == null ? "null" : componentType.Name));
                return true;
            }

            if(NotInjectionTargetTypes.Contains(componentType)) {
                QM.OutputLog(SOURCE_IS_TARGET_TYPE, Message.EnumMsgCategory.DEBUG,
                    string.Format("[{0}] is not injection target.",
                    componentType == null ? "null" : componentType.Name));
                return false;
            }

            QM.OutputLog(SOURCE_IS_TARGET_TYPE, Message.EnumMsgCategory.DEBUG,
                    string.Format("[{0}]:isInjectionTarget:{1}", 
                    componentType == null ? "null" : componentType.Name, 
                    IsTargetTypeDefault));
            return IsTargetTypeDefault;
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        public virtual void Dispose() {
            NotInjectionTargetTypes.Clear();
            InjectionTargetTypes.Clear();
        }
    }
}
