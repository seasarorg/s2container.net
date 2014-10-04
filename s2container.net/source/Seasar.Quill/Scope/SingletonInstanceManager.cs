using System;
using System.Collections.Concurrent;

namespace Seasar.Quill.Scope
{
    /// <summary>
    /// 型ごとにsingletonでインスタンス管理するクラス
    /// </summary>
    public class SingletonInstanceManager
    {
        private readonly ConcurrentDictionary<Type, object> _createdInstances = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="implType"></param>
        /// <param name="createInstanceCallback"></param>
        /// <returns></returns>
        public virtual object GetComponent(Type baseType, Type implType, Seasar.Quill.QuillContainer.CreateInstanceCallback createInstanceCallback)
        {
            if (baseType == null)
            {
                throw new ArgumentNullException("baseType");
            }
            if (createInstanceCallback == null)
            {
                throw new ArgumentNullException("createInstanceCallback");
            }

            if (_createdInstances.ContainsKey(baseType))
            {
                return _createdInstances[baseType];
            }

            var component = createInstanceCallback(baseType, (implType == null ? baseType : implType));
            _createdInstances[baseType] = component;
            return component;
        }
    }
}
