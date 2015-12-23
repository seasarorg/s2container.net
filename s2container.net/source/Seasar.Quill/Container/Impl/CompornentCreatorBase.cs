using System;
using System.Collections.Generic;

namespace Quill.Container.Impl {
    /// <summary>
    /// 既定のコンポーネント生成クラス
    /// </summary>
    public class CompornentCreatorBase : IComponentCreator {
        private readonly IDictionary<Type, Func<Type, object>> _creatorMap
            = new Dictionary<Type, Func<Type, object>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public virtual object Create(Type componentType) {
            if(componentType == null || componentType == typeof(object)) {
                // TODO インスタンスを生成できない型が指定された場合
                return new object();
            }

            if(_creatorMap.ContainsKey(componentType)) {
                return _creatorMap[componentType](componentType);
            }
            return Activator.CreateInstance(componentType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="func"></param>
        public virtual void AddCreator(Type targetType, Func<Type, object> func) {
            _creatorMap[targetType] = func;
        }
    }
}
