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

using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Seasar.Framework.Aop.Impl;
using Seasar.Framework.Util;

namespace Seasar.Framework.Aop.Proxy
{
    /// <summary>
    /// AopProxy
    /// </summary>
    /// <remarks>
    /// 透過的プロクシによってAOPを実現しています。
    /// </remarks>
    [Serializable]
    public sealed class AopProxy : RealProxy
    {
        /// <summary>
        /// 透過的プロクシを作成するインスタンス
        /// </summary>
        private object _target;

        /// <summary>
        /// 適用するAspect
        /// </summary>
        private readonly IAspect[] _aspects;

        /// <summary>
        /// メソッドとそのクラスのインスタンスが属するS2コンテナに関する情報
        /// </summary>
        private readonly Hashtable _parameters;

        public Type TargetType { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectされるターゲットの型のType</param>
        /// <param name="aspects">適応するAspect</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="target">Aspectされるターゲット</param>
        public AopProxy(Type type, IAspect[] aspects, Hashtable parameters, object target)
            : base(type)
        {
            TargetType = type;
            _target = target;
            _aspects = aspects;
            _parameters = parameters;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectされるターゲットの型のType</param>
        /// <param name="aspects">適応するAspect</param>
        /// <param name="parameters">パラメータ</param>
        public AopProxy(Type type, IAspect[] aspects, Hashtable parameters)
            : this(type, aspects, parameters, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectされるターゲットの型のType</param>
        /// <param name="aspects">適応するAspect</param>
        public AopProxy(Type type, IAspect[] aspects)
            : this(type, aspects, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">Aspectされるターゲットの型のType</param>
        public AopProxy(Type type)
            : this(type, null)
        {
        }

        /// <summary>
        /// 透過的プロクシを返す
        /// </summary>
        /// <returns>透過的プロクシのインスタンス</returns>
        public object Create()
        {
            return GetTransparentProxy();
        }

        /// <summary>
        /// 透過的プロクシを返す
        /// </summary>
        /// <param name="argTypes">透過的プロクシの対象となるクラスのコンストラクタの引数の型のリスト</param>
        /// <param name="args">透過的プロクシの対象となるクラスのコンストラクタの引数のリスト</param>
        /// <returns>透過的プロクシのインスタンス</returns>
        public object Create(Type[] argTypes, object[] args)
        {
            var constructor = ClassUtil.GetConstructorInfo(TargetType, argTypes);
            _target = ConstructorUtil.NewInstance(constructor, args);
            return GetTransparentProxy();
        }

        /// <summary>
        /// 透過的プロクシを返す
        /// </summary>
        /// <param name="argTypes">透過的プロクシの対象となるクラスのコンストラクタの引数の型のリスト</param>
        /// <param name="args">透過的プロクシの対象となるクラスのコンストラクタの引数のリスト</param>
        /// <param name="targetType">透過的プロクシの対象となるクラスの型</param>
        /// <returns>透過的プロクシのインスタンス</returns>
        public object Create(Type[] argTypes, object[] args, Type targetType)
        {
            var constructor = ClassUtil.GetConstructorInfo(targetType, argTypes);
            _target = ConstructorUtil.NewInstance(constructor, args);
            return GetTransparentProxy();
        }

        #region RealProxy メンバ

        /// <summary>
        /// AopProxyを通したオブジェクトのメソッドが実行されるとこのメソッドが呼ばれます
        /// </summary>
        /// <param name="msg">IMessage</param>
        /// <returns>IMessage</returns>
        /// <seealso cref="System.Runtime.Remoting.Proxies">System.Runtime.Remoting.Proxies</seealso>
        public override IMessage Invoke(IMessage msg)
        {
            if (_target == null)
            {
//                if (!_type.IsInterface) _target = Activator.CreateInstance(_type);
                if (!TargetType.IsInterface) _target = ClassUtil.NewInstance(TargetType);
                if (_target == null) _target = new object();
            }

            var methodMessage = msg as IMethodMessage;
            if (methodMessage != null)
            {
                var method = methodMessage.MethodBase;

                var interceptorList = new ArrayList();

                if (_aspects != null)
                {
                    // 定義されたAspectからInterceptorのリストの作成
                    foreach (var aspect in _aspects)
                    {
                        var pointcut = aspect.Pointcut;
                        // IPointcutよりAdvice(Interceptor)を挿入するか確認
                        if (pointcut == null || pointcut.IsApplied(method))
                        {
                            // Aspectを適用する場合
                            interceptorList.Add(aspect.MethodInterceptor);
                        }
                    }
                }

                object ret;

                object[] methodArgs;

                if (interceptorList.Count == 0)
                {
                    methodArgs = methodMessage.Args;

                    try
                    {
                        //Interceptorを挿入しない場合
                        ret = MethodUtil.Invoke((MethodInfo) method, _target, methodArgs);
//                    ret = method.Invoke(_target, methodArgs);
                    }
                    catch (TargetInvocationException ex)
                    {
                        // InnerExceptionのStackTraceを保存する
                        ExceptionUtil.SaveStackTraceToRemoteStackTraceString(ex.InnerException);

                        // InnerExceptionをthrowする
                        throw ex.InnerException;
                    }
                }
                else
                {
                    // Interceptorを挿入する場合
                    var interceptors = (IMethodInterceptor[])
                        interceptorList.ToArray(typeof (IMethodInterceptor));

                    IMethodInvocation invocation = new MethodInvocationImpl(_target,
                        method, methodMessage.Args, interceptors, _parameters);

                    ret = interceptors[0].Invoke(invocation);

                    methodArgs = invocation.Arguments;
                }

                IMethodReturnMessage mrm = new ReturnMessage(ret, methodArgs, methodArgs.Length,
                    methodMessage.LogicalCallContext, (IMethodCallMessage) msg);

                return mrm;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
