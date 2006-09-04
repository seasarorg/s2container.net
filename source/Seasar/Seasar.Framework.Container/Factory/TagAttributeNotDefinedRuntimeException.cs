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

namespace Seasar.Framework.Container.Factory
{
	[Serializable]
	public class TagAttributeNotDefinedRuntimeException : SRuntimeException
	{
		private string tagName_;
		private string attributeName_;

		public TagAttributeNotDefinedRuntimeException(string tagName,string attributeName)
			: base("ESSR0056",new object[] {tagName,attributeName})
		{
			tagName_ = tagName;
			attributeName_ = attributeName;
		}

		public TagAttributeNotDefinedRuntimeException(SerializationInfo info, StreamingContext context) 
			: base( info, context )
		{
			this.tagName_ = info.GetString("tagName_");
			this.attributeName_ = info.GetString("attributeName_");
		}

		public override void GetObjectData( SerializationInfo info,
			StreamingContext context )
		{
			info.AddValue("tagName_", this.tagName_, typeof(String));
			info.AddValue("attributeName_", this.attributeName_, typeof(String));

			base.GetObjectData(info, context);
		}

		public string TagName
		{
			get { return tagName_; }
		}

		public string AttributeName
		{
			get { return attributeName_; }
		}
	}
}
