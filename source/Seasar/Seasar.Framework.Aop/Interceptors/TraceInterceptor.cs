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
using System.Text;
using System.Reflection;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Framework.Aop.Interceptors
{
	/// <summary>
	/// 呼び出されたメソッドのトレースをログに出力するInterceptor
	/// </summary>
	public class TraceInterceptor : AbstractInterceptor
	{
        /// <summary>
        /// ロガー
        /// </summary>
		private static Logger logger_ = Logger.GetLogger(typeof(TraceInterceptor));

        #region IMethodInterceptor クラス

        /// <summary>
        /// メソッドがInterceptされる場合、このメソッドが呼び出されます
        /// </summary>
        /// <param name="invocation">IMethodInvocation</param>
        /// <returns>Interceptされるメソッドの戻り値</returns>
        public override object Invoke(IMethodInvocation invocation)
		{
			MethodBase mb = invocation.Method;
			StringBuilder buf = new StringBuilder(100);
			buf.Append(mb.DeclaringType.FullName);
			buf.Append("#");
			buf.Append(mb.Name);
			buf.Append("(");
			object[] args = invocation.Arguments;
			if(args != null && args.Length > 0)
			{
				for(int i=0;i<args.Length;i++)
				{
					buf.Append(args[i]);
					buf.Append(", ");
				}
				buf.Length = buf.Length - 2;
			}
			buf.Append(")");
			logger_.Debug("BEGIN " + buf.ToString());
			object ret = null;
			Exception cause = null;
			try
			{
				ret = invocation.Proceed();
				buf.Append(" : ");
				buf.Append(ret);
			}
			catch(Exception ex)
			{
				buf.Append(" Exception:");
				buf.Append(ex);
				cause = ex;
			}
			logger_.Debug("END " + buf.ToString());
			if(cause == null)
			{
				return ret;
			}
			else
			{
                ExceptionUtil.SaveStackTraceToRemoteStackTraceString(cause);
				throw cause;
			}
		}

        #endregion

	}
}
