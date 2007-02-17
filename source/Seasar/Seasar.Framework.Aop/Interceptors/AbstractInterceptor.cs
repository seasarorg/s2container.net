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
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Container;

namespace Seasar.Framework.Aop.Interceptors
{
	/// <summary>
	/// Interceptorを作成する場合は、このクラスを継承します
	/// </summary>
	[Serializable]
	public abstract class AbstractInterceptor : IMethodInterceptor
	{
		const long serialVersionUID = 0L;

		public object CreateProxy(Type proxyType)
		{
			IAspect aspect = new AspectImpl(this, new PointcutImpl(
				new string[] { ".*" } ));
			return new AopProxy(proxyType,
				new IAspect[] { aspect }).GetTransparentProxy();
		}

		protected IComponentDef GetComponentDef(IMethodInvocation invocation)
		{
			if(invocation is IS2MethodInvocation)
			{
				IS2MethodInvocation impl = (IS2MethodInvocation) invocation;
				return (IComponentDef) impl.GetParameter(ContainerConstants.COMPONENT_DEF_NAME);
			}
			return null;
		}

        #region IMethodInterceptor インターフェイス

		public virtual object Invoke(IMethodInvocation invocation)
		{
			return invocation.Proceed();
		}

        #endregion

	}
}
