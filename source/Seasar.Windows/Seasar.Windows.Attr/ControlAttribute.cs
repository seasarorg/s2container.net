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
using System.Windows.Forms;

namespace Seasar.Windows.Attr
{
    /// <summary>
    /// コントロール属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class ControlAttribute : Attribute
    {
        /// <summary>
        /// 対象コントロール名
        /// </summary>
        private string _controlName;

        /// <summary>
        /// 対象コントロールのプロパティ
        /// </summary>
        private string _controlProperty;

        /// <summary>
        /// 取得プロパティ名
        /// </summary>
        private string _propertyName;

#if NET_1_1
#else
        /// <summary>
        /// 表示されるデータの書式を指定するフラグ
        /// </summary>
        private bool _formattingEnabled;

        /// <summary>
        /// データソースの更新タイミング
        /// </summary>
        private DataSourceUpdateMode _updateMode;

        /// <summary>
        /// データ ソースの値がDBNullである場合に、バインドされたコントロールプロパティに適用されるObject
        /// </summary>
        private object _nullValue;

        /// <summary>
        /// 値の表示方法を示す1つ以上の書式指定子文字
        /// </summary>
        private string _formatString;

#endif
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controlName">対象コントロール名</param>
        /// <param name="controlProperty">対象コントロールのプロパティ</param>
        /// <param name="propertyName">バインディングプロパティ名</param>
        public ControlAttribute(string controlName, string controlProperty, string propertyName)
        {
            this._controlName = controlName;
            this._controlProperty = controlProperty;
            this._propertyName = propertyName;

#if NET_1_1
#else
            this._formattingEnabled = true;
            this._updateMode = DataSourceUpdateMode.OnValidation;
            this._nullValue = null;
            this._formatString = "";
#endif

        }

#if NET_1_1
#else

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controlName">対象コントロール名</param>
        /// <param name="controlProperty">対象コントロールのプロパティ</param>
        /// <param name="propertyName">バインディングプロパティ名</param>
        /// <param name="updateMode">データソースの更新タイミング</param>
        public ControlAttribute(string controlName, string controlProperty, string propertyName, DataSourceUpdateMode updateMode)
        {
            this._controlName = controlName;
            this._controlProperty = controlProperty;
            this._propertyName = propertyName;
            this._formattingEnabled = true;
            this._updateMode = updateMode;
            this._nullValue = null;
            this._formatString = "";
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controlName">対象コントロール名</param>
        /// <param name="controlProperty">対象コントロールのプロパティ</param>
        /// <param name="propertyName">バインディングプロパティ名</param>
        /// <param name="formattingEnabled">表示されるデータの書式を指定する場合は true。それ以外の場合は false</param>
        /// <param name="updateMode">データソースの更新タイミング</param>
        public ControlAttribute(string controlName, string controlProperty, string propertyName, bool formattingEnabled, DataSourceUpdateMode updateMode)
        {
            this._controlName = controlName;
            this._controlProperty = controlProperty;
            this._propertyName = propertyName;
            this._formattingEnabled = formattingEnabled;
            this._updateMode = updateMode;
            this._nullValue = null;
            this._formatString = "";
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controlName">対象コントロール名</param>
        /// <param name="controlProperty">対象コントロールのプロパティ</param>
        /// <param name="propertyName">バインディングプロパティ名</param>
        /// <param name="formattingEnabled">表示されるデータの書式を指定する場合は true。それ以外の場合は false</param>
        /// <param name="updateMode">データソースの更新タイミング</param>
        /// <param name="nullValue">データ ソースの値がDBNullである場合に、バインドされたコントロールプロパティに適用されるObject</param>
        /// <param name="formatString">値の表示方法を示す1つ以上の書式指定子文字</param>
        public ControlAttribute(string controlName, string controlProperty, string propertyName, bool formattingEnabled, DataSourceUpdateMode updateMode, 
            object nullValue, string formatString)
        {
            this._controlName = controlName;
            this._controlProperty = controlProperty;
            this._propertyName = propertyName;
            this._formattingEnabled = formattingEnabled;
            this._updateMode = updateMode;
            this._nullValue = nullValue;
            this._formatString = formatString;
        }

#endif

        /// <summary>
        /// 対象コントロール名
        /// </summary>
        public string ControlName
        {
            get { return _controlName; }
            set { _controlName = value; }
        }

        /// <summary>
        /// 対象コントロールのプロパティ
        /// </summary>
        public string ControlProperty
        {
            get { return _controlProperty; }
            set { _controlProperty = value; }
        }

        /// <summary>
        /// 取得プロパティ名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

#if NET_1_1
#else
        /// <summary>
        /// 表示されるデータの書式を指定するフラグ
        /// </summary>
        public bool FormattingEnabled
        {
            get { return _formattingEnabled; }
            set { _formattingEnabled = value; }
        }

        /// <summary>
        /// データソースの更新タイミング
        /// </summary>
        public DataSourceUpdateMode UpdateMode
        {
            get { return _updateMode; }
            set { _updateMode = value; }
        }

        /// <summary>
        /// データ ソースの値がDBNullである場合に、バインドされたコントロールプロパティに適用されるObject
        /// </summary>
        public object NullValue
        {
            get { return _nullValue; }
            set { _nullValue = value; }
        }

        /// <summary>
        /// 値の表示方法を示す1つ以上の書式指定子文字
        /// </summary>
        public string FormatString
        {
            get { return _formatString; }
            set { _formatString = value; }
        }

#endif

    }
}