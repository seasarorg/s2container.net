using System;

namespace Seasar.Quill.Attr
{
    /// <summary>
    /// 実装定義属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class ImplementationAttribute : Attribute
    {
        /// <summary>
        /// 実装型（省略時はこの属性を設定したクラスの型をそのまま使用）
        /// </summary>
        public Type ImplType { get; set; }
    }

    /// <summary>
    /// Implementation属性拡張クラス
    /// </summary>
    public static class ImplementationAttrExtension
    {
        /// <summary>
        /// Implementation属性が設定されているか判定
        /// </summary>
        /// <param name="targetType">判定対象の型</param>
        /// <returns>true:Implementation属性あり／false:なし</returns>
        public static bool IsImplementationAttrAttached(this Type targetType)
        {
            var attr = GetImplementationAttribute(targetType);
            return attr != null;
        }

        /// <summary>
        /// 実装型の取得（Implementation属性で定義された実装型を取得）
        /// </summary>
        /// <param name="targetType">取得対象の型</param>
        /// <returns>実装型（定義されていない場合はnull</returns>
        public static Type GetImplType(this Type targetType)
        {
            var attr = GetImplementationAttribute(targetType);
            return attr.ImplType;
        }

        /// <summary>
        /// Implementation属性情報の取得
        /// </summary>
        /// <param name="targetType">取得対象の型</param>
        /// <returns>Implementation属性情報（属性が設定されていない場合はnull）</returns>
        private static ImplementationAttribute GetImplementationAttribute(Type targetType)
        {
            if (targetType == null) { throw new ArgumentNullException("targetType"); }
            var attrs = targetType.GetCustomAttributes(typeof(ImplementationAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                return (ImplementationAttribute)attrs[0];
            }
            return null;
        }
    }
}