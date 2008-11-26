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

using System;
using System.Reflection;
using Seasar.Framework.Util;

namespace Seasar.Framework.Beans.Impl
{
    /// <summary>
    /// メソッド情報記述クラス
    /// </summary>
	public class MethodDescImpl : IMethodDesc
	{
	    private readonly MethodInfo _methodInfo;
        private bool? _canOverride;

        private ParameterInfo[] _cachedParameterInfo;
        private Type _cachedReturnType;

        /// <summary>
        /// 元となるメソッド情報
        /// </summary>
        public virtual MethodInfo Method
	    {
            get { return _methodInfo; }
	    }

	    /// <summary>
	    /// メソッド名
	    /// </summary>
        public virtual string Name
	    {
            get { return _methodInfo.Name; }
	    }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="methodInfo">メソッド情報</param>
        public MethodDescImpl(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }

	    /// <summary>
	    /// パラメータ情報
	    /// </summary>
	    /// <returns></returns>
        public virtual ParameterInfo[] GetParameterInfos()
	    {
            if(_cachedParameterInfo == null)
            {
                _cachedParameterInfo = _methodInfo.GetParameters();
            }
	        return _cachedParameterInfo;
	    }

	    /// <summary>
	    /// 戻り値の型を返す
	    /// </summary>
	    /// <returns>戻り値の型（voidならnull)</returns>
        public virtual Type GetReturnType()
	    {
            if(_cachedReturnType == null)
            {
                _cachedReturnType = _methodInfo.ReturnType;
            }
            if (_cachedReturnType.Equals(typeof(void))) { return null; }
            return _cachedReturnType;
	    }

        /// <summary>
        /// Overridできるかどうか判定する
        /// </summary>
        /// <returns></returns>
        public virtual bool CanOverride()
        {
            if(_canOverride.HasValue) { return _canOverride.Value; }

            _canOverride = MethodUtil.CanOverride(_methodInfo);
            return _canOverride.Value;
        }

	    /// <summary>
	    /// メソッド呼び出し
	    /// </summary>
	    /// <param name="obj">メソッドをもつオブジェクト</param>
	    /// <param name="parameters">メソッドに渡す引数</param>
	    /// <returns>メソッドの戻り値</returns>
        public virtual object Invoke(object obj, params object[] parameters)
	    {
	        object[] p = parameters;
	        if (p != null)
	        {
	        }
	        else
	        {
	            p = new object[] {};
	        }
	        return MethodUtil.Invoke(_methodInfo, obj, p);
	    }
	}
}
