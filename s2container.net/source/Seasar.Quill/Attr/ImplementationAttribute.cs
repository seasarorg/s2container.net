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

        /// <summary>
        /// インスタンス生成クラスの型（要IComponentCreator実装）
        /// </summary>
        /// <exception cref="NotImplementedException">実装型に誤り</exception>
        public Type ComponentCreatorType { get; set; }

        /// <summary>
        /// インスタンス管理クラスの型（要IInstanceManager実装）
        /// </summary>
        /// <exception cref="NotImplementedException">実装型に誤り</exception>
        public Type InstanceManagerType { get; set; } 
    }
}