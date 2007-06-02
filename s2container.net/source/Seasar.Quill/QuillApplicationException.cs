#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using Seasar.Framework.Message;
using System.Reflection;
using Seasar.Quill.Util;

namespace Seasar.Quill
{
    /// <summary>
    /// Quillでアプリケーションエラーが発生した場合にスローされる例外クラス
    /// </summary>
    [Serializable]
    public class QuillApplicationException : ApplicationException
    {
        /// <summary>
        /// メッセージコード
        /// </summary>
        protected string messageCode;

        /// <summary>
        /// メッセージに埋め込む値の配列
        /// </summary>
        protected object[] args;

        /// <summary>
        /// メッセージ(メッセージコードを含む)
        /// </summary>
        protected string message;

        /// <summary>
        /// 簡単なメッセージ(メッセージコードを含まない)
        /// </summary>
        protected string simpleMessage;

        /// <summary>
        /// メッセージコードを指定してQuillApplicationExceptionを初期化する
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        public QuillApplicationException(string messageCode)
			: this(messageCode,null,null)
		{
		}

        /// <summary>
        /// メッセージコード・メッセージ中に埋め込む文字列の配列を指定して
        /// QuillApplicationExceptionを初期化する
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        /// <param name="args">メッセージ中に埋め込む文字列の配列</param>
		public QuillApplicationException(string messageCode,object[] args)
			: this(messageCode,args,null)
		{
		}

        /// <summary>
        /// メッセージコード・メッセージ中に埋め込む文字列の配列・元となった例外
        /// を指定してQuillApplicationExceptionを初期化する
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        /// <param name="args">メッセージ中に埋め込む値の配列</param>
        /// <param name="cause">元となった例外</param>
        public QuillApplicationException(
            string messageCode, object[] args, Exception cause) : base(null, cause)
		{
            // メッセージコードをセットする
            this.messageCode = messageCode;

            // メッセージ中に埋め込む値の配列をセットする
            this.args = args;
            
            // メッセージをセットする
            simpleMessage = MessageUtil.GetSimpleMessage(messageCode, args);

            // メッセージコード付きのメッセージをセットする
            message = "[" + messageCode + "]" + simpleMessage;
        }

        /// <summary>
        /// メッセージコードを取得する
        /// </summary>
        /// <value>メッセージコード</value>
        public string MessageCode
        {
            get { return messageCode; }
        }

        /// <summary>
        /// メッセージに埋め込む値の配列を取得する
        /// </summary>
        /// <value>メッセージに埋め込む値の配列</value>
        public object[] Args
        {
            get { return args; }
        }

        /// <summary>
        /// メッセージ(メッセージコードを含む)を取得する
        /// </summary>
        /// <value>メッセージ(メッセージコードを含む)</value>
        public override string Message
        {
            get { return message; }
        }

        /// <summary>
        /// 簡単なメッセージ(メッセージコードを含まない)を取得する
        /// </summary>
        /// <value>簡単なメッセージ(メッセージコードを含まない)</value>
        public string SimpleMessage
        {
            get { return simpleMessage; }
        }
    }
}
