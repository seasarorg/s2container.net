using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quill.Sample.App_Code {
    public static class SampleInitializer {
        public static void Initialize() {
            QuillManager.InitializeDefault();
            // SQL Server用のパラメータ名
            QuillManager.ReplaceToParamMark = (pname => "@" + pname);
        }
    }
}
