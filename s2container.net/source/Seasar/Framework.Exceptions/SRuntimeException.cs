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
using Seasar.Framework.Container;
using Seasar.Framework.Message;

namespace Seasar.Framework.Exceptions
{
    /// <summary>
    /// Seasarの実行時例外のベースとなるクラスです。
    /// メッセージコードによって例外を詳細に特定できます。
    /// </summary>
    [Serializable]
    public class SRuntimeException : ApplicationException
    {
        private string messageCode;
        private object[] args;
        private string message;
        private string simpleMessage;

        public SRuntimeException(string messageCode)
            : this(messageCode, null, null)
        {
        }

        public SRuntimeException(string messageCode, object[] args)
            : this(messageCode, args, null)
        {
        }

        public SRuntimeException(string messageCode, object[] args, Exception cause)
            : base(messageCode, cause)
        {
            this.messageCode = messageCode;
            this.args = args;
            simpleMessage = MessageFormatter.GetSimpleMessage(messageCode, args);
            message = "[" + messageCode + "]" + simpleMessage;
        }

        public string MessageCode
        {
            get { return messageCode; }
        }

        public object[] Args
        {
            get { return args; }
        }

        public override string Message
        {
            get { return message; }
        }

        public string SimpleMessage
        {
            get { return simpleMessage; }
        }
    }
}
