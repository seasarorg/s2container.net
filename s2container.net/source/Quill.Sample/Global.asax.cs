using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using log4net.Core;
using Quill.Container;
using Quill.Container.Impl;
using Quill.DataSource;
using Quill.DataSource.Impl;
using Quill.Inject;
using Quill.Inject.Impl;
using Quill.Message;
using Quill.Scope.Impl;
using QM = Quill.QuillManager;

namespace Quill.Sample {
    public class Global : HttpApplication {
        protected void Application_Start(object sender, EventArgs e) {
            QM.InitializeDefault();
            QM.ReplaceToParamMark = (pname => "@" + pname);
            QM.OutputLog = OutputLog;

            //特殊なインスタンス生成をcallbackでセット
            QM.ComponentCreator = CreateComponentCreator();

            // インジェクション対象とするフィルターを設定
            QM.InjectionFilter = CreateInjectionFilter();
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
            QM.Dispose();
        }

        private IComponentCreator CreateComponentCreator() {
            var creator = new ComponentCreators();
            creator.AddCreator(typeof(IDataSource), t => {
                return new DataSourceImpl(() => new SqlConnection(
                    "Server=localhost\\SQLEXPRESS;database=s2dotnetdemo;Integrated Security=SSPI"));
            });

            return creator;
        }

        private IInjectionFilter CreateInjectionFilter() {
            var injectionFilter = new InjectionFilterBase();
            injectionFilter.IsTargetTypeDefault = false;
            injectionFilter.InjectionTargetTypes.Add(typeof(ConnectionDecorator));
            injectionFilter.InjectionTargetTypes.Add(typeof(IDataSource));
            injectionFilter.InjectionTargetTypes.Add(typeof(IDbConnection));

            return injectionFilter;
        }

        private static void OutputLog(Type source, EnumMsgCategory category, string log) {
            ILog logger = LogManager.GetLogger(source);
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
    }
}