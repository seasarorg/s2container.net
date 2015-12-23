using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Quill.Exception;
using QM = Quill.QuillManager;

namespace Quill.Container.Impl {

    /// <summary>.
    /// Quillコンテナ実装クラス
    /// </summary>
    public class QuillContainer {
        private readonly IDictionary<Type, object> _components;

        /// <summary>
        /// QuillContainer初期化
        /// </summary>
        internal QuillContainer(IDictionary<Type, object> container = null) {
            _components = (container == null ? CreateContainer() : container);
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isCache"></param>
        /// <param name="withInjection"></param>
        /// <returns></returns>
        public virtual T GetComponent<T>(bool isCache = true, bool withInjection = false)
            where T : class {

            Type componentType = GetComponentType(typeof(T));
            if(isCache) {
                if(_components is ConcurrentDictionary<Type, object>) {
                    var components = (ConcurrentDictionary<Type, object>)_components;
                    return (T)components.GetOrAdd(componentType, t => CreateComponent(t, withInjection));
                }

                if(!_components.ContainsKey(componentType)) {
                    _components.Add(componentType, CreateComponent(componentType, withInjection));
                }
                return (T)_components[componentType];
            }
            return (T)CreateComponent(componentType, withInjection);
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="componentType">コンポーネントの型</param>
        /// <param name="isCache"></param>
        /// <param name="withInjection"></param>
        /// <returns></returns>
        public virtual object GetComponent(Type componentType, bool isCache = true, bool withInjection = false) {
            if(isCache) {
                if(_components is ConcurrentDictionary<Type, object>) {
                    var components = (ConcurrentDictionary<Type, object>)_components;
                    return components.GetOrAdd(componentType, t => CreateComponent(t, withInjection));
                }

                if(!_components.ContainsKey(componentType)) {
                    _components.Add(componentType, CreateComponent(componentType, withInjection));
                }
                return _components[componentType];
            }
            return CreateComponent(componentType, withInjection);
        }

        /// <summary>
        /// コンポーネント生成
        /// </summary>
        /// <param name="componentType">コンポーネントの型</param>
        /// <param name="withInjection">true:Injection済のコンポーネントを返す, false:Injectionなし</param>
        /// <returns>生成したコンポーネント</returns>
        protected virtual object CreateComponent(Type componentType, bool withInjection) {
            object component = QM.ComponentCreator.Create(componentType);
            if(withInjection) {
                QM.Injector.Inject(component);
            }
            return component;
        }

        /// <summary>
        /// コンテナの生成
        /// </summary>
        /// <returns></returns>
        protected virtual IDictionary<Type, object> CreateContainer() {
            return new ConcurrentDictionary<Type, object>();
        }

        /// <summary>
        /// コンポーネント型の取得
        /// </summary>
        /// <param name="receiptType"></param>
        /// <returns></returns>
        protected virtual Type GetComponentType(Type receiptType) {
            Type componentType = receiptType;
            if(QM.TypeMap.IsMapped(receiptType)) {
                componentType = QM.TypeMap.GetComponentType(receiptType);
            }

            if(!receiptType.IsAssignableFrom(componentType)) {
                throw new QuillException(QM.Message.GetNotAssignable(receiptType, componentType));
            }
            return componentType;
        }
    }
}
