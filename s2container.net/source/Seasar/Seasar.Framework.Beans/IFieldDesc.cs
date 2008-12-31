#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
    /// フィールド（インスタンス変数）情報記述インターフェース
    /// </summary>
	public interface IFieldDesc
	{
        /// <summary>
        /// 元となるプロパティ
        /// </summary>
        FieldInfo Field { get; }

        /// <summary>
        /// プロパティ名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// プロパティの型
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// 読み取り専用フラグ
        /// </summary>
        bool IsReadOnly();

        /// <summary>
        /// プロパティの値を返します。
        /// </summary>
        /// <param name="target">target</param>
        /// <returns>プロパティの値</returns>
        /// <exception cref="IllegalFieldRuntimeException">値の取得に失敗した場合。</exception>
        object GetValue(object target);

        /// <summary>
        /// プロパティに値を設定します。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <exception cref="IllegalFieldRuntimeException">値の設定に失敗した場合。</exception>
        void SetValue(object target, object value);
	}
}
