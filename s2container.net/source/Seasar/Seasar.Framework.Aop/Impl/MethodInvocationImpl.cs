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
		private object target_;

		/// <summary>
		/// 呼び出されるメソッド
		/// </summary>
		private MethodBase method_;

		/// <summary>
		/// メソッドをInterceptするInterceptorの配列
		/// </summary>
		private IMethodInterceptor[] interceptors_;

		/// <summary>
		/// 処理されているInterceptorの再帰レベル
		/// </summary>
		private int interceptorsIndex_ = 1;

		/// <summary>
		/// メソッドの引数
		/// </summary>
		private object[] arguments_;

		/// <summary>
		/// メソッドとそのクラスのインスタンスが属するS2コンテナに関する情報
		/// </summary>
		private Hashtable parameters_;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="target">Interceptされるオブジェクト</param>
		/// <param name="method">InterceptされるメソッドのMethodBase</param>
		/// <param name="interceptors">メソッドのInterceptするInterceptor</param>
		/// <param name="parameters">Interceptされるメソッドとそのクラスのインスタンスが属するS2コンテナに関する情報</param>
		public MethodInvocationImpl(object target,MethodBase method,
			object[] arguments,IMethodInterceptor[] interceptors,Hashtable parameters)
		{
			if(target==null) throw new NullReferenceException("target");
			if(method==null) throw new NullReferenceException("method");
			if(interceptors==null) throw new NullReferenceException("interceptors");
			target_       = target;
			method_       = method;
			arguments_    = arguments;
			interceptors_ = interceptors;
			parameters_   = parameters;
		}

		#region IMethodInvocation メンバ

		/// <summary>
		/// InterceptされるメソッドのMethod
		/// </summary>
		public MethodBase Method
		{
			get
			{
				return method_;
			}
		}

		/// <summary>
		/// Interceptされるオブジェクト
		/// </summary>
		public Object Target
		{
			get
			{
				return target_;
			}
		}

		/// <summary>
		/// Interceptされるメソッドの引数
		/// </summary>
		public Object[] Arguments
		{
			get
			{
				return arguments_;
			}
		}

		/// <summary>
		/// メソッドの呼び出し
		/// </summary>
		/// <remarks>
		/// 他にチェーンされているInterceptorがあれば、Interceptorを呼び出します（再帰的に呼び出される）。
		/// 他にチェーンされているInterceptorが無ければ、Interceptされているメソッドを実行します。
		/// <remarks>
		/// <returns>Interceptされたメソッドの戻り値</returns>
		public Object Proceed()
		{
			while(interceptorsIndex_ < interceptors_.Length)
			{
				// 他にInterceptorがあれば、Interceptorを呼び出す
				return interceptors_[interceptorsIndex_++].Invoke(this);
			}

            try
            {
                // Interceptされたメソッドを実行する
                return method_.Invoke(target_, arguments_);
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
			get { return target_.GetType(); }
		}

		/// <summary>
		/// メソッドとそのクラスのインスタンスが属するS2コンテナに関する情報
		/// </summary>
		public object GetParameter(string name)
		{
			return parameters_[name];
		}

		#endregion
	}
}
