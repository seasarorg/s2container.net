using System;
using Seasar.Quill.Attrs;

namespace Seasar.Quill.Examples
{
    [Implementation(typeof(FugaLogic))]
    public interface IFugaLogic
    {
        void FugaFuga();
    }
}
