
using System;
namespace Seasar.Quill.Injection
{
    public interface IQuillInjector
    {
        void Inject(object root);
        void Inject(object root, bool isImplementationOnly);
        object CreateInjectedInstance(Type t);
        object CreateInjectedInstance(Type t, bool isImplementationOnly);
        object CreateInjectedInstance(Type i, Type t);
        object CreateInjectedInstance(Type i, Type t, bool isImplementationOnly);
        T CreateInjectedInstance<T>();
        I CreateInjectedInstance<I, T>() where T : I;
        T CreateInjectedInstance<T>(bool isImplementationOnly);
        I CreateInjectedInstance<I, T>(bool isImplementationOnly) where T : I;
    }
}
