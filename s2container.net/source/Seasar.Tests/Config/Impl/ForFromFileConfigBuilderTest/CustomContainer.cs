using Seasar.Quill.Container;
using System;

namespace Seasar.Tests.Config.Impl.ForFromFileConfigBuilderTest
{
    public class CustomContainer : IQuillContainer
    {

        public IImplTypeDetector ImplTypeDetector
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IInstanceScope DefaultScope
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IComponentCreater DefaultComponentCreator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void SetInstanceScopeEachType(Type targetType, IInstanceScope scope)
        {
            throw new NotImplementedException();
        }

        public void SetComponentCreatorEachType(Type targetType, IComponentCreater creator)
        {
            throw new NotImplementedException();
        }

        public T GetComponent<T>()
        {
            throw new NotImplementedException();
        }

        public object GetComponent(Type t)
        {
            throw new NotImplementedException();
        }
    }
}
