using System;
using System.Reflection;

namespace Quill.Inject {
    /// <summary>
    /// Injectionフィルターインターフェース
    /// </summary>
    public interface IInjectionFilter : IDisposable {
        /// <summary>
        /// Injection対象とするフィールドの抽出条件取得
        /// </summary>
        /// <returns>Injection対象とするフィールドの抽出条件</returns>
        BindingFlags GetTargetFieldBindinFlags();

        /// <summary>
        /// Injection対象の型か判定
        /// </summary>
        /// <param name="componentType">判定対象の型</param>
        /// <returns>true:Injection対象, false:非対象</returns>
        bool IsTargetType(Type componentType);

        /// <summary>
        /// Injection対象のフィールドか判定
        /// </summary>
        /// <param name="componentType">判定対象の型</param>
        /// <param name="fieldInfo">フィールド情報</param>
        /// <returns></returns>
        bool IsTargetField(Type componentType, FieldInfo fieldInfo);
    }
}
