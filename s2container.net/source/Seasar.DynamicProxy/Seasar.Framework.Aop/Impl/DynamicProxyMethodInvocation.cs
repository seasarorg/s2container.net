#region Copyright
/*
* Copyright 2005-2008 the Seasar Foundation and the Others.
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

#region using directives

using System;
using System.Collections;
using System.Reflection;
using Castle.DynamicProxy;

#endregion

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// 複数のAdvice(Interceptor)によるチェーンを抽象化したインタフェースの実装クラスです
    /// </summary>
    /// <author>Kazz</author>
    /// <version>1.3 2006/05/23</version>
    ///
    public class DynamicProxyMethodInvocation : IS2MethodInvocation
    {
        #region fields

        private readonly object _target;
        private readonly Type _targetType;
        private readonly IInvocation _invocation;
        private readonly IMethodInterceptor[] _interceptors;
        private int _interceptorsIndex = 1;
        private readonly object[] _arguments;
        private readonly Hashtable _parameters;

        #endregion

        #region constructors

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">対象のオブジェクトをセット</param>
        /// <param name="targetType">対象の型をセット</param>
        /// <param name="invocation">IInvocationインタフェースをセット</param>
        /// <param name="arguments">引数</param>
        /// <param name="interceptors">インターセプタの配列をセット</param>
        /// <param name="parameters">パラメータ</param>
        public DynamicProxyMethodInvocation(object target
                                            , Type targetType
                                            , IInvocation invocation
                                            , object[] arguments
                                            , IMethodInterceptor[] interceptors
                                            , Hashtable parameters)
        {
            if (target == null) throw new NullReferenceException("target");
            if (targetType == null) throw new NullReferenceException("targetType");
            if (invocation == null) throw new NullReferenceException("invocation");
            if (interceptors == null) throw new NullReferenceException("interceptors");
            _target = target;
            _targetType = targetType;
            _invocation = invocation;
            _arguments = arguments;
            _interceptors = interceptors;
            _parameters = parameters;
        }

        #endregion

        #region IMethodInvocation member

        public MethodBase Method
        {
            get { return _invocation.Method; }
        }

        public object Target
        {
            get { return _target; }
        }

        public object[] Arguments
        {
            get { return _arguments; }
        }

        public object Proceed()
        {
            while (_interceptorsIndex < _interceptors.Length)
            {
                return _interceptors[_interceptorsIndex++].Invoke(this);
            }
            return _invocation.Proceed(_arguments);
        }

        #endregion

        #region IS2MethodInvocation メンバ

        public Type TargetType
        {
            get { return _targetType; }
        }

        public object GetParameter(string name)
        {
            return _parameters[name];
        }

        #endregion
    }
}
