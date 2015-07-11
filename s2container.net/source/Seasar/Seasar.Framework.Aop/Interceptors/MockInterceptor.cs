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
        private readonly Hashtable _returnValues = new Hashtable();

        /// <summary>
        /// Mockの例外
        /// </summary>
        private readonly Hashtable _exceptions = new Hashtable();

        /// <summary>
        /// メソッドが呼び出し済みかどうか示すフラグ
        /// </summary>
        private readonly Hashtable _invokedMethods = new Hashtable();

        /// <summary>
        /// メソッドを呼び出したときの引数
        /// </summary>
        private readonly Hashtable _invokedMethodArgs = new Hashtable();

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
            _invokedMethods[methodName] = true;
            _invokedMethodArgs[methodName] = invocation.Arguments;

            if (_exceptions.ContainsKey(methodName))
            {
                throw (Exception) _exceptions[methodName];
            }
            else if (_exceptions.ContainsKey(string.Empty))
            {
                throw (Exception) _exceptions[string.Empty];
            }
            else if (_returnValues.ContainsKey(methodName))
            {
                return _returnValues[methodName];
            }
            else
            {
                return _returnValues[string.Empty];
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
        public void SetReturnValue(object returnValue)
        {
            SetReturnValue(string.Empty, returnValue);
        }

        /// <summary>
        /// 戻り値の設定
        /// </summary>
        /// <remarks>
        /// Mockの指定された名前のメソッドの戻り値を設定します。
        /// </remarks>
        /// <param name="methodName">戻り値を設定するメソッド名</param>
        /// <param name="returnValue">戻り値</param>
        public void SetReturnValue(string methodName, object returnValue)
        {
            _returnValues[methodName] = returnValue;
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
            SetThrowable(string.Empty, exception);
        }

        /// <summary>
        /// 例外の設定
        /// </summary>
        /// <remarks>
        /// Mockのすべてのメソッドに例外を設定します。
        /// </remarks>
        /// <param name="methodName">例外を設定するメソッド名</param>
        /// <param name="exception">例外</param>
        public void SetThrowable(string methodName, Exception exception)
        {
            _exceptions[methodName] = exception;
        }

        /// <summary>
        /// Mockのメソッドが既に呼び出されているか判定します
        /// </summary>
        /// <param name="methodName">呼び出されているかどうか判定するメソッド命</param>
        /// <returns>Mockのメソッドが既に呼び出されているか</returns>
        public bool IsInvoked(string methodName)
        {
            return _invokedMethods.ContainsKey(methodName);
        }

        /// <summary>
        /// Mockのメソッドが呼ばれたときの引数を取得します
        /// </summary>
        /// <param name="methodName">引数を取得するメソッド名</param>
        /// <returns>引数のリスト</returns>
        public object[] GetArgs(string methodName)
        {
            return (object[]) _invokedMethodArgs[methodName];
        }
    }
}
