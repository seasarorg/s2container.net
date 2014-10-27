using Seasar.Quill.Parts.Container;
using System;
using System.Collections.Concurrent;

namespace Seasar.Quill.Parts.Container.InstanceFactory.Impl
{
    /// <summary>
    /// コンテナ内でSingletonインスタンス生成
    /// </summary>
    public class SingletonInstanceFactory : IInstanceFactory
    {
        /// <summary>
        /// キャッシュしたインスタンスのコレクション（スレッドセーフ）
        /// </summary>
        private readonly ConcurrentDictionary<Type, object> _instances
            = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// インスタンス生成ファクトリ
        /// </summary>
        private readonly Func<Type, object> _valueFactory;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="valueFactory">インスタンス生成ファクトリ</param>
        public SingletonInstanceFactory(Func<Type, object> valueFactory = null)
        {
            _valueFactory = (valueFactory == null ? (t => Activator.CreateInstance(t)) : valueFactory);
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="targetType">取得対象の型</param>
        /// <returns>targetTypeのインスタンス</returns>
        public virtual object GetInstance(Type targetType)
        {
            if (targetType == null) { throw new ArgumentNullException("targetType"); }
            return _instances.GetOrAdd(targetType, _valueFactory);
        }

        public virtual void Dispose()
        {
            _instances.Clear();
        }
    }
}
