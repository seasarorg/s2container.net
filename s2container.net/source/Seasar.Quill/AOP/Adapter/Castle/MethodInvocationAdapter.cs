using Castle.DynamicProxy;

namespace Seasar.Quill.AOP.Impl.Castle
{
    /// <summary>
    /// Castle.DynamicProxy用MethodInvocationアダプタクラス
    /// </summary>
    public class MethodInvocationAdapter : IMethodInvocation
    {
        public IInvocation Invocation { private get; set; }

        public System.Reflection.MethodBase Method
        {
            get { return Invocation.Method; }
        }

        public object Target
        {
            get { return Invocation.InvocationTarget; }
        }

        public object[] Arguments
        {
            get { return Invocation.Arguments; }
        }

        public object Proceed()
        {
            Invocation.Proceed();
            return Invocation.ReturnValue;
        }
    }
}
