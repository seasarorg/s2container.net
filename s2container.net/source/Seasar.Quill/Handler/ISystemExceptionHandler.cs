using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Handler
{
    public delegate void HandleSystemException(System.Exception ex);

    public interface ISystemExceptionHandler
    {
        void Handle(System.Exception ex);
    }
}
