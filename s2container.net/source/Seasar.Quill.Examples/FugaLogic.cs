using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Quill.Attrs;

namespace Seasar.Quill.Examples
{
    public class FugaLogic : IFugaLogic
    {
        #region IFugaLogic ÉÅÉìÉo

        [Aspect(typeof(ConsoleWriteInterceptor))]
        [Aspect("hogeS2Interceptor")]
        public void FugaFuga()
        {
        }

        #endregion
    }
}
