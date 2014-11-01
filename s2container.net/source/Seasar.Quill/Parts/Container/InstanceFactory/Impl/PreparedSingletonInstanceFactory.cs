using Seasar.Quill.Parts.Container;
using System;
using System.Collections.Generic;

namespace Seasar.Quill.Parts.Container.InstanceFactory.Impl
{
    /// <summary>
    /// Singletonインスタンス生成ファクトリ（生成済オブジェクト指定あり）
    /// </summary>
    public class PreparedSingletonInstanceFactory : IInstanceFactory
    {
        /// <summary>
        /// 生成済コンポーネントのコレクション
        /// </summary>
        private readonly IDictionary<Type, object> _preparedComponents;

        /// <summary>
        /// Singletonインスタンス生成ファクトリ
        /// </summary>
        private readonly SingletonInstanceFactory _factory;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="preparedComponents">生成済コンポーネントのコレクション</param>
        /// <param name="valueFactory">インスタンス生成ファクトリ</param>
        public PreparedSingletonInstanceFactory(IDictionary<Type, object> preparedComponents, Func<Type, object> valueFactory = null)
        {
            if (preparedComponents == null) { throw new ArgumentNullException("preparedComponents"); }

            _preparedComponents = preparedComponents;
            _factory = new SingletonInstanceFactory(valueFactory);
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="targetType">取得対象の型</param>
        /// <returns>targetTypeのインスタンス</returns>
        public object GetInstance(Type targetType)
        {
            if (_preparedComponents.ContainsKey(targetType))
            {
                return _preparedComponents[targetType];
            }
            return _factory.GetInstance(targetType);
        }

        /// <summary>
        /// リソースの破棄
        /// </summary>
        public void Dispose()
        {
            _preparedComponents.Clear();
            _factory.Dispose();
        }
    }
}
