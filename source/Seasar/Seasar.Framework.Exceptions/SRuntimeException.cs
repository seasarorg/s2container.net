#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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

		public SRuntimeException(SerializationInfo info, StreamingContext context ) 
			: base( info, context )
		{
			this.messageCode_ = info.GetString("messageCode_");
			this.args_ = info.GetValue("args_", typeof(object[])) as object[];
			this.message_ = info.GetString("message_");
			this.simpleMessage_ = info.GetString("simpleMessage_");
		}

		public override void GetObjectData( SerializationInfo info,
			StreamingContext context )
		{
			info.AddValue("messageCode_", this.messageCode_, typeof(String));
			info.AddValue("args_", this.args_, typeof(object[]));
			info.AddValue("message_", this.message_, typeof(String));
			info.AddValue("simpleMessage_", this.simpleMessage_, typeof(String));

			base.GetObjectData(info, context);
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
