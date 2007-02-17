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
using System.Collections;

namespace Seasar.Framework.Aop.Interceptors
{
	/// <summary>
	/// Interceptの対象となるメソッドとインスタンスをMockにします
	/// </summary>
	public class MockInterceptor : AbstractInterceptor
	{

		/// <summary>
		/// Mockの戻り値
		/// </summary>
		private Hashtable returnValues_ = new Hashtable();

		/// <summary>
		/// Mockの例外
		/// </summary>
		private Hashtable exceptions_ = new Hashtable();

		/// <summary>
		/// メソッドが呼び出し済みかどうか示すフラグ
		/// </summary>
		private Hashtable invokedMethods_ = new Hashtable();

		/// <summary>
		/// メソッドを呼び出したときの引数
		/// </summary>
		private Hashtable invokedMethodArgs_ = new Hashtable();
		
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MockInterceptor() 
        {
		}
		
        #region IMethodInterceptor クラス

        /// <summary>
        /// メソッドがInterceptされる場合、このメソッドが呼び出されます
        /// </summary>
        /// <param name="invocation">IMethodInvocation</param>
        /// <returns>Interceptされるメソッドの戻り値</returns>
        public override object Invoke(IMethodInvocation invocation)
        {
            string methodName = invocation.Method.Name;
            invokedMethods_[methodName] = true;
            invokedMethodArgs_[methodName] = invocation.Arguments;

            if (exceptions_.ContainsKey(methodName)) 
            {
                throw exceptions_[methodName] as Exception;
            } 
            else if (exceptions_.ContainsKey("")) 
            {
                throw exceptions_[""] as Exception;
            } 
            else if (returnValues_.ContainsKey(methodName)) 
            {
                return returnValues_[methodName];
            } 
            else 
            {
                return returnValues_[""];
            }
        }

        #endregion

        /// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">Mockの戻り値</param>
		public MockInterceptor(object value) 
        {
			SetReturnValue(value);
		}

        /// <summary>
        /// 戻り値の設定
        /// </summary>
        /// <remarks>
        /// Mockのすべてのメソッドの戻り値を設定します。
        /// </remarks>
        /// <param name="returnValue">戻り値</param>
		public void SetReturnValue(object returnValue) {
			SetReturnValue("", returnValue);
		}

        /// <summary>
        /// 戻り値の設定
        /// </summary>
        /// <remarks>
        /// Mockの指定された名前のメソッドの戻り値を設定します。
        /// </remarks>
        /// <param name="methodName">戻り値を設定するメソッド名</param>
        /// <param name="returnValue">戻り値</param>
		public void SetReturnValue(String methodName, object returnValue) 
        {
			returnValues_[methodName] = returnValue;
		}

        /// <summary>
        /// 例外の設定
        /// </summary>
        /// <remarks>
        /// Mockのすべてのメソッドに例外を設定します。
        /// </remarks>
        /// <param name="exception">例外</param>
		public void SetThrowable(Exception exception) 
        {
			SetThrowable("", exception);
		}
		
        /// <summary>
        /// 例外の設定
        /// </summary>
        /// <remarks>
        /// Mockのすべてのメソッドに例外を設定します。
        /// </remarks>
        /// <param name="methodName">例外を設定するメソッド名</param>
        /// <param name="exception">例外</param>
        public void SetThrowable(String methodName, Exception exception) 
        {
			exceptions_[methodName] = exception;
		}

        /// <summary>
        /// Mockのメソッドが既に呼び出されているか判定します
        /// </summary>
        /// <param name="methodName">呼び出されているかどうか判定するメソッド命</param>
        /// <returns>Mockのメソッドが既に呼び出されているか</returns>
		public bool IsInvoked(String methodName) {
			return invokedMethods_.ContainsKey(methodName);
		}

        /// <summary>
        /// Mockのメソッドが呼ばれたときの引数を取得します
        /// </summary>
        /// <param name="methodName">引数を取得するメソッド名</param>
        /// <returns>引数のリスト</returns>
		public object[] GetArgs(String methodName) {
			return (object[]) invokedMethodArgs_[methodName];
		}

	}
}
