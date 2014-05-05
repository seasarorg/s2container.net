using Seasar.Quill.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Container.Impl
{
    public class QuillContainer : IQuillContainer
    {
        private IInstanceManager _instanceManager = null;
        private IImplTypeDetector[] _detectors = null;

        public virtual void SetInstanceManager(IInstanceManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            _instanceManager = manager;
        }

        public virtual void SetImplTypeDetector(params IImplTypeDetector[] detectors)
        {
            if (detectors == null || detectors.Length == 0)
            {
                throw new ArgumentNullException("detectors");
            }
            _detectors = detectors;
        }

        public virtual T GetComponent<T>()
        {
            return (T)GetComponent(typeof(T));
        }

        public virtual IF GetComponent<IF, IMPL>() where IMPL : IF
        {
            return (IF)GetComponent(typeof(IF), typeof(IMPL));
        }

        public virtual object GetComponent(Type t)
        {
            Type implType = GetImplType(t);
            if (implType == null)
            {
                throw new ImplementTypeNotFoundException(t.FullName);
            }

            if (t == implType)
            {
                return _instanceManager.GetInstance(implType);    
            }
            return _instanceManager.GetInstance(t, implType);
        }

        public virtual object GetComponent(Type i, Type t)
        {
            return _instanceManager.GetInstance(i, t);
        }

        protected virtual Type GetImplType(Type baseType)
        {
            if (baseType.IsClass)
            {
                return baseType;
            }

            if (baseType.IsInterface || baseType.IsInterface)
            {
                foreach(IImplTypeDetector detector in _detectors)
                {
                    Type implType = detector.GetImplType(baseType);
                    if (implType != null)
                    {
                        return implType;
                    }
                }
            }
            return null;
        }
    }
}
