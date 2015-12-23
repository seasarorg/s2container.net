using System;
using System.Collections.Generic;
using System.Reflection;

namespace Quill.Inject.Impl {
    /// <summary>
    /// 既定のInjectionフィルタークラス
    /// </summary>
    public class InjectionFilterBase : IInjectionFilter {
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
                return true;
            }

            if(NotInjectionTargetTypes.Contains(componentType)) {
                return false;
            }

            return IsTargetTypeDefault;
        }
    }
}
