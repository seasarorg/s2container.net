#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

using System;

namespace Seasar.Quill.Exception
{
    public class QuillComponentInvalidCastException : QuillApplicationException
    {
        /// <summary>
        /// コンポーネントの型を指定してQuillComponentInvalidCastExceptionを初期化する
        /// </summary>
        /// <param name="componentType"></param>
        public QuillComponentInvalidCastException(Type componentType) : base
            (string.Format("ComponentType = {0}", componentType.Name))
        {
        }

        /// <summary>
        /// メッセージコードとメッセージを指定してQuillComponentInvalidCastExceptionを初期化する
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        /// <param name="message">メッセージ</param>
        public QuillComponentInvalidCastException(string messageCode, string message) : base(messageCode, message)
        {
        }

        /// <summary>
        /// メッセージコード・メッセージ中に埋め込む文字列の配列を指定して
        /// QuillComponentInvalidCastExceptionを初期化する
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        /// <param name="args">メッセージ中に埋め込む文字列の配列</param>
        public QuillComponentInvalidCastException(string messageCode, object[] args)
            : this(messageCode, args, null)
        {
        }

        /// <summary>
        /// メッセージコード・メッセージ中に埋め込む文字列の配列・元となった例外
        /// を指定してQuillComponentInvalidCastExceptionを初期化する
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        /// <param name="args">メッセージ中に埋め込む値の配列</param>
        /// <param name="cause">元となった例外</param>
        public QuillComponentInvalidCastException(
            string messageCode, object[] args, System.Exception cause)
            : base(messageCode, args, cause)
        {
        }
    }
}
