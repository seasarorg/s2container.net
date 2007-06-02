using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Quill.Attrs;

namespace Seasar.Quill.Examples
{
    [Implementation]
    [Aspect(typeof(ConsoleWriteInterceptor))]
    public class HogeLogic
    {
        protected IFugaLogic fugaLogic = null;

        public virtual int HogeHoge(int x, int y)
        {
            Console.WriteLine("HogeHoge");
            fugaLogic.FugaFuga();
            return x + y;

        }

    }
}
