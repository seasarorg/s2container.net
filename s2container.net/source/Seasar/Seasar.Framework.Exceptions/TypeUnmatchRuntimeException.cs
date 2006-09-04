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
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Container
{
	/// <summary>
	/// TypeUnmatchRuntimeException ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[Serializable]
	public class TypeUnmatchRuntimeException : SRuntimeException
	{
		private Type componentType_;
		private Type realComponentType_;

		public TypeUnmatchRuntimeException(
			Type componentType,Type realComponentType)
			: base("ESSR0069",new object[] 
			{
				componentType.FullName,
				realComponentType.FullName != null ? 
				realComponentType.FullName : "null"
			})
		{
			componentType_ = componentType;
			realComponentType_ = realComponentType;
		}

		public TypeUnmatchRuntimeException(SerializationInfo info, StreamingContext context) 
			: base( info, context )
		{
			this.componentType_ = info.GetValue("componentType_", typeof(Type)) as Type;
			this.realComponentType_ = info.GetValue("realComponentType_", typeof(Type)) as Type;
		}

		public override void GetObjectData( SerializationInfo info,
			StreamingContext context )
		{
			info.AddValue("componentType_", this.componentType_, typeof(Type));
			info.AddValue("realComponentType_", this.realComponentType_, typeof(Type));

			base.GetObjectData(info, context);
		}

		public Type ComponentType
		{
			get { return componentType_; }
		}

		public Type RealComponentType
		{
			get { return realComponentType_; }
		}
	}
}
