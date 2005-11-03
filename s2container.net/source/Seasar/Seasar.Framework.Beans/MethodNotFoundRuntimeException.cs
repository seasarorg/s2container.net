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
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Framework.Beans
{
	/// <summary>
	/// MethodNotFoundRuntimeException ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MethodNotFoundRuntimeException : SRuntimeException
	{
		private Type targetType_;
		private string methodName_;
		private Type[] methodArgTypes_;

		public MethodNotFoundRuntimeException(
			Type targetType,string methodName,object[] methodArgs)
			: base("ESSR0049",new object[] {
											   targetType.FullName,
											   MethodUtil.GetSignature(methodName,methodArgs) } )
		{
			targetType_ = targetType;
			methodName_ = methodName;
			if(methodArgs != null) methodArgTypes_ = Type.GetTypeArray(methodArgs);
		}

		public MethodNotFoundRuntimeException(
			Type targetType,string methodName,Type[] methodArgTypes)
			: base("ESSR0049",new object[] {
											   targetType.FullName,
											   MethodUtil.GetSignature(methodName,methodArgTypes)})
		{
			targetType_ = targetType;
			methodName_ = methodName;
			methodArgTypes_ = methodArgTypes;
		}

		public Type TargetType
		{
			get { return targetType_; }
		}

		public string MethodName
		{
			get { return methodName_; }
		}

		public Type[] MethodArgTypes
		{
			get { return methodArgTypes_; }
		}
	}
}
