using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public RETURN_TYPE Exec<RETURN_TYPE>(Func<RETURN_TYPE> f, object[] parameters)
        {
            try
            {
                BeginTryCatch();
                var returnValue = f();
                EndTryCatch();
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
        protected abstract void BeginTryCatch();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void EndTryCatch();

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
