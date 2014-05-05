using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seasar.Quill.Exception
{
    public class QuillApplicationException : System.Exception
    {
        public QuillApplicationException() : base()
        { }

        public QuillApplicationException(string message) : base(message)
        { }

        public QuillApplicationException(string message, System.Exception innerException) : base(message, innerException)
        { }
    }
}
