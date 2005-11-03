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
using System.Reflection;
using System.Collections;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
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
		private object target_;

        /// <summary>
        /// 適用するAspect
        /// </summary>
		private IAspect[] aspects_;

        /// <summary>
        /// 透過的プロクシを作成する型
        /// </summary>
		private Type type_;

        /// <summary>
        /// メソッドとそのクラスのインスタンスが属するS2コンテナに関する情報
        /// </summary>
		private Hashtable parameters_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">Aspectされるターゲットの型のType</param>
		/// <param name="aspects">適応するAspect</param>
		/// <param name="parameters">パラメータ</param>
		/// <param name="target">Aspectされるターゲット</param>
		public AopProxy(Type type,IAspect[] aspects,Hashtable parameters, object target) : base(type) 
		{
			type_       = type;
			target_     = target;
			aspects_    = aspects;
			parameters_ = parameters;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">Aspectされるターゲットの型のType</param>
		/// <param name="aspects">適応するAspect</param>
		/// <param name="parameters">パラメータ</param>
		public AopProxy(Type type,IAspect[] aspects,Hashtable parameters)
			: this(type,aspects,parameters,null)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">Aspectされるターゲットの型のType</param>
		/// <param name="aspects">適応するAspect</param>
		public AopProxy(Type type,IAspect[] aspects)
			: this(type,aspects,null)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">Aspectされるターゲットの型のType</param>
		public AopProxy(Type type)
			: this(type,null)
		{
		}

        /// <summary>
        /// 透過的プロクシを返す
        /// </summary>
        /// <returns>透過的プロクシのインスタンス</returns>
		public object Create()
		{
			return this.GetTransparentProxy();
		}

        /// <summary>
        /// 透過的プロクシを返す
        /// </summary>
        /// <param name="argTypes">透過的プロクシの対象となるクラスのコンストラクタの引数の型のリスト</param>
        /// <param name="args">透過的プロクシの対象となるクラスのコンストラクタの引数のリスト</param>
        /// <returns>透過的プロクシのインスタンス</returns>
		public object Create(Type[] argTypes,object[] args)
		{
			ConstructorInfo constructor = ClassUtil.GetConstructorInfo(type_,argTypes);
			target_ = ConstructorUtil.NewInstance(constructor,args);
			return this.GetTransparentProxy();
		}

        /// <summary>
        /// 透過的プロクシを返す
        /// </summary>
        /// <param name="argTypes">透過的プロクシの対象となるクラスのコンストラクタの引数の型のリスト</param>
        /// <param name="args">透過的プロクシの対象となるクラスのコンストラクタの引数のリスト</param>
        /// <param name="targetType">透過的プロクシの対象となるクラスの型</param>
        /// <returns>透過的プロクシのインスタンス</returns>
        public object Create(Type[] argTypes,object[] args,Type targetType)
		{
			ConstructorInfo constructor = ClassUtil.GetConstructorInfo(targetType,argTypes);
			target_ = ConstructorUtil.NewInstance(constructor,args);
			return this.GetTransparentProxy();
		}

        #region RealProxy メンバ

		/// <summary>
		/// AopProxyを通したオブジェクトのメソッドが実行されるとこのメソッドが呼ばれます
		/// </summary>
		/// <param name="msg">IMessage</param>
		/// <returns>IMessage</returns>
		/// <seealso="System.Runtime.Remoting.Proxies">System.Runtime.Remoting.Proxies</seealso>
		public override IMessage Invoke(IMessage msg) 
		{
			if(target_ == null)
			{
				if(!type_.IsInterface) target_ = Activator.CreateInstance(type_);
				if(target_==null) target_ = new object();
			}
			IMethodMessage methodMessage = msg as IMethodMessage;
			MethodBase method = methodMessage.MethodBase;

			ArrayList interceptorList = new ArrayList();

			if(aspects_ != null)
			{
				// 定義されたAspectからInterceptorのリストの作成
				foreach(IAspect aspect in aspects_)
				{
					IPointcut pointcut = aspect.Pointcut;
					// IPointcutよりAdvice(Interceptor)を挿入するか確認
					if(pointcut == null || pointcut.IsApplied(method)) 
					{
						// Aspectを適用する場合
						interceptorList.Add(aspect.MethodInterceptor);
					}
				}
			}

			Object ret = null;

			if(interceptorList.Count == 0)
			{
				// Interceptorを挿入しない場合
				ret = method.Invoke(target_,methodMessage.Args);
			}
			else
			{
				// Interceptorを挿入する場合
				IMethodInterceptor[] interceptors = (IMethodInterceptor[])
					interceptorList.ToArray(typeof(IMethodInterceptor));
				IMethodInvocation invocation = new MethodInvocationImpl(target_,
					method,methodMessage.Args,interceptors,parameters_);
				ret = interceptors[0].Invoke(invocation);
			}

			return new ReturnMessage(ret, null, 0, 
				methodMessage.LogicalCallContext, (IMethodCallMessage)msg);

		}

        #endregion

	}   // AopProxy
}
