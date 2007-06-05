using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Quill.Attrs;

namespace Seasar.Quill.Examples
{
    [Implementation]
    public class HogeLogic
    {
        protected IFugaLogic fugaLogic = null;

        [Aspect(typeof(ConsoleWriteInterceptor))]
        public virtual int HogeHoge(int x, int y)
        {
            Console.WriteLine("HogeHoge");
            fugaLogic.FugaFuga();
            return x + y;

        }

    }
}
