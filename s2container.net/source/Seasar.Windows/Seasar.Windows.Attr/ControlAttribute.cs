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
using System.Windows.Forms;

namespace Seasar.Windows.Attr
{
    /// <summary>
    /// コントロール属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class ControlAttribute : Attribute
    {
#if NET_1_1
#else

#endif
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="controlName">対象コントロール名</param>
        /// <param name="controlProperty">対象コントロールのプロパティ</param>
        /// <param name="propertyName">バインディングプロパティ名</param>
        public ControlAttribute(string controlName, string controlProperty, string propertyName)
        {
            ControlName = controlName;
            ControlProperty = controlProperty;
            PropertyName = propertyName;

#if NET_1_1
#else
            FormattingEnabled = true;
            UpdateMode = DataSourceUpdateMode.OnValidation;
            NullValue = null;
            FormatString = "";
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
            ControlName = controlName;
            ControlProperty = controlProperty;
            PropertyName = propertyName;
            FormattingEnabled = true;
            UpdateMode = updateMode;
            NullValue = null;
            FormatString = "";
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
            ControlName = controlName;
            ControlProperty = controlProperty;
            PropertyName = propertyName;
            FormattingEnabled = formattingEnabled;
            UpdateMode = updateMode;
            NullValue = null;
            FormatString = "";
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
            ControlName = controlName;
            ControlProperty = controlProperty;
            PropertyName = propertyName;
            FormattingEnabled = formattingEnabled;
            UpdateMode = updateMode;
            NullValue = nullValue;
            FormatString = formatString;
        }

#endif

        /// <summary>
        /// 対象コントロール名
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// 対象コントロールのプロパティ
        /// </summary>
        public string ControlProperty { get; set; }

        /// <summary>
        /// 取得プロパティ名
        /// </summary>
        public string PropertyName { get; set; }

#if NET_1_1
#else
        /// <summary>
        /// 表示されるデータの書式を指定するフラグ
        /// </summary>
        public bool FormattingEnabled { get; set; }

        /// <summary>
        /// データソースの更新タイミング
        /// </summary>
        public DataSourceUpdateMode UpdateMode { get; set; }

        /// <summary>
        /// データ ソースの値がDBNullである場合に、バインドされたコントロールプロパティに適用されるObject
        /// </summary>
        public object NullValue { get; set; }

        /// <summary>
        /// 値の表示方法を示す1つ以上の書式指定子文字
        /// </summary>
        public string FormatString { get; set; }

#endif

    }
}
