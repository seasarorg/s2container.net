using System;

namespace Seasar.Quill.Decorator
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScopeDecorator
    {
       
        RETURN_TYPE Exec<RETURN_TYPE>(Func<RETURN_TYPE> f, object[] parameters);
    }
}
