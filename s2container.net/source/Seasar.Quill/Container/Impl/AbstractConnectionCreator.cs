using System;
using System.Data;

namespace Quill.Container.Impl {
    public abstract class AbstractConnectionCreator : IComponentCreator {
        public virtual object Create(Type componentType) {
            if(!typeof(IDbConnection).IsAssignableFrom(componentType)) {
                throw new ArgumentException(componentType.FullName, "componentType");
            }

            return CreateConnection(componentType);
        }

        protected abstract IDbConnection CreateConnection(Type connectionType);
    }
}
