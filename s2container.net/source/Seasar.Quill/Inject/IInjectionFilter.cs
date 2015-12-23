using System;
using System.Reflection;

namespace Quill.Inject {
    /// <summary>
    /// Injectionフィルターインターフェース
    /// </summary>
    public interface IInjectionFilter {
        /// <summary>
        /// Injection対象とするフィールドの抽出条件
        /// </summary>
        /// <returns></returns>
        BindingFlags GetTargetFieldBindinFlags();

        /// <summary>
        /// Injection対象の型か判定
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        bool IsTargetType(Type componentType);

        /// <summary>
        /// Injection対象のフィールドか判定
        /// </summary>
        /// <param name="componentType"></param>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        bool IsTargetField(Type componentType, FieldInfo fieldInfo);
    }
}
