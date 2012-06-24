#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using System.Collections;
using System.Reflection;
using Seasar.Framework.Util;

namespace Seasar.Framework.Aop.Impl
{
    /// <summary>
    /// IS2MethodInvocationインターフェイスの実装
    /// </summary>
    public class MethodInvocationImpl : IS2MethodInvocation
    {
        /// <summary>
        /// 呼び出されるメソッドが属するインスタンス
        /// </summary>
        private readonly object _target;

        /// <summary>
        /// 呼び出されるメソッド
        /// </summary>
        private readonly MethodBase _method;

        /// <summary>
        /// メソッドをInterceptするInterceptorの配列
        /// </summary>
        private readonly IMethodInterceptor[] _interceptors;

        /// <summary>
        /// 処理されているInterceptorの再帰レベル
        /// </summary>
        private int _interceptorsIndex = 1;

        /// <summary>
        /// メソッドの引数
        /// </summary>
        private readonly object[] _arguments;

        /// <summary>
        /// メソッドとそのクラスのインスタンスが属するS2コンテナに関する情報
        /// </summary>
        private readonly Hashtable _parameters;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">Interceptされるオブジェクト</param>
        /// <param name="method">InterceptされるメソッドのMethodBase</param>
        /// <param name="arguments">引数</param>
        /// <param name="interceptors">メソッドのInterceptするInterceptor</param>
        /// <param name="parameters">Interceptされるメソッドとそのクラスのインスタンスが属するS2コンテナに関する情報</param>
        public MethodInvocationImpl(object target, MethodBase method,
            object[] arguments, IMethodInterceptor[] interceptors, Hashtable parameters)
        {
            if (target == null)
            {
                throw new NullReferenceException("target");
            }
            if (method == null)
            {
                throw new NullReferenceException("method");
            }
            if (interceptors == null)
            {
                throw new NullReferenceException("interceptors");
            }
            _target = target;
            _method = method;
            _arguments = arguments;
            _interceptors = interceptors;
            _parameters = parameters;
        }

        #region IMethodInvocation メンバ

        /// <summary>
        /// InterceptされるメソッドのMethod
        /// </summary>
        public MethodBase Method
        {
            get { return _method; }
        }

        /// <summary>
        /// Interceptされるオブジェクト
        /// </summary>
        public object Target
        {
            get { return _target; }
        }

        /// <summary>
        /// Interceptされるメソッドの引数
        /// </summary>
        public object[] Arguments
        {
            get { return _arguments; }
        }

        /// <summary>
        /// メソッドの呼び出し
        /// </summary>
        /// <remarks>
        /// 他にチェーンされているInterceptorがあれば、Interceptorを呼び出します（再帰的に呼び出される）。
        /// 他にチェーンされているInterceptorが無ければ、Interceptされているメソッドを実行します。
        /// <remarks>
        /// <returns>Interceptされたメソッドの戻り値</returns>
        public object Proceed()
        {
            while (_interceptorsIndex < _interceptors.Length)
            {
                // 他にInterceptorがあれば、Interceptorを呼び出す
                return _interceptors[_interceptorsIndex++].Invoke(this);
            }

            try
            {
                // Interceptされたメソッドを実行する
                return _method.Invoke(_target, _arguments);
            }
            catch (TargetInvocationException ex)
            {
                // InnerExceptionのStackTraceを保存する
                ExceptionUtil.SaveStackTraceToRemoteStackTraceString(ex.InnerException);

                // InnerExceptionをthrowする
                throw ex.InnerException;
            }
        }

        #endregion

        #region IS2MethodInvocation メンバ

        /// <summary>
        /// メソッドが属するクラスの型情報
        /// </summary>
        public Type TargetType
        {
            get { return _target.GetType(); }
        }

        /// <summary>
        /// メソッドとそのクラスのインスタンスが属するS2コンテナに関する情報
        /// </summary>
        public object GetParameter(string name)
        {
            return _parameters[name];
        }

        #endregion
    }
}
