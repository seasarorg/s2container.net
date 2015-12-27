namespace Quill.Message {
    /// <summary>
    /// メッセージカテゴリ列挙体
    /// </summary>
    public enum EnumMsgCategory {
        /// <summary>デバッグ</summary>
        DEBUG = 0,

        /// <summary>運用参考情報</summary>
        INFO,

        /// <summary>警告</summary>
        WARN,

        /// <summary>エラー</summary>
        ERROR
    };

    /// <summary>
    /// メッセージカテゴリ列挙体ユーティリティクラス
    /// </summary>
    public static class EnumMsgCategoryUtils {
        /// <summary>
        /// カテゴリ名を取得
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static string GetCategoryName(this EnumMsgCategory category) {
            switch(category) {
                case EnumMsgCategory.INFO:
                    return "INFO";
                case EnumMsgCategory.WARN:
                    return "INFO";
                case EnumMsgCategory.ERROR:
                    return "ERROR";
                default:
                    return "DEBUG";
            }
        }
    }
}
