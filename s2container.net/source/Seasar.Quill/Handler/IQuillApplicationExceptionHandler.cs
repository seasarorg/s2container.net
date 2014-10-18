using Seasar.Quill.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Handler
{
    public delegate void HandleQuillApplicationException(QuillApplicationException ex);

    public interface IQuillApplicationExceptionHandler
    {
        void Handle(QuillApplicationException ex);
    }
}
