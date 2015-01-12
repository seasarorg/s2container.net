using System;
using System.Reflection;

namespace Seasar.Quill.Decorator.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TryCacthDecorator : IScopeDecorator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <param name="f"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RETURN_TYPE Execute<RETURN_TYPE>(Func<RETURN_TYPE> f, object[] parameters)
        {
            try
            {
                BeginTryCatch(f.Target, f.Method, parameters);
                var returnValue = f();
                EndTryCatch(f.Target, f.Method, parameters);
                return returnValue;
            }
            catch(System.Exception)
            {
                HandleException();
                return default(RETURN_TYPE);
            }
            finally
            {
                HandleFinally();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        protected abstract void BeginTryCatch(object target, MethodInfo method, object[] parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        protected abstract void EndTryCatch(object target, MethodInfo method, object[] parameters);

        /// <summary>
        /// 
        /// </summary>
        protected abstract void HandleException();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void HandleFinally();
    }
}
