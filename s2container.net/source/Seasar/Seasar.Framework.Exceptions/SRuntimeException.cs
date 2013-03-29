#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Runtime.Serialization;
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
        private readonly string _messageCode;
        private readonly object[] _args;
        private readonly string _message;
        private readonly string _simpleMessage;

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
            _messageCode = messageCode;
            _args = args;
            _simpleMessage = MessageFormatter.GetSimpleMessage(_messageCode, _args);
            _message = "[" + messageCode + "]" + _simpleMessage;
        }

        public SRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _messageCode = info.GetString("_messageCode");
            _args = info.GetValue("_args", typeof(object[])) as object[];
            _message = info.GetString("_message");
            _simpleMessage = info.GetString("_simpleMessage");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_messageCode", _messageCode, typeof(string));
            info.AddValue("_args", _args, typeof(object[]));
            info.AddValue("_message", _message, typeof(string));
            info.AddValue("_simpleMessage", _simpleMessage, typeof(string));
            base.GetObjectData(info, context);
        }

        public string MessageCode
        {
            get { return _messageCode; }
        }

        public object[] Args
        {
            get { return _args; }
        }

        public override string Message
        {
            get { return _message; }
        }

        public string SimpleMessage
        {
            get { return _simpleMessage; }
        }
    }
}
