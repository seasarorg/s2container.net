using System;

namespace Seasar.Quill.Parts.Container.InstanceFactory
{
    /// <summary>
    /// インスタンスファクトリインターフェース
    /// </summary>
    public interface IInstanceFactory : IDisposable
    {
        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="targetType">取得対象の型</param>
        /// <returns>targetTypeのインスタンス</returns>
        object GetInstance(Type targetType);
    }
}
