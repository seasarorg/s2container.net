#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System.Reflection;
using System.Resources;
using Seasar.Quill.Exception;

namespace Seasar.Quill.Util
{
    /// <summary>
    /// Quillで作成されるメッセージを扱うクラス
    /// </summary>
    public class MessageUtil
    {
        /// <summary>
        /// Quillで使用するメッセージを格納しているResourceManager
        /// </summary>
        private static readonly ResourceManager MESSAGES_RESOURCE_MANAGER = 
            new ResourceManager("Seasar.Quill.QLLMessages", Assembly.GetExecutingAssembly());

        // objectの空の配列
        private static readonly object[] EMPTY_ARRAY = new object[0];

        /// <summary>
        /// メッセージコードを含まないメッセージを取得する
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        /// <param name="arguments">メッセージ中に埋め込む値の配列</param>
        /// <returns>メッセージコードを含まないメッセージ</returns>
        public static string GetSimpleMessage(string messageCode, object[] arguments)
        {
            // メッセージのフォーマットをResourceManagerから取得する
            string format = MESSAGES_RESOURCE_MANAGER.GetString(messageCode);

            if (format == null)
            {
                // メッセージが見つからない場合は例外をスローする
                throw new QuillApplicationException(
                    "EQLL0000", "message not found.");
            }

            if (arguments == null)
            {
                // メッセージ中に埋め込む値がnullの場合は空の配列に変換する
                arguments = EMPTY_ARRAY;
            }
            
            // フォーマットに値を埋め込みメッセージを作成する
            string message = string.Format(format, arguments);

            // メッセージを返す
            return message;
        }

        /// <summary>
        /// メッセージコードを含むメッセージを取得する
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        /// <param name="arguments">メッセージ中に埋め込む値の配列</param>
        /// <returns>メッセージコードを含まないメッセージ</returns>
        public static string GetMessage(string messageCode, object[] arguments)
        {
            // メッセージコード付きのメッセージを返す
            return "[" + messageCode + "]" + GetSimpleMessage(messageCode, arguments);
        }
    }
}
