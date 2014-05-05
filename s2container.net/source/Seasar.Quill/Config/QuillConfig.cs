using Seasar.Quill.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seasar.Quill.Config
{
    public class QuillConfig
    {
        #region static
        private static QuillConfig _config = new QuillConfig();

        public static QuillConfig GetInstance()
        {
            return _config;
        }
        #endregion

        public ILoggerFactory LoggerFactory { get; set; }
    }
}
