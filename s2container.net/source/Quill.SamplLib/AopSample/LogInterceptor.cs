using Castle.DynamicProxy;
using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.SampleLib.AopSample {
    /// <summary>
    /// ログ出力インターセプター
    /// </summary>
    public class LogInterceptor : IInterceptor {
        public void Intercept(IInvocation invocation) {
            QM.OutputLog(GetType(), EnumMsgCategory.DEBUG, GetBasicTargetInfo(invocation, "Start"));
            try {
                invocation.Proceed();
            } catch(System.Exception ex) {
                QM.OutputLog(GetType(), EnumMsgCategory.DEBUG, GetBasicTargetInfo(invocation,
                    string.Format("{0}, StackTrace:{1}", ex.Message, ex.StackTrace)));
            } finally {
                QM.OutputLog(GetType(), EnumMsgCategory.DEBUG, GetBasicTargetInfo(invocation, "End"));
            }
        }

        private string GetBasicTargetInfo(IInvocation invocation, string additionalMsg) {
            return string.Format("{0}.{1} : {2}",
                invocation.Method.DeclaringType.Name, invocation.Method.Name, additionalMsg);
        }
    }
}
