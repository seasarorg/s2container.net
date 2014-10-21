using Seasar.Quill.Custom;
using System;
using System.Collections.Concurrent;

namespace Seasar.Quill.Preset.Scope
{
    /// <summary>
    /// コンテナ内でSingletonインスタンス生成
    /// </summary>
    public class ContainerSingletonFactory : IInstanceFactory
    {
        private readonly ConcurrentDictionary<Type, object> _cache
            = new ConcurrentDictionary<Type, object>();

        private readonly Func<Type, object> _valueFactory;

        public ContainerSingletonFactory(Func<Type, object> valueFactory = null)
        {
            _valueFactory = (valueFactory == null ? (t => Activator.CreateInstance(t)) : valueFactory);
        }

        public object CreateInstance(Type targetType)
        {
            return _cache.GetOrAdd(targetType, _valueFactory);
        }
    }
}
