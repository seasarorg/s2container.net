using System.Data;
using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.Util {
    /// <summary>
    /// コネクションユーティリティクラス
    /// </summary>
    public static class ConnectionUtils {
        /// <summary>
        /// 接続開始
        /// </summary>
        /// <param name="connection">コネクション</param>
        public static void OpenConnection(this IDbConnection connection) {
            if(connection != null && connection.State != ConnectionState.Open) {
                connection.Open();
                QM.OutputLog("ConnectionUtils#OpenConnection",
                    EnumMsgCategory.DEBUG, QM.Message.GetConnectionOpened());
            }
        }

        /// <summary>
        /// 接続終了
        /// </summary>
        /// <param name="connection">コネクション</param>
        public static void CloseConnection(this IDbConnection connection) {
            if(connection != null && connection.State == ConnectionState.Open) {
                connection.Close();
                QM.OutputLog("ConnectionUtils#CloseConnection",
                    EnumMsgCategory.DEBUG, QM.Message.GetConnectionClosed());
            }
        }
    }
}
