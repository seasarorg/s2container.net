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
using Seasar.Framework.Beans.Factory;
using System.Reflection;

namespace Seasar.Framework.Beans.Impl
{
    /// <summary>
    /// プロパティ情報記述クラス
    /// </summary>
	public class PropertyDescImpl : IPropertyDesc
	{
	    private readonly PropertyInfo _propertyInfo;
	    private IMethodDesc _readMethod;
	    private IMethodDesc _writeMethod;

        /// <summary>
        /// 元となるプロパティ情報
        /// </summary>
        public virtual PropertyInfo Property
        {
            get { return _propertyInfo; }
        }

	    /// <summary>
	    /// プロパティ名
	    /// </summary>
        public virtual string Name
	    {
            get { return _propertyInfo.Name; }
	    }

	    /// <summary>
	    /// プロパティの型
	    /// </summary>
        public virtual Type PropertyType
	    {
            get { return _propertyInfo.PropertyType; }
	    }

	    /// <summary>
	    /// getterメソッド
	    /// </summary>
        /// <exception cref="MethodNotFoundRuntimeException"></exception>
        public virtual IMethodDesc ReadMethod
	    {
	        get
	        {
	            if(_readMethod == null)
	            {
	                MethodInfo mi = _propertyInfo.GetGetMethod();
                    if(mi == null)
                    {
                        throw new MethodNotFoundRuntimeException(_propertyInfo.DeclaringType,
                            "get", null);
                    }
	                _readMethod = MethodDescFactory.NewMethodDesc(mi);
	            }
	            return _readMethod;
	        }
	    }

	    /// <summary>
	    /// setterメソッド
	    /// </summary>
        /// <exception cref="MethodNotFoundRuntimeException"></exception>
        public virtual IMethodDesc WriteMethod
	    {
            get
            {
                if (_writeMethod == null)
                {
                    MethodInfo mi = _propertyInfo.GetSetMethod();
                    if (mi == null)
                    {
                        throw new MethodNotFoundRuntimeException(_propertyInfo.DeclaringType,
                            "set", null);
                    }
                    _writeMethod = MethodDescFactory.NewMethodDesc(mi);
                }
                return _writeMethod;
            }
	    }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="proeprtyInfo"></param>
        public PropertyDescImpl(PropertyInfo proeprtyInfo)
        {
            _propertyInfo = proeprtyInfo;
        }

	    /// <summary>
	    /// getterメソッドを持っているかどうか返します。
	    /// </summary>
	    /// <returns>getterメソッドを持っているかどうか</returns>
        public virtual bool HasReadMethod()
	    {
	        return _propertyInfo.CanRead;
	    }

	    /// <summary>
	    /// setterメソッドを持っているかどうか返します。
	    /// </summary>
	    /// <returns>setterメソッドを持っているかどうか</returns>
        public virtual bool HasWriteMethod()
	    {
	        return _propertyInfo.CanWrite;
	    }

	    /// <summary>
	    /// プロパティの値を返します。
	    /// </summary>
	    /// <param name="target">target</param>
	    /// <returns>プロパティの値</returns>
        /// <exception cref="IllegalPropertyRuntimeException">値の設定に失敗した場合。</exception>
        public virtual object GetValue(object target)
	    {
	        try
	        {
                return ReadMethod.Invoke(target);
	        }
	        catch (Exception ex)
	        {
                throw new IllegalPropertyRuntimeException(
                    _propertyInfo.DeclaringType, Name, ex);
	        }
	    }

	    /// <summary>
	    /// プロパティに値を設定します。
	    /// </summary>
	    /// <param name="target"></param>
	    /// <param name="value"></param>
	    /// <exception cref="IllegalPropertyRuntimeException">値の設定に失敗した場合。</exception>
        public virtual void SetValue(object target, object value)
	    {
	        try
	        {
                WriteMethod.Invoke(target, value);
	        }
	        catch (Exception ex)
	        {
	            throw new IllegalPropertyRuntimeException(
	                _propertyInfo.DeclaringType, Name, ex);
	        }            
	    }
	}
}
