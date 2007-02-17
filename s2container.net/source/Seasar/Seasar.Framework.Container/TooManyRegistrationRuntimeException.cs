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
using System.Text;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Container
{
	/// <summary>
	/// TooManyRegistrationRuntimeException の概要の説明です。
	/// </summary>
	[Serializable]
	public sealed class TooManyRegistrationRuntimeException : SRuntimeException
	{
		private object key_;
		private Type[] componentTypes_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="key"></param>
		/// <param name="componentTypes"></param>
		public TooManyRegistrationRuntimeException(object key,Type[] componentTypes)
			: base("ESSR0045", new object[] {key,GetTypeNames(componentTypes)})
		{
			key_ = key;
			componentTypes_ = componentTypes;
		}

		public TooManyRegistrationRuntimeException(SerializationInfo info, StreamingContext context) 
			: base( info, context )
		{
			this.key_ = info.GetValue("key_", typeof(object));
			this.componentTypes_ = info.GetValue("componentTypes_", typeof(Type[])) as Type[];
		}

		public override void GetObjectData( SerializationInfo info,
			StreamingContext context )
		{
			info.AddValue("key_", this.key_, typeof(object));
			info.AddValue("componentTypes_", this.componentTypes_, typeof(Type[]));

			base.GetObjectData(info, context);
		}

		public object Key
		{
			get { return key_; }
		}


		public Type[] ComponentTypes
		{
			get { return componentTypes_; }
		}


		public static string GetTypeNames(Type[] componentTypes)
		{
			StringBuilder buf = new StringBuilder(255);
			foreach(Type componentType in componentTypes)
			{
				buf.Append(componentType.FullName);
				buf.Append(", ");
			}
			buf.Length -= 2;
			return buf.ToString();
		}

	}
}
