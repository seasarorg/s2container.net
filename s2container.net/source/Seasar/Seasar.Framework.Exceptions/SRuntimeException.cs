#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Exceptions
{
	/// <summary>
	/// Seasarの実行時例外のベースとなるクラスです。
	/// メッセージコードによって例外を詳細に特定できます。
	/// </summary>
	public class SRuntimeException : ApplicationException
	{
		private string messageCode_;
		private object[] args_;
		private string message_;
		private string simpleMessage_;

		public SRuntimeException(string messageCode)
			: this(messageCode,null,null)
		{
		}


		public SRuntimeException(string messageCode,object[] args)
			: this(messageCode,args,null)
		{
		}


		public SRuntimeException(string messageCode,object[] args,System.Exception cause)
			: base(messageCode,cause)
		{
			messageCode_ = messageCode;
			args_ = args;
			simpleMessage_ = MessageFormatter.GetSimpleMessage(messageCode_,args_);
			message_ = "[" + messageCode + "]" + simpleMessage_;
		}


		public string MessageCode
		{
			get { return messageCode_; }
		}


		public object[] Args
		{
			get { return args_; }
		}
		
		public override string Message
		{
			get { return message_; }
		}


		public string SimpleMessage
		{
			get { return simpleMessage_; }
		}

	}
}
