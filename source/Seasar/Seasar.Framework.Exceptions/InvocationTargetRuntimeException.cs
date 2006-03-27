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
using System.Reflection;
using System.Runtime.Serialization;

namespace Seasar.Framework.Exceptions
{
	/// <summary>
	/// TargetInvocationExceptionをラップする実行時例外です。
	/// </summary>
	[Serializable]
	public class InvocationTargetRuntimeException : SRuntimeException
	{
		private Type targetType_;

		public InvocationTargetRuntimeException(
			Type targetType,TargetInvocationException cause)
			: base("ESSR0043",new object[] { targetType.FullName,cause.GetBaseException() })

		{
			targetType_ = targetType;
		}

		public InvocationTargetRuntimeException(SerializationInfo info, StreamingContext context) 
			: base( info, context )
		{
			this.targetType_ = info.GetValue("targetType_", typeof(Type)) as Type;
		}

		public override void GetObjectData( SerializationInfo info,
			StreamingContext context )
		{
			info.AddValue("targetType_", this.targetType_, typeof(Type));

			base.GetObjectData(info, context);
		}

		public Type TargetType
		{
			get { return targetType_; }
		}
	}
}
