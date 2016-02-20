using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using log4net.Core;
using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.Sample {
    public class Global : System.Web.HttpApplication {
        private readonly ILog _log = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e) {
            QM.InitializeDefault();
            QM.ReplaceToParamMark = (pname => "@" + pname);
            QM.OutputLog = OutputLog;
        }

        private static void OutputLog(string source, EnumMsgCategory category, string log) {
            ILog logger = LogManager.GetLogger(Type.GetType(source));
            switch(category) {
                case EnumMsgCategory.ERROR:
                    logger.Error(log);
                    break;
                case EnumMsgCategory.WARN:
                    logger.Warn(log);
                    break;
                case EnumMsgCategory.INFO:
                    logger.Info(log);
                    break;
                default:
                    logger.Debug(log);
                    break;
            }
        }

        protected void Session_Start(object sender, EventArgs e) {

        }

        protected void Application_BeginRequest(object sender, EventArgs e) {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {

        }

        protected void Application_Error(object sender, EventArgs e) {

        }

        protected void Session_End(object sender, EventArgs e) {

        }

        protected void Application_End(object sender, EventArgs e) {

        }
    }
}