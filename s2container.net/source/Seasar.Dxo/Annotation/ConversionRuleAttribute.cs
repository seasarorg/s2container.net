#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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

namespace Seasar.Dxo.Annotation
{
    /// <summary>
    /// DataExchange(DxO)に対して、特定のプロパティに対する変換方法を記述するカスタム属性
    /// </summary>
    /// <seealso cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class ConversionRuleAttribute : Attribute
    {
        private string _propertyName;
        private string _targetPropertyName;
        private Type _propertyConverter;
        private bool _ignore;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="propertyName">変換をカスタマイズするプロパティ名</param>
        /// <param name="targetPropertyName">変換先となるプロパティ名(省略時、変換先はPropertyNameと同一)</param>
        /// <param name="propertyConverter">対象のプロパティを変換するためのコンバータの型</param>
        /// <param name="ignore">型変換を行わないことを指示するフラグ</param>
        public ConversionRuleAttribute(string propertyName, string targetPropertyName, Type propertyConverter, bool ignore)
        {
            this._propertyName = propertyName;
            if (!string.IsNullOrEmpty(targetPropertyName))
                this._targetPropertyName = targetPropertyName;
            else
                this._targetPropertyName = propertyName;
            this._propertyConverter = propertyConverter;
            this._ignore = ignore;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="propertyName">変換をカスタマイズするプロパティ名</param>
        /// <param name="targetPropertyName">変換先となるプロパティ名(省略時、変換先はPropertyNameと同一)</param>
        /// <param name="propertyConverter">対象のプロパティを変換するためのコンバータの型</param>
        public ConversionRuleAttribute(string propertyName, string targetPropertyName, Type propertyConverter)
        {
            this._propertyName = propertyName;
            if (!String.IsNullOrEmpty(targetPropertyName))
                this._targetPropertyName = targetPropertyName;
            else
                this._targetPropertyName = propertyName;
            this._propertyConverter = propertyConverter;
            this._ignore = false;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="propertyName">変換をカスタマイズするプロパティ名</param>
        /// <param name="targetPropertyName">変換先となるプロパティ名(省略時、変換先はPropertyNameと同一)</param>
        public ConversionRuleAttribute(string propertyName, string targetPropertyName)
        {
            this._propertyName = propertyName;
            if (!String.IsNullOrEmpty(targetPropertyName))
                this._targetPropertyName = targetPropertyName;
            else
                this._targetPropertyName = propertyName;
            _propertyConverter = null;
            _ignore = false;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConversionRuleAttribute()
        {
            this._propertyName = String.Empty;
            this._targetPropertyName = String.Empty;
            this._propertyConverter = null;
            this._ignore = false;
        }

        /// <summary>
        /// 変換をカスタマイズするプロパティ名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        /// <summary>
        /// 変換先となるプロパティ名
        /// (省略した場合、変換先はPropertyNameと同一とみなされます)
        /// </summary>
        public string TargetPropertyName
        {
            get { return _targetPropertyName; }
            set { _targetPropertyName = value; }
        }

        /// <summary>
        /// 対象のプロパティを変換するためのコンバータの型
        /// (この型のデフォルトコンストラクタで、コンバータのインスタンスを生成することが
        /// できなくてはなりません)
        /// </summary>
        public Type PropertyConverter
        {
            get { return _propertyConverter; }
            set { _propertyConverter = value; }
        }

        /// <summary>
        /// プロパティ名と共に指定することで、型変換を行わないことを指示するフラグ
        /// </summary>
        public bool Ignore
        {
            get { return _ignore; }
            set { _ignore = value; }
        }
    }
}
