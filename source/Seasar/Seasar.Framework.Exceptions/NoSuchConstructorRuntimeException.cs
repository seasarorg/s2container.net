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
using System.Runtime.Serialization;
using Seasar.Framework.Util;

namespace Seasar.Framework.Exceptions
{
	/// <summary>
	/// コンストラクタが見つからない場合の実行時例外です。
	/// </summary>
	[Serializable]
	public class NoSuchConstructorRuntimeException : SRuntimeException
	{
		private Type targetType_;
		private Type[] argTypes_;

		public NoSuchConstructorRuntimeException(
			Type targetType,Type[] argTypes)
			: base("ESSR0064",new object[] { targetType.FullName,
											   MethodUtil.GetSignature(targetType.Name,argTypes)})
		{
			targetType_ = targetType;
			argTypes_ = argTypes;
		}

		public NoSuchConstructorRuntimeException(SerializationInfo info, StreamingContext context) 
			: base( info, context )
		{
			this.targetType_ = info.GetValue("targetType_", typeof(Type)) as Type;
			this.argTypes_ = info.GetValue("argTypes_", typeof(Type[])) as Type[];
		}

		public override void GetObjectData( SerializationInfo info,
			StreamingContext context )
		{
			info.AddValue("targetType_", this.targetType_, typeof(Type));
			info.AddValue("argTypes_", this.argTypes_, typeof(Type[]));

			base.GetObjectData(info, context);
		}

		public Type TargetType
		{
			get { return targetType_; }
		}

		public Type[] ArgTypes
		{
			get { return argTypes_; }
		}
	}
}
