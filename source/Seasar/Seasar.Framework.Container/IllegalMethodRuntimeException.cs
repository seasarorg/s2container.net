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
using System.Runtime.Serialization;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Container
{
	/// <summary>
	/// IllegalMethodRuntimeException ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[Serializable]
	public class IllegalMethodRuntimeException : SRuntimeException
	{
		private Type componentType_;
		private string methodName_;

		public IllegalMethodRuntimeException(
			Type componentType,string methodName,Exception cause)
            : base("ESSR0060", new object[] { componentType.FullName, methodName, cause }, cause)
		{
			componentType_ = componentType;
			methodName_ = methodName;
		}

		public IllegalMethodRuntimeException(SerializationInfo info, StreamingContext context) 
			: base( info, context )
		{
			this.componentType_ = info.GetValue("componentType_", typeof(Type)) as Type;
			this.methodName_ = info.GetString("methodName_");
		}

		public override void GetObjectData( SerializationInfo info,
			StreamingContext context )
		{
			info.AddValue("componentType_", this.componentType_, typeof(Type));
			info.AddValue("methodName_", this.methodName_, typeof(String));

			base.GetObjectData(info, context);
		}

		public Type ComponentType
		{
			get { return componentType_; }
		}

		public string MethodName
		{
			get { return methodName_; }
		}
	}
}
