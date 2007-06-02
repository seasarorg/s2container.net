using System;
using Seasar.Quill.Attrs;

namespace Seasar.Quill.Examples
{
    [Implementation]
    public interface IFugaLogic
    {
        [Aspect(typeof(ConsoleWriteInterceptor))]
        void FugaFuga();
    }
}
