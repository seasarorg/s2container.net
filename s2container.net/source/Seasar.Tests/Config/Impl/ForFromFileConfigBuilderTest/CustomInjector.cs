using Seasar.Quill.Injection;
using System;

namespace Seasar.Tests.Config.Impl.ForFromFileConfigBuilderTest
{
    public class CustomInjector : IQuillInjector
    {
        public Quill.Container.IQuillContainer Container
        {
            get { throw new NotImplementedException(); }
        }

        public IQuillInjectionContext Context
        {
            get { throw new NotImplementedException(); }
        }

        public void Inject(object root)
        {
            throw new NotImplementedException();
        }

        public void SetContainer(Quill.Container.IQuillContainer container)
        {
            return;
        }

        public void SetInjectionContext(IQuillInjectionContext context)
        {
            return;
        }
    }
}
