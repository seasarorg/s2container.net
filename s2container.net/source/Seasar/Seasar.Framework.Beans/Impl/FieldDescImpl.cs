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
using System.Reflection;
using Seasar.Framework.Util;

namespace Seasar.Framework.Beans.Impl
{
    /// <summary>
    /// フィールド(インスタンス変数)記述実装クラス
    /// </summary>
    public class FieldDescImpl : IFieldDesc
    {
        private readonly FieldInfo _fieldInfo;
        private bool? _isReadOnly;

        /// <summary>
        /// 元となるプロパティ
        /// </summary>
        public virtual FieldInfo Field
        {
            get { return _fieldInfo; }
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public virtual string Name
        {
            get { return _fieldInfo.Name; }
        }

        /// <summary>
        /// プロパティの型
        /// </summary>
        public virtual Type FieldType
        {
            get { return _fieldInfo.FieldType; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fieldInfo">フィールド情報(NotNull)</param>
        public FieldDescImpl(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException("fieldInfo");
            }
            _fieldInfo = fieldInfo;
        }

        /// <summary>
        /// 読み取り専用フラグ
        /// </summary>
        public virtual bool IsReadOnly()
        {
            if (_isReadOnly.HasValue) { return _isReadOnly.Value; }
            _isReadOnly = FieldUtil.IsReadOnly(_fieldInfo);
            return _isReadOnly.Value;
        }

        /// <summary>
        /// プロパティの値を返します。
        /// </summary>
        /// <param name="target">target</param>
        /// <returns>プロパティの値</returns>
        /// <exception cref="IllegalFieldRuntimeException">値の取得に失敗した場合。</exception>
        public virtual object GetValue(object target)
        {
            try
            {
                return _fieldInfo.GetValue(target);
            }
            catch (Exception ex)
            {
                throw new IllegalFieldRuntimeException(_fieldInfo.DeclaringType, Name, ex);
            }
        }

        /// <summary>
        /// プロパティに値を設定します。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <exception cref="IllegalFieldRuntimeException">値の設定に失敗した場合。</exception>
        public virtual void SetValue(object target, object value)
        {
            try
            {
                _fieldInfo.SetValue(target, value);
            }
            catch (Exception ex)
            {
                throw new IllegalFieldRuntimeException(_fieldInfo.DeclaringType, Name, ex);
            }
        }
    }
}
