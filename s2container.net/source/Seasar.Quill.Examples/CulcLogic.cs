using System;
using System.Collections.Generic;
using System.Text;
using Seasar.Quill.Attrs;

namespace Seasar.Quill.Examples
{
    public class CulcLogic : ICulcLogic
    {
        protected HogeLogic hogeLogic = null;

        #region ICulcLogic ÉÅÉìÉo

        [Aspect(typeof(ConsoleWriteInterceptor))]
        public int Plus(int x, int y)
        {
            Console.WriteLine("PlusÇ™åƒÇŒÇÍÇ‹ÇµÇΩ");
            return LocalPlus(x, y);
        }

        #endregion

        [Aspect(typeof(ConsoleWriteInterceptor))]
        public virtual int LocalPlus(int x, int y)
        {
            return hogeLogic.HogeHoge(x, y);
        }
    }
}
