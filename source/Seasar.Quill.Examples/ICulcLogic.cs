using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Quill.Attrs;

namespace Seasar.Quill.Examples
{
    [Implementation(typeof(CulcLogic))]
    public interface ICulcLogic
    {
        int Plus(int x, int y);
    }
}
