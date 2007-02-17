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

namespace Seasar.Framework.Aop.Impl
{
	/// <summary>
	/// IAspectインターフェイスの実装
	/// </summary>
	[Serializable]
	public class AspectImpl : IAspect
	{
		/// <summary>
		/// Interceptor
		/// </summary>
		private IMethodInterceptor methodInterceptor_;

		/// <summary>
		/// Pointcut
		/// </summary>
		/// <remarks>
		/// このフィールドがnullの場合はすべてのメソッドがInterceptされます。
		/// </remarks>
		private IPointcut pointcut_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="methodInterceptor">IMethodInterceptor</param>
		/// <param name="pointcut">
		/// IPointcut(nullでもよい。その場合すべてのメソッドがInterceptの対象となる)
		/// </param>
		public AspectImpl(IMethodInterceptor methodInterceptor,IPointcut pointcut)
		{
			methodInterceptor_ = methodInterceptor;
			pointcut_          = pointcut;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <remarks>
		/// すべてのメソッドにInterceptする場合はこのコンストラクタをつかいます。
		/// </remarks>
		/// <param name="methodInterceptor">IMethodInterceptor</param>
		public AspectImpl(IMethodInterceptor methodInterceptor)
			: this(methodInterceptor,null)
		{
		}

		#region IAspect メンバ

		/// <summary>
		/// Advice(Interceptor)
		/// </summary>
		public IMethodInterceptor MethodInterceptor
		{
			get
			{
				return methodInterceptor_;
			}
		}

		/// <summary>
		/// Pointcut
		/// </summary>
		public IPointcut Pointcut
		{
			get
			{
				return pointcut_;
			}
			set
			{
				pointcut_ = value;
			}
		}

		#endregion

	}	// AspectImpl 
}
