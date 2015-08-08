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
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Seasar.Framework.Util;
using Seasar.Windows.Attr;
using Seasar.Windows.Seasar.Windows.Utils;

namespace Seasar.Windows
{
    /// <summary>
    /// S2WindowsFormクラス
    /// </summary>
    public partial class S2Form : Form
    {
        /// <summary>
        /// バインディングデータ
        /// </summary>
        private object _dataSource;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public S2Form()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームにセットするデータ
        /// </summary>
        /// <remarks>フォームをセットするときは一度バインディングを削除する</remarks>
        public object DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;

                _RemoveBindings();
                if (value != null)
                    _SetDataToControls();
            }
        }

        /// <summary>
        /// バインディング用デフォルトプロパティ
        /// </summary>
        public string DefaultProperty { get; set; } = "Text";

        /// <summary>
        /// Gridに列を追加する
        /// </summary>
        /// <param name="propertyName">バインディングデータのプロパティ</param>
        /// <param name="data">挿入するデータ</param>
        /// <param name="row">挿入位置</param>
        /// <returns>追加行数</returns>
        protected virtual int AddRow(string propertyName, object data, int row)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001, "propertyName"));
            if (row < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002, "row"));

            var ret = 0;
            // 修飾子を取得する
            var formType = GetType();
            var attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
            var prefix = string.Empty;
            var suffix = string.Empty;
            _GetModifier(formType, ref prefix, ref suffix);

            var type = _dataSource.GetExType();
            IDictionary<string, Control> hashtable = new Dictionary<string, Control>();
            _GetAllControls(this, hashtable);

            var pis = type.GetProperties();
            foreach (var info in pis)
            {
                if (info.Name.ToLower() == propertyName.ToLower())
                {
                    var isFind = false;
                    foreach (var o in attributes)
                    {
                        // 個別バインディングを行う
                        var attribute = o as ControlAttribute;
                        if (attribute != null)
                        {
                            if (info.Name.ToLower() == attribute.PropertyName.ToLower()
                                && hashtable.ContainsKey(attribute.ControlName.ToLower()) )
                            {
                                var control = hashtable[attribute.ControlName.ToLower()];
                                ret += _AddSingleRow(info, control, attribute, data, row);
                                isFind = true;
                            }
                        }
                    }
                    if ( !isFind )
                    {
                        // 自動バインディングを行う
                        if ( hashtable.ContainsKey(prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()) )
                        {
                            var control = hashtable[prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()];
                            var attr = new ControlAttribute(control.Name, DefaultProperty, propertyName);
                            ret += _AddSingleRow(info, control, attr, data, row);
                            break;
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gridから列を削除する
        /// </summary>
        /// <param name="propertyName">バインディングデータのプロパティ</param>
        /// <param name="row">行</param>
        /// <returns>削除行数</returns>
        protected virtual int DeleteRow(string propertyName, int row)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001, "propertyName"));
            if (row < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002, "row"));

            var ret = 0;
            // 修飾子を取得する
            var formType = GetType();
            var attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
            var prefix = string.Empty;
            var suffix = string.Empty;
            _GetModifier(formType, ref prefix, ref suffix);

            var type = _dataSource.GetExType();
            IDictionary<string, Control> hashtable = new Dictionary<string, Control>();
            _GetAllControls(this, hashtable);

            var pis = type.GetProperties();
            foreach (var info in pis)
            {
                if (info.Name.ToLower() == propertyName.ToLower())
                {
                    var isFind = false;
                    foreach (var o in attributes)
                    {
                        // 個別バインディングを行う
                        var attribute = o as ControlAttribute;
                        if (attribute != null)
                        {
                            if (info.Name.ToLower() == attribute.PropertyName.ToLower()
                                && hashtable.ContainsKey(attribute.ControlName.ToLower()) )
                            {
                                var control = hashtable[attribute.ControlName.ToLower()];
                                ret += _DeleteSingleRow(info, control, attribute, row);
                                isFind = true;
                            }
                        }
                    }
                    if ( !isFind )
                    {
                        // 自動バインディングを行う
                        if ( hashtable.ContainsKey(prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()) )
                        {
                            var control = hashtable[prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()];
                            var attr = new ControlAttribute(control.Name, DefaultProperty, propertyName);
                            ret += _DeleteSingleRow(info, control, attr, row);
                            break;
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gridから列を削除する
        /// </summary>
        /// <param name="propertyName">バインディングデータのプロパティ</param>
        /// <param name="startRow">削除開始行</param>
        /// <param name="endRow">削除終了行</param>
        /// <returns>削除行数</returns>
        protected virtual int DeleteRows(string propertyName, int startRow, int endRow)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001, "propertyName"));
            if (startRow < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002, "startRow"));
            if (endRow < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002, "endRow"));
            if (startRow > endRow)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0004, "endRow"));

            // 修飾子を取得する
            var formType = GetType();
            var attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
            var prefix = string.Empty;
            var suffix = string.Empty;
            _GetModifier(formType, ref prefix, ref suffix);

            var ret = 0;
            var type = _dataSource.GetExType();
            IDictionary<string, Control> hashtable = new Dictionary<string, Control>();
            _GetAllControls(this, hashtable);

            var pis = type.GetProperties();
            foreach (var info in pis)
            {
                if (info.Name.ToLower() == propertyName.ToLower())
                {
                    var isFind = false;
                    foreach (var o in attributes)
                    {
                        // 個別バインディングを行う
                        var attribute = o as ControlAttribute;
                        if (attribute != null)
                        {
                            if (info.Name.ToLower() == attribute.PropertyName.ToLower()
                                && hashtable.ContainsKey(attribute.ControlName.ToLower()) )
                            {
                                var control = hashtable[attribute.ControlName.ToLower()];
                                for ( var i = endRow ; i >= startRow ; i-- )
                                    ret += _DeleteSingleRow(info, control, attribute, i);

                                isFind = true;
                            }
                        }
                    }
                    if ( !isFind )
                    {
                        // 自動バインディングを行う
                        if ( hashtable.ContainsKey(prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()) )
                        {
                            var control = hashtable[prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()];
                            for ( var i = endRow ; i >= startRow ; i-- )
                            {
                                var attr =
                                    new ControlAttribute(control.Name, DefaultProperty, propertyName);
                                ret += _DeleteSingleRow(info, control, attr, i);
                            }
                            break;
                        }

                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Gridの列を一行上げる
        /// </summary>
        /// <param name="propertyName">バインディングデータのプロパティ</param>
        /// <param name="row">対象行</param>
        public virtual void MoveUpRow(string propertyName, int row)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001, "propertyName"));
            if (row < 1)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0005, "row"));

            // 修飾子を取得する
            var formType = GetType();
            var attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
            var prefix = string.Empty;
            var suffix = string.Empty;
            _GetModifier(formType, ref prefix, ref suffix);

            var type = _dataSource.GetExType();
            IDictionary<string, Control> hashtable = new Dictionary<string, Control>();
            _GetAllControls(this, hashtable);

            var pis = type.GetProperties();
            foreach (var info in pis)
            {
                if (info.Name.ToLower() == propertyName.ToLower())
                {
                    var isFind = false;
                    foreach (var o in attributes)
                    {
                        // 個別バインディングを行う
                        var attribute = o as ControlAttribute;
                        if (attribute != null)
                        {
                            if (info.Name.ToLower() == attribute.PropertyName.ToLower()
                                && hashtable.ContainsKey(attribute.ControlName.ToLower()) )
                            {
                                var control = hashtable[attribute.ControlName.ToLower()];
                                _MoveSingleRow(info, control, attribute, row, MovingDirection.Upper);
                                isFind = true;
                            }
                        }
                    }
                    if ( !isFind )
                    {
                        // 自動バインディングを行う
                        if ( hashtable.ContainsKey(prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()) )
                        {
                            var control = hashtable[prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()];
                            var attr = new ControlAttribute(control.Name, DefaultProperty, propertyName);
                            _MoveSingleRow(info, control, attr, row, MovingDirection.Upper);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gridの列を一行下げる
        /// </summary>
        /// <param name="propertyName">バインディングデータのプロパティ</param>
        /// <param name="row">対象行</param>
        public virtual void MoveDownRow(string propertyName, int row)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001, "propertyName"));
            if (row < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002, "row"));

            // 修飾子を取得する
            var formType = GetType();
            var attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
            var prefix = string.Empty;
            var suffix = string.Empty;
            _GetModifier(formType, ref prefix, ref suffix);

            var type = _dataSource.GetExType();
            IDictionary<string, Control> hashtable = new Dictionary<string, Control>();
            _GetAllControls(this, hashtable);

            var pis = type.GetProperties();
            foreach (var info in pis)
            {
                if (info.Name.ToLower() == propertyName.ToLower())
                {
                    var isFind = false;
                    foreach (var o in attributes)
                    {
                        // 個別バインディングを行う
                        var attribute = o as ControlAttribute;
                        if (attribute != null)
                        {
                            if (info.Name.ToLower() == attribute.PropertyName.ToLower()
                                && hashtable.ContainsKey(attribute.ControlName.ToLower()))
                            {
                                var control = hashtable[attribute.ControlName.ToLower()];
                                _MoveSingleRow(info, control, attribute, row, MovingDirection.Lower);
                                isFind = true;
                            }
                        }
                    }
                    if ( !isFind )
                    {
                        // 自動バインディングを行う
                        if ( hashtable.ContainsKey(prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()) )
                        {
                            var control = hashtable[prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()];
                            var attr = new ControlAttribute(control.Name, DefaultProperty, propertyName);
                            _MoveSingleRow(info, control, attr, row, MovingDirection.Lower);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// コントロールプロパティをデータソースから読み取った値に設定します
        /// </summary>
        protected virtual void ReadValues()
        {
            var controls = _GetControls(this);
            foreach (var control in controls)
            {
                for (var i = 0; i < control.DataBindings.Count; i++)
                {
                    control.DataBindings[i].ReadValue();
                }
            }
        }

        /// <summary>
        /// コントロールプロパティから現在の値を読み取って、データソースに書き込みます
        /// </summary>
        protected virtual void WriteValues()
        {
            var controls = _GetControls(this);
            foreach (var control in controls)
            {
                for (var i = 0; i < control.DataBindings.Count; i++)
                {
                    control.DataBindings[i].WriteValue();
                }
            }
        }

        /// <summary>
        /// コントロールにデータをセットする
        /// </summary>
        private void _SetDataToControls()
        {
            // 修飾子を取得する
            var formType = GetType();
            var attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
            var prefix = string.Empty;
            var suffix = string.Empty;
            _GetModifier(formType, ref prefix, ref suffix);

            var type = _dataSource.GetExType();

            IDictionary<string, Control> hashtable = new Dictionary<string, Control>();
            _GetAllControls(this, hashtable);
            var pis = type.GetProperties();
            foreach (var info in pis)
            {
                var isFind = false;
                foreach (var o in attributes)
                {
                    // 個別バインディングを行う
                    var attribute = o as ControlAttribute;
                    if (attribute != null)
                    {
                        if (info.Name.ToLower() == attribute.PropertyName.ToLower()
                            && hashtable.ContainsKey(attribute.ControlName.ToLower()))
                        {
                            var control = hashtable[attribute.ControlName.ToLower()];
                            control.DataBindings.Add(attribute.ControlProperty, _dataSource, info.Name,
                                                     attribute.FormattingEnabled,
                                                     attribute.UpdateMode, attribute.NullValue,
                                                     attribute.FormatString);
                            isFind = true;
                        }

                    }
                }

                if (!isFind)
                {
                    // 自動バインディングを行う
                    if ( hashtable.ContainsKey(prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()) )
                    {
                        var control = hashtable[prefix.ToLower() + info.Name.ToLower() + suffix.ToLower()];
                        control.DataBindings.Add(DefaultProperty, _dataSource, info.Name, true,
                                                 DataSourceUpdateMode.OnValidation, null, string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 修飾子を取得する
        /// </summary>
        /// <param name="formType">フォームのType</param>
        /// <param name="prefix">プレフィックス</param>
        /// <param name="suffix">サフィックス</param>
        private static void _GetModifier(Type formType, ref string prefix, ref string suffix)
        {
            var modifiers =
                formType.GetCustomAttributes(typeof (ControlModifierAttribute), false);
            foreach (var modifier in modifiers)
            {
                var attribute = modifier as ControlModifierAttribute;
                if (attribute != null)
                {
                    prefix = attribute.Prefix;
                    suffix = attribute.Suffix;
                }
            }
        }

        /// <summary>
        /// バインディングを削除する
        /// </summary>
        private void _RemoveBindings()
        {
            var controls = _GetControls(this);
            foreach (var control in controls)
            {
                control.DataBindings.Clear();
            }
        }

        /// <summary>
        /// コントロールをすべて取得する
        /// </summary>
        /// <param name="controls">コントロール</param>
        /// <returns>コントロールの持つコントロール</returns>
        private static Control[] _GetControls(Control controls)
        {
            var retList = new ArrayList();
            foreach (Control control in controls.Controls)
            {
                retList.Add(control);
                retList.AddRange(_GetControls(control));
            }

            return ((Control[]) retList.ToArray(typeof (Control)));
        }

        /// <summary>
        /// コントロールをすべて取得する
        /// </summary>
        /// <param name="controls">コントロール</param>
        /// <param name="table">取得したコントロールをControl.Nameをキーにして保持するDictinary</param>
        private static void _GetAllControls (Control controls, IDictionary<string, Control> table)
        {
            foreach ( Control control in controls.Controls )
            {
                if (control is UserControl)
                {
                    if (String.Equals(control.Name.ToLower(), String.Empty) == false
                        && table.ContainsKey(control.Name.ToLower()) == false)
                    {
                        table.Add(control.Name.ToLower(), control);
                    }
                }
                else
                {
                    if ( String.Equals(control.Name.ToLower(), String.Empty) == false )
                    {
                        table.Add(control.Name.ToLower(), control);
                        _GetAllControls(control, table);
                    }
                }
            }
        }
        /// <summary>
        /// 一行を挿入する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性情報</param>
        /// <param name="data">挿入するデータ</param>
        /// <param name="row">挿入位置</param>
        /// <returns>削除行数</returns>
        private int _AddSingleRow(PropertyInfo info, Control control, ControlAttribute attr, object data, int row)
        {
            var propertyType = info.PropertyType;
            var factory = BindingUtilFactory.Factory;
            var util = factory.Create(propertyType);
            return (util.AddData(ref _dataSource, info, ref control, attr, data, row));
        }

        /// <summary>
        /// 一行を削除する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性情報</param>
        /// <param name="row">行</param>
        /// <returns>削除行数</returns>
        private int _DeleteSingleRow(PropertyInfo info, Control control, ControlAttribute attr, int row)
        {
            var propertyType = info.PropertyType;
            var factory = BindingUtilFactory.Factory;
            var util = factory.Create(propertyType);
            return (util.DeleteData(ref _dataSource, info, ref control, attr, row));
        }

        /// <summary>
        /// 行を移動させる
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性情報</param>
        /// <param name="row">対象行</param>
        /// <param name="direction">移動する方向</param>
        /// <returns>削除行数</returns>
        private void _MoveSingleRow(PropertyInfo info, Control control, ControlAttribute attr, int row,
                                    MovingDirection direction)
        {
            var propertyType = info.PropertyType;
            var factory = BindingUtilFactory.Factory;
            var util = factory.Create(propertyType);
            util.MoveRow(ref _dataSource, info, ref control, attr, row, direction);
        }
    }
}
