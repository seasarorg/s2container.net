using Seasar.Quill.Parts.Container;
using System;

namespace Seasar.Quill.Parts.Container.InstanceFactory.Impl
{
    /// <summary>
    /// インスタンス都度生成ファクトリ
    /// </summary>
    public class PrototypeInstanceFactory : IInstanceFactory
    {
        /// <summary>
        /// インスタンス生成ファクトリ
        /// </summary>
        private readonly Func<Type, object> _valueFactory;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="valueFactory">インスタンス生成ファクトリ</param>
        public PrototypeInstanceFactory(Func<Type, object> valueFactory = null)
        {
            _valueFactory = valueFactory == null ?
                t => Activator.CreateInstance(t) : valueFactory;
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="targetType">取得対象の型</param>
        /// <returns>targetTypeのインスタンス</returns>
        public virtual object GetInstance(Type targetType)
        {
            return _valueFactory(targetType);
        }

        /// <summary>
        /// リソースの破棄
        /// </summary>
        public virtual void Dispose()
        {
            // キャッシュなどはしていないので処理なし
        }
    }
}
