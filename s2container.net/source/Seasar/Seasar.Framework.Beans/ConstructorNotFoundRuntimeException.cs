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
using System.Text;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Beans
{
	/// <summary>
	/// 対象のクラスに適用可能なコンストラクタが見つからなかった場合の実行時例外です。
	/// </summary>
	public class ConstructorNotFoundRuntimeException : SRuntimeException
	{
		private Type targetType_;
		private object[] methodArgs_;

		public ConstructorNotFoundRuntimeException(Type targetType,
			object[] methodArgs) : base("ESSR0048",
			new object[] { targetType.FullName, GetSignature(methodArgs) })
		{
			targetType_ = targetType;
			methodArgs_ = methodArgs;
		}

		public Type TargetType
		{
			get { return targetType_; }
		}

		public object[] MethodArgs
		{
			get { return methodArgs_; }
		}

		private static string GetSignature(object[] methodArgs)
		{
			StringBuilder buf = new StringBuilder(100);
			if(methodArgs != null)
			{
				for(int i = 0; i < methodArgs.Length; ++i)
				{
					if (i > 0) buf.Append(", ");
					if(methodArgs[i] != null)
					{
						buf.Append(methodArgs[i].GetType().FullName);
					}
					else
					{
						buf.Append("null");
					}
				}
			}
			return buf.ToString();
		}
	}
}
