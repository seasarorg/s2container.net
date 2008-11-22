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

namespace Seasar.Framework.Beans
{
    /// <summary>
    /// プロパティを扱うためのインターフェースです。
    /// </summary>
	public interface IPropertyDesc
	{
        /// <summary>
        /// 元となるプロパティ
        /// </summary>
        PropertyInfo Property { get; }

        /// <summary>
        /// プロパティ名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// プロパティの型
        /// </summary>
        Type PropertyType { get; }

        /// <summary>
        /// getterメソッド
        /// </summary>
        /// <exception cref="MethodNotFoundRuntimeException"></exception>
        IMethodDesc ReadMethod { get; }

        /// <summary>
        /// setterメソッド
        /// </summary>
        /// <exception cref="MethodNotFoundRuntimeException"></exception>
        IMethodDesc WriteMethod { get; }

        /// <summary>
        /// getterメソッドを持っているかどうか返します。
        /// </summary>
        /// <returns>getterメソッドを持っているかどうか</returns>
        bool HasReadMethod();

        /// <summary>
        /// setterメソッドを持っているかどうか返します。
        /// </summary>
        /// <returns>setterメソッドを持っているかどうか</returns>
        bool HasWriteMethod();

        /// <summary>
        /// プロパティの値を返します。
        /// </summary>
        /// <param name="target">target</param>
        /// <returns>プロパティの値</returns>
        /// <exception cref="IllegalPropertyRuntimeException">値の取得に失敗した場合。</exception>
        Object GetValue(Object target);

        /// <summary>
        /// プロパティに値を設定します。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <exception cref="IllegalPropertyRuntimeException">値の設定に失敗した場合。</exception>
        void SetValue(Object target, Object value);
	}
}
