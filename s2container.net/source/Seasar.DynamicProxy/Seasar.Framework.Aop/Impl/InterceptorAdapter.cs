#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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

#if NET_4_0
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// Castle.DynamicProxyとSeasarのIMethodInterceptorの橋渡しを行う
    /// Adapterクラス
    /// </summary>
	public class InterceptorAdapter : IInterceptor
	{
        /// <summary>
        /// Seasar側のInterceptor
        /// </summary>
        private readonly IDictionary<MethodInfo, IMethodInterceptor[]> _interceptors;

        /// <summary>
        /// Aspectが適用される型
        /// </summary>
        private readonly Type _type;

        /// <summary>
        /// パラメータ
        /// </summary>
        private readonly Hashtable _parameters;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interceptors">Seasar側のInterceptor</param>
        /// <param name="type">Aspectが適用される型</param>
        /// <param name="parameters">パラメータ</param>
        public InterceptorAdapter(IDictionary<MethodInfo, IMethodInterceptor[]> interceptors,
            Type type, Hashtable parameters)
        {
            if (interceptors == null)
            {
                throw new ArgumentNullException("interceptors");
            }
            _interceptors = interceptors;
             _type = type;
            _parameters = parameters;
        }

        #region IInterceptor メンバー

        /// <summary>
        /// Castle.DynamicProxy側のIntercept処理
        /// </summary>
        /// <remarks>
        /// AOPが適用された場合、元のクラスのメソッドの代わりにこのメソッドが
        /// 呼び出される
        /// </remarks>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            if (IsInterceptTarget(invocation))
            {
                var interceptors = _interceptors[invocation.Method] as IMethodInterceptor[];
                var mehotdInvocation = new DynamicProxyMethodInvocation(invocation.Proxy, _type, 
                       invocation, invocation.Arguments, interceptors, _parameters);
                invocation.ReturnValue = interceptors[0].Invoke(mehotdInvocation);
            }
            else
            {
                // 元のメソッドの処理を呼び出す
                invocation.Proceed();
            }
        }

        #endregion

        /// <summary>
        /// Intercept対象か判定
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns>true:Intercept対象、false:Intercept対象外</returns>
        private bool IsInterceptTarget(IInvocation invocation)
        {
            if ((invocation.Proxy == invocation.Proxy ||
                !(invocation.Method.IsVirtual && !invocation.Method.IsFinal)) &&
                _interceptors.ContainsKey(invocation.Method))
            {
                return true;
            }

            return false;
        }
    }
}
#endif
