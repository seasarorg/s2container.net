using Seasar.Quill.Parts.Container.InstanceFactory.Impl;
using System;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// Singletonインスタンス保持staticクラス
    /// </summary>
    public static class SingletonInstances
    {
        /// <summary>
        /// Singletonインスタンス取得ファクトリ
        /// </summary>
        private static SingletonInstanceFactory _factory = new SingletonInstanceFactory();

        /// <summary>
        /// インスタンス生成ファクトリの設定
        /// </summary>
        /// <param name="valueFactory">Singletonインスタンス取得ファクトリ</param>
        public static void SetValueFactory(Func<Type, object> valueFactory)
        {
            if (valueFactory == null) { throw new ArgumentNullException("valueFactory"); }
            _factory = new SingletonInstanceFactory(valueFactory);
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <typeparam name="RECEIPT_TYPE">受け取り側の型</typeparam>
        /// <returns>インスタンス</returns>
        public static RECEIPT_TYPE GetInstance<RECEIPT_TYPE>() where RECEIPT_TYPE : new()
        {
            return (RECEIPT_TYPE)_factory.GetInstance(typeof(RECEIPT_TYPE));
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <typeparam name="RECEIPT_TYPE">受け取り側の型</typeparam>
        /// <param name="targetType">生成インスタンスの実装型</param>
        /// <returns>インスタンス</returns>
        public static RECEIPT_TYPE GetInstance<RECEIPT_TYPE>(this Type targetType)
        {
            return (RECEIPT_TYPE)_factory.GetInstance(targetType);
        }

        /// <summary>
        /// 保持しているファクトリのクリア
        /// </summary>
        public static void Clear()
        {
            _factory.Dispose();
            _factory = new SingletonInstanceFactory();
        }
    }
}
