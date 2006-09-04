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

namespace Seasar.Framework.Beans
{
	/// <summary>
	/// PropertyNotFoundRuntimeException ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[Serializable]
	public class PropertyNotFoundRuntimeException : SRuntimeException
	{
		private Type targetType_;
		private string propertyName_;

		public PropertyNotFoundRuntimeException(Type componentType,string propertyName)
			: base("ESSR0065",new object[] { componentType.FullName,propertyName})
		{
			targetType_ = componentType;
			propertyName_ = propertyName;
		}

		public PropertyNotFoundRuntimeException(SerializationInfo info, StreamingContext context) 
			: base( info, context )
		{
			this.targetType_ = info.GetValue("targetType_", typeof(Type)) as Type;
			this.propertyName_ = info.GetString("propertyName_");
		}

		public override void GetObjectData( SerializationInfo info,
			StreamingContext context )
		{
			info.AddValue("targetType_", this.targetType_, typeof(Type));
			info.AddValue("propertyName_", this.propertyName_, typeof(string));

			base.GetObjectData(info, context);
		}

		public Type TargetType
		{
			get { return targetType_; }
		}

		public string PropertyName
		{
			get { return propertyName_; }
		}
	}
}
