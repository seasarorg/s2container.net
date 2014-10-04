using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// singletonとしてインスタンスを取得するユーティリティクラス
    /// </summary>
    public static class SingletonUtils
    {
        private static readonly ConcurrentDictionary<Type, object> _instances = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// インスタンス取得
        /// </summary>
        /// <typeparam name="TYPE_TO_SINGLETON">オブジェクトをsingletonで管理する型</typeparam>
        /// <returns>インスタンス取得</returns>
        public static TYPE_TO_SINGLETON GetInstance<TYPE_TO_SINGLETON>() where TYPE_TO_SINGLETON : new()
        {
            var type = typeof(TYPE_TO_SINGLETON);
            if (_instances.ContainsKey(type))
            {
                _instances.TryAdd(type, new TYPE_TO_SINGLETON());
            }

            return (TYPE_TO_SINGLETON)_instances[type];
        }
    }
}
