using System;
using System.Collections.Generic;

namespace Seasar.Quill.Decorator.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class ScopeDecorators : IScopeDecorator
    {
        private readonly IList<IScopeDecorator> _decorators = new List<IScopeDecorator>();

        public virtual void Add(IScopeDecorator decorator)
        {
            _decorators.Add(decorator);
        }

        public virtual void Clear()
        {
            _decorators.Clear();
        }

        public RETURN_TYPE Exec<RETURN_TYPE>(Func<RETURN_TYPE> f, object[] parameters)
        {
            return DoDecorate(f,  _decorators.Count - 1, parameters);
        }

        protected virtual RETURN_TYPE DoDecorate<RETURN_TYPE>(Func<RETURN_TYPE> f, int nextIndex, object[] parameters)
        {
            if (nextIndex > 0)
            {
                return _decorators[nextIndex].Exec(() => DoDecorate(f, nextIndex - 1, parameters), parameters);
            }
            else if(nextIndex == 0)
            {
                return _decorators[nextIndex].Exec(f, parameters);
            }
            return f();
        }
    }
}
