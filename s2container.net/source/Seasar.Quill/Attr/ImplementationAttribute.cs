using System;

namespace Seasar.Quill.Attr
{
    /// <summary>
    /// 実装定義属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ImplementationAttribute : Attribute
    {
        /// <summary>
        /// 実装型（省略時はこの属性を設定したクラスの型をそのまま使用）
        /// </summary>
        /// <exception cref="NotImplementedException">実装型に誤り</exception>
        public Type ImplType { get; set; }
    }

    public static class ImplementationAttrExtension
    {
        public static bool IsImplementationAttrAttached(this Type targetType)
        {
            var attr = GetImplementationAttribute(targetType);
            return attr != null;
        }

        public static Type GetImplType(this Type targetType)
        {
            var attr = GetImplementationAttribute(targetType);
            return attr.ImplType;
        }

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