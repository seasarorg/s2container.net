using Seasar.Quill.Attr;
using Seasar.Quill.Container;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Seasar.Quill.Injection.Impl
{
    public abstract class AbstractQuillInjector : IQuillInjector
    {
        private readonly IDictionary<Type, bool> _implementTypeMap = new Dictionary<Type, bool>();
        protected readonly IQuillContainer _container;

        protected AbstractQuillInjector(IQuillContainer container)
        {
            _container = container;
        }

        public virtual void Inject(object root)
        {
            Inject(root, true);
        }

        public virtual void Inject(object root, bool isImplementationOnly)
        {
            var fields = root.GetType().GetFields(GetFieldFilter());
            if (fields == null || fields.Length == 0)
            {
                return;
            }

            foreach (var field in fields)
            {
                InjectField(root, field, isImplementationOnly);
            }
        }

        public virtual T CreateInjectedInstance<T>()
        {
            return CreateInjectedInstance<T>(true);
        }

        public virtual I CreateInjectedInstance<I, T>() where T : I
        {
            return CreateInjectedInstance<I, T>(true);
        }

        public virtual T CreateInjectedInstance<T>(bool isImplementationOnly)
        {
            var component = _container.GetComponent<T>();
            Inject(component, isImplementationOnly);
            return component;
        }

        public virtual I CreateInjectedInstance<I, T>(bool isImplementationOnly) where T : I
        {
            var component = _container.GetComponent<I, T>();
            Inject(component, isImplementationOnly);
            return component;
        }

        protected virtual void InjectField(object target, FieldInfo field, bool isImplementationOnly)
        {
            var component = _container.GetComponent(field.FieldType);
            if (component == null)
            {
                return;
            }

            if (isImplementationOnly)
            {
                if (!_implementTypeMap.ContainsKey(field.FieldType))
                {
                    var targetAttributes = component.GetType().GetCustomAttributes(typeof(ImplementationAttribute));
                    _implementTypeMap.Add(field.FieldType, targetAttributes != null);
                }
                    
                if (!_implementTypeMap[field.FieldType])
                {
                    // Implementation属性がついていない型のフィールドはInjectの対象としない
                    return;
                }
            }
            // 再帰的に設定
            Inject(component, isImplementationOnly);
            field.SetValue(target, component);
        }

        protected abstract BindingFlags GetFieldFilter();
        protected abstract bool IsRecursive();
    }
}
