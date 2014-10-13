using System;
using System.Collections.Concurrent;

namespace Seasar.Quill.Typical.Creation
{
    /// <summary>
    /// singletonとしてインスタンスを取得するstaticなファクトリクラス
    /// </summary>
    public static class SingletonFactory
    {
        /// <summary>
        /// スレッドセーフに型ごとのインスタンスを保持する
        /// </summary>
        private static readonly ConcurrentDictionary<Type, object> _instances;

        /// <summary>
        /// インスタンス生成処理
        /// </summary>
        private static Func<Type, object> _valueFactory;

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static SingletonFactory()
        {
            _instances = new ConcurrentDictionary<Type, object>();
            _valueFactory = CreateInstance;
        }

        /// <summary>
        /// 既定のインスタンス生成処理を設定
        /// </summary>
        /// <param name="valueFactory"></param>
        public static void SetValueFactory(Func<Type, object> valueFactory)
        {
            if (valueFactory == null) { throw new ArgumentNullException("valueFactory"); }
            _valueFactory = valueFactory;
        }

        /// <summary>
        /// キャッシュ済か判定
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static bool IsCached(Type targetType)
        {
            return _instances.ContainsKey(targetType);
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <typeparam name="TYPE_TO_SINGLETON">オブジェクトをsingletonで管理する型</typeparam>
        /// <returns>インスタンス取得</returns>
        public static TYPE_TO_SINGLETON GetInstance<TYPE_TO_SINGLETON>() where TYPE_TO_SINGLETON : new()
        {
            return GetInstance<TYPE_TO_SINGLETON>(null);
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <param name="creator">
        /// インスタンスが未生成だった場合に実行するインスタンス生成処理
        /// （指定なしの場合は引数なしのコンストラクタを呼びだしてインスタンス生成）
        /// </param>
        /// <typeparam name="TYPE_TO_SINGLETON">オブジェクトをsingletonで管理する型</typeparam>
        /// <returns>インスタンス取得</returns>
        public static TYPE_TO_SINGLETON GetInstance<TYPE_TO_SINGLETON>(Func<Type, object> creator) where TYPE_TO_SINGLETON : new()
        {
            var targetType = typeof(TYPE_TO_SINGLETON);
            return (TYPE_TO_SINGLETON)GetInstance(targetType, creator);
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <param name="targetType">取得対象の型</param>
        /// <typeparam name="TYPE_TO_SINGLETON">オブジェクトをsingletonで管理する型</typeparam>
        /// <returns>インスタンス取得</returns>
        public static object GetInstance(Type targetType)
        {
            return GetInstance(targetType, null);
        }

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <param name="targetType">取得対象の型</param>
        /// <param name="creator">
        /// インスタンスが未生成だった場合に実行するインスタンス生成処理
        /// （指定なしの場合は引数なしのコンストラクタを呼びだしてインスタンス生成）
        /// </param>
        /// <typeparam name="TYPE_TO_SINGLETON">オブジェクトをsingletonで管理する型</typeparam>
        /// <returns>インスタンス取得</returns>
        public static object GetInstance(Type targetType, Func<Type, object> creator = null)
        {
            if (targetType == null) { throw new ArgumentNullException("targetType"); }
            var actualCreator = (creator == null ? _valueFactory : creator);
            return _instances.GetOrAdd(targetType, actualCreator);
        }

        /// <summary>
        /// インスタンス生成
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private static object CreateInstance(Type targetType)
        {
            return Activator.CreateInstance(targetType);
        }
    }
}
