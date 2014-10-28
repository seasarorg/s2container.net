using Seasar.Quill.Exception;
using System;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// ロジック選択ユーティリティクラス
    /// </summary>
    public static class LogicUtils
    {
        /// <summary>
        /// 適切なロジックが見つからなかったときのメッセージ
        /// </summary>
        private const string MSG_LOGIC_NOT_FOUND = "Suitable logic is not found.";

        /// <summary>
        /// ロジックの取得
        /// </summary>
        /// <typeparam name="USING_LOGIC">コールバックの型</typeparam>
        /// <typeparam name="DEFAULT_INTERFACE">既定処理を行うインターフェース</typeparam>
        /// <param name="callback">コールバック</param>
        /// <param name="defaultInterce">既定処理インターフェース</param>
        /// <param name="defaultInvoke">既定処理</param>
        /// <param name="notFoundMessage">ロジックが見つからなかった場合のメッセージ</param>
        /// <returns>決定したロジック</returns>
        public static USING_LOGIC GetLogic<USING_LOGIC, DEFAULT_INTERFACE>(
            USING_LOGIC callback,
            DEFAULT_INTERFACE defaultInterce,
            Func<DEFAULT_INTERFACE, USING_LOGIC> defaultInvoke,
            string notFoundMessage = null)
            where USING_LOGIC : class
        {
            // コールバックが指定されている場合はそちらを優先
            if (callback != null)
            {
                return callback;
            }

            // コールバック指定なしの場合は既定のインターフェースから処理を呼びだし
            if (defaultInterce != null)
            {
                return defaultInvoke(defaultInterce);
            }

            // ロジックが見つからなければエラーとする
            throw new QuillApplicationException(notFoundMessage == null ? MSG_LOGIC_NOT_FOUND : notFoundMessage);
        }
    }
}
