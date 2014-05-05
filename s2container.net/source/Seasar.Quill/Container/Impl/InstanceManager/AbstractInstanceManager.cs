using Seasar.Quill.Exception;
using System;

namespace Seasar.Quill.Container.Impl.InstanceManager
{
    public abstract class AbstractInstanceManager : IInstanceManager
    {
        private IComponentCreater[] _creators = null;

        public virtual void SetComponentCreator(params IComponentCreater[] creators)
        {
            if (creators == null || creators.Length == 0)
            {
                throw new ArgumentNullException("creators");
            }
            _creators = creators;
        }

        public virtual object GetInstance(Type t)
        {
            return GetInstance(t, c => c.Create(t));
        }

        public virtual object GetInstance(Type i, Type impl)
        {
            return GetInstance(impl, c => c.Create(i, impl));
        }

        protected virtual object GetInstance(Type t, Func<IComponentCreater, object> createInvoker)
        {
            Validate(t);
            foreach (IComponentCreater creator in _creators)
            {
                if (creator.IsTarget(t))
                {
                    return createInvoker(creator);
                }
            }
            throw new ComponentCreatorNotFoundException(t.FullName);
        }

        protected virtual void Validate(Type t)
        {
            if (_creators == null)
            {
                throw new ArgumentNullException("ComponentCreator");
            }

            if (t == null)
            {
                throw new ArgumentNullException("TargetType");
            }
        }
    }
}
