using Seasar.Quill.Custom;
using System;
using System.Collections.Generic;

namespace Seasar.Quill.Preset.Scope
{
    public class ContainerPreparedSingletonFactory : IInstanceFactory
    {
        private readonly IDictionary<Type, object> _preparedComponents;
        private readonly ContainerSingletonFactory _factory;

        public ContainerPreparedSingletonFactory(IDictionary<Type, object> preparedComponents, Func<Type, object> valueFactory = null)
        {
            if (preparedComponents == null) { throw new ArgumentNullException("preparedComponents"); }

            _preparedComponents = preparedComponents;
            _factory = new ContainerSingletonFactory(valueFactory);
        }

        public object CreateInstance(Type targetType)
        {
            if (_preparedComponents.ContainsKey(targetType))
            {
                return _preparedComponents[targetType];
            }
            return _factory.CreateInstance(targetType);
        }
    }
}
