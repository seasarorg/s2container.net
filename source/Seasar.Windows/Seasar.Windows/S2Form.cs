#region Copyright

/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Seasar.Windows.Attr;

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
        /// バインディング用デフォルトプロパティ
        /// </summary>
        private string _defaultProperty = "Text";

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
        public string DefaultProperty
        {
            get { return _defaultProperty; }
            set { _defaultProperty = value; }
        }

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
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001,"propertyName"));
            if (row < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002,"row"));

            int ret = 0;
            // 修飾子を取得する
            Type formType = this.GetType();
            string prefix = "";
            string suffix = "";
            _GetModifier(formType, ref prefix, ref suffix);

            Type type = _dataSource.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo info in pis)
            {
                if (info.Name == propertyName)
                {
                    bool isFind = false;
                    Control[] controls = _GetControls(this);
                    foreach (Control control in controls)
                    {
                        object[] attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
                        foreach (object o in attributes)
                        {
                            // 個別バインディングを行う
                            ControlAttribute attribute = o as ControlAttribute;
                            if (attribute != null)
                            {
                                if (info.Name == attribute.PropertyName
                                    && control.Name == attribute.ControlName)
                                {
                                    ret += _AddSingleRow(info, control, attribute, data, row);
                                    isFind = true;
                                    break;
                                }
                            }
                        }
                        if (isFind) break;

                        // 自動バインディングを行う
                        if (control.Name == (prefix + info.Name + suffix))
                        {
                            ControlAttribute attr = new ControlAttribute(control.Name, _defaultProperty, propertyName);
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
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001,"propertyName"));
            if (row < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002,"row"));

            int ret = 0;
            // 修飾子を取得する
            Type formType = this.GetType();
            string prefix = "";
            string suffix = "";
            _GetModifier(formType, ref prefix, ref suffix);

            Type type = _dataSource.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo info in pis)
            {
                if (info.Name == propertyName)
                {
                    bool isFind = false;
                    Control[] controls = _GetControls(this);
                    foreach (Control control in controls)
                    {
                        object[] attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
                        foreach (object o in attributes)
                        {
                            // 個別バインディングを行う
                            ControlAttribute attribute = o as ControlAttribute;
                            if (attribute != null)
                            {
                                if (info.Name == attribute.PropertyName
                                    && control.Name == attribute.ControlName)
                                {
                                    ret += _DeleteSingleRow(info, control, attribute, row);
                                    isFind = true;
                                    break;
                                }
                            }
                        }
                        if (isFind) break;

                        // 自動バインディングを行う
                        if (control.Name == (prefix + info.Name + suffix))
                        {
                            ControlAttribute attr = new ControlAttribute(control.Name, _defaultProperty, propertyName);
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
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001,"propertyName"));
            if (startRow < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002,"startRow"));
            if (endRow < 0)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002,"endRow"));
            if (startRow > endRow)
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0004,"endRow"));

            // 修飾子を取得する
            Type formType = this.GetType();
            string prefix = "";
            string suffix = "";
            _GetModifier(formType, ref prefix, ref suffix);

            int ret = 0;
            Type type = _dataSource.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo info in pis)
            {
                if (info.Name == propertyName)
                {
                    bool isFind = false;
                    Control[] controls = _GetControls(this);
                    foreach (Control control in controls)
                    {
                        object[] attributes = formType.GetCustomAttributes(typeof (ControlAttribute), false);
                        foreach (object o in attributes)
                        {
                            // 個別バインディングを行う
                            ControlAttribute attribute = o as ControlAttribute;
                            if (attribute != null)
                            {
                                if (info.Name == attribute.PropertyName
                                    && control.Name == attribute.ControlName)
                                {
                                    for (int i = endRow; i >= startRow; i--)
                                        ret += _DeleteSingleRow(info, control, attribute, i);

                                    isFind = true;
                                    break;
                                }
                            }
                        }
                        if (isFind) break;

                        // 自動バインディングを行う
                        if (control.Name == (prefix + info.Name + suffix))
                        {
                            for (int i = endRow; i >= startRow; i--)
                            {
                                ControlAttribute attr = new ControlAttribute(control.Name, _defaultProperty, propertyName);
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
            Type formType = this.GetType();
            string prefix = "";
            string suffix = "";
            _GetModifier(formType, ref prefix, ref suffix);

            Type type = _dataSource.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo info in pis)
            {
                if (info.Name == propertyName)
                {
                    bool isFind = false;
                    Control[] controls = _GetControls(this);
                    foreach (Control control in controls)
                    {
                        object[] attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
                        foreach (object o in attributes)
                        {
                            // 個別バインディングを行う
                            ControlAttribute attribute = o as ControlAttribute;
                            if (attribute != null)
                            {
                                if (info.Name == attribute.PropertyName
                                    && control.Name == attribute.ControlName)
                                {
                                    _MoveSingleRow(info, control, attribute, row, MovingDirection.Upper);
                                    isFind = true;
                                    break;
                                }
                            }
                        }
                        if (isFind) break;

                        // 自動バインディングを行う
                        if (control.Name == (prefix + info.Name + suffix))
                        {
                            ControlAttribute attr = new ControlAttribute(control.Name, _defaultProperty, propertyName);
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
                throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0002,"row"));

            // 修飾子を取得する
            Type formType = this.GetType();
            string prefix = "";
            string suffix = "";
            _GetModifier(formType, ref prefix, ref suffix);

            Type type = _dataSource.GetType();
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo info in pis)
            {
                if (info.Name == propertyName)
                {
                    bool isFind = false;
                    Control[] controls = _GetControls(this);
                    foreach (Control control in controls)
                    {
                        object[] attributes = formType.GetCustomAttributes(typeof(ControlAttribute), false);
                        foreach (object o in attributes)
                        {
                            // 個別バインディングを行う
                            ControlAttribute attribute = o as ControlAttribute;
                            if (attribute != null)
                            {
                                if (info.Name == attribute.PropertyName
                                    && control.Name == attribute.ControlName)
                                {
                                    _MoveSingleRow(info, control, attribute, row, MovingDirection.Lower);
                                    isFind = true;
                                    break;
                                }
                            }
                        }
                        if (isFind) break;

                        // 自動バインディングを行う
                        if (control.Name == (prefix + info.Name + suffix))
                        {
                            ControlAttribute attr = new ControlAttribute(control.Name, _defaultProperty, propertyName);
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
            Control[] controls = _GetControls(this);
            foreach (Control control in controls)
            {
                for (int i = 0; i < control.DataBindings.Count; i++)
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
            Control[] controls = _GetControls(this);
            foreach (Control control in controls)
            {
                for (int i = 0; i < control.DataBindings.Count; i++)
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
            Type formType = this.GetType();
            string prefix = "";
            string suffix = "";
            _GetModifier(formType, ref prefix, ref suffix);

            Type type = _dataSource.GetType();

            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo info in pis)
            {
                bool isFind = false;
                Control[] controls = _GetControls(this);
                foreach (Control control in controls)
                {
                    object[] attributes = formType.GetCustomAttributes(typeof (ControlAttribute), false);
                    foreach (object o in attributes)
                    {
                        // 個別バインディングを行う
                        ControlAttribute attribute = o as ControlAttribute;
                        if ( attribute != null)
                        {
                            if (info.Name == attribute.PropertyName
                                && control.Name == attribute.ControlName)
                            {
                                control.DataBindings.Add(attribute.ControlProperty, _dataSource, info.Name, attribute.FormattingEnabled,
                                                         attribute.UpdateMode, attribute.NullValue, attribute.FormatString);
                                isFind = true;
                                break;
                            }
                        }
                    }
                    if (isFind) break;

                    // 自動バインディングを行う
                    if (control.Name == (prefix + info.Name + suffix))
                    {
                        control.DataBindings.Add(_defaultProperty, _dataSource, info.Name, true,
                                                 DataSourceUpdateMode.OnValidation, null, "");
                        break;
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
            object[] modifiers =
                formType.GetCustomAttributes(typeof (ControlModifierAttribute), false);
            foreach (object modifier in modifiers)
            {
                ControlModifierAttribute attribute = modifier as ControlModifierAttribute;
                if ( attribute != null)
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
            Control[] controls = _GetControls(this);
            foreach (Control control in controls)
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
            ArrayList retList = new ArrayList();
            foreach (Control control in controls.Controls)
            {
                retList.Add(control);
                retList.AddRange(_GetControls(control));
            }

            return ((Control[]) retList.ToArray(typeof (Control)));
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
            Type propertyType = info.PropertyType;
            if (propertyType == typeof (IList))
                return (_AddDataToList(info, control, attr, data, row));
            else if (propertyType.Name == typeof (IList<>).Name)
                return (_AddDataToGenericList(info, control, attr, data, row));
            else if (propertyType.IsArray)
                return (_AddToArray(info, control, attr, data, row));
            else if (propertyType == typeof (DataTable))
                return (_AddDataToDataTable(info, control, attr, data, row));
            else
                throw new InvalidCastException(attr.PropertyName);
        }

        /// <summary>
        /// IList型プロパティにデータを挿入する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="data">挿入するデータ</param>
        /// <param name="row">挿入位置</param>
        /// <returns>挿入件数</returns>
        private int _AddDataToList(PropertyInfo info, Control control, ControlAttribute attr, object data, int row)
        {
            if (data == null)
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001,"data"));

            int ret = 0;
            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (row > list.Count)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003,"row"));

                list.Insert(row, data);
                IList list2 = new ArrayList();
                foreach (object o in list)
                    list2.Add(o);

                info.SetValue(_dataSource, new ArrayList(), null);
                control.DataBindings.Clear();

                info.SetValue(_dataSource, list2, null);
                control.DataBindings.Add(
                    attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                    attr.NullValue, attr.FormatString);

                ret = 1;
            }
            return ret;
        }

        /// <summary>
        /// Generics.IList型プロパティにデータを挿入する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="data">挿入するデータ</param>
        /// <param name="row">挿入位置</param>
        /// <returns>挿入件数</returns>
        private int _AddDataToGenericList(PropertyInfo info, Control control, ControlAttribute attr, object data,
                                          int row)
        {
            if (data == null)
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001, "data"));

            int ret = 0;
            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (row > list.Count)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                list.Insert(row, data);

                Type genericType = target.GetType();
                object obj = Activator.CreateInstance(genericType);
                object obj2 = Activator.CreateInstance(genericType);

                info.SetValue(_dataSource, obj, null);
                control.DataBindings.Clear();

                IList list2 = (IList) obj2;
                foreach (object o in list)
                    list2.Add(o);

                info.SetValue(_dataSource, obj2, null);
                control.DataBindings.Add(
                    attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode, attr.NullValue,
                    attr.FormatString);

                ret = 1;
            }
            return ret;
        }

        /// <summary>
        /// 配列プロパティにデータを挿入する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="data">挿入データ</param>
        /// <param name="row">削除位置</param>
        /// <returns>削除件数</returns>
        private int _AddToArray(PropertyInfo info, Control control, ControlAttribute attr, object data, int row)
        {
            if (data == null)
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001, "data"));

            int ret = 0;
            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (row > list.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                if (list.Count > 0)
                {
                    Type type = ((object) list[0]).GetType();
                    Array obj = Array.CreateInstance(type, 1);
                    info.SetValue(_dataSource, obj, null);
                    control.DataBindings.Clear();

                    Array obj2 = Array.CreateInstance(type, list.Count + 1);
                    int i = 0;
                    foreach (object o in list)
                    {
                        if (i != row)
                        {
                            obj2.SetValue(o, i);
                        }
                        else
                        {
                            obj2.SetValue(data, i);
                            i++;
                            obj2.SetValue(o, i);
                        }
                        i++;
                    }
                    info.SetValue(_dataSource, obj2, null);
                    control.DataBindings.Add(
                        attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                        attr.NullValue, attr.FormatString);
                }

                ret = 1;
            }

            return ret;
        }

        /// <summary>
        /// DataTable型プロパティにデータを挿入する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="data">挿入するデータ</param>
        /// <param name="row">挿入位置</param>
        /// <returns>挿入件数</returns>
        private int _AddDataToDataTable(PropertyInfo info, Control control, ControlAttribute attr, object data,
                                        int row)
        {
            int ret = 0;
            object target = info.GetValue(_dataSource, null);
            DataTable dt = target as DataTable;
            if (dt != null)
            {
                if (row > dt.Rows.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                DataTable newTable = new DataTable(dt.TableName);
                // 列設定
                DataColumnCollection columns = dt.Columns;
                foreach (DataColumn column in columns)
                {
                    DataColumn newColumn = new DataColumn();

                    Type type = newColumn.GetType();
                    PropertyInfo[] pis = type.GetProperties();
                    foreach (PropertyInfo pi in pis)
                    {
                        Type type2 = column.GetType();
                        PropertyInfo[] pis2 = type2.GetProperties();
                        foreach (PropertyInfo pi2 in pis2)
                        {
                            if (pi.Name == pi2.Name)
                            {
                                MethodInfo[] mis = pi.GetAccessors();
                                bool isSetter = false;
                                foreach (MethodInfo mi in mis)
                                {
                                    if (mi.Name.Substring(0, 3).ToLower() == "set")
                                        isSetter = true;
                                }
                                if (isSetter)
                                {
                                    object obj = pi2.GetValue(column, null);
                                    pi.SetValue(newColumn, obj, null);
                                }
                            }
                        }
                    }

                    newTable.Columns.Add(newColumn);
                }

                // 行追加
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow addRow = newTable.NewRow();
                    DataRow rowData = dt.Rows[i];
                    if (data != null)
                    {
                        DataRow newRow = data as DataRow;
                        if (newRow != null)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                addRow[j] = newRow[j];
                            }
                        }
                    }

                    if (i == row)
                        newTable.Rows.Add(addRow);

                    addRow = newTable.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        addRow[j] = rowData[j];
                    }
                    newTable.Rows.Add(addRow);
                }
                info.SetValue(_dataSource, new DataTable(dt.TableName), null);
                control.DataBindings.Clear();

                info.SetValue(_dataSource, newTable, null);
                control.DataBindings.Add(
                    attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                    attr.NullValue, attr.FormatString);

                ret = 1;
            }
            return ret;
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
            Type propertyType = info.PropertyType;
            if (propertyType == typeof (IList))
                return (_DeleteFromList(info, control, attr, row));
            else if (propertyType.Name == typeof (IList<>).Name)
                return (_DeleteFromGenericList(info, control, attr, row));
            else if (propertyType.IsArray)
                return (_DeleteFromArray(info, control, attr, row));
            else if (propertyType == typeof (DataTable))
                return (_DeleteFromDataTable(info, row));
            else
                throw new InvalidCastException(attr.PropertyName);
        }

        /// <summary>
        /// IList型プロパティにデータを削除する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">削除位置</param>
        /// <returns>削除件数</returns>
        private int _DeleteFromList(PropertyInfo info, Control control, ControlAttribute attr, int row)
        {
            int ret = 0;
            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (row >= list.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                list.RemoveAt(row);
                IList list2 = new ArrayList();
                foreach (object o in list)
                    list2.Add(o);

                info.SetValue(_dataSource, new ArrayList(), null);
                control.DataBindings.Clear();

                info.SetValue(_dataSource, list2, null);
                control.DataBindings.Add(
                    attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                    attr.NullValue, attr.FormatString);

                ret = 1;
            }
            return ret;
        }

        /// <summary>
        /// Generic.IList型プロパティにデータを削除する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">削除位置</param>
        /// <returns>削除件数</returns>
        private int _DeleteFromGenericList(PropertyInfo info, Control control, ControlAttribute attr, int row)
        {
            int ret = 0;
            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (row > list.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                list.RemoveAt(row);

                Type genericType = target.GetType();

                object obj = Activator.CreateInstance(genericType);
                object obj2 = Activator.CreateInstance(genericType);

                IList list2 = (IList) obj2;
                foreach (object o in list)
                    list2.Add(o);
                info.SetValue(_dataSource, obj, null);
                control.DataBindings.Clear();


                info.SetValue(_dataSource, obj2, null);
                control.DataBindings.Add(
                    attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                    attr.NullValue, attr.FormatString);

                ret = 1;
            }
            return ret;
        }

        /// <summary>
        /// 配列プロパティにデータを削除する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">削除位置</param>
        /// <returns>削除件数</returns>
        private int _DeleteFromArray(PropertyInfo info, Control control, ControlAttribute attr, int row)
        {
            int ret = 0;
            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (row > list.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                if (list.Count > 0)
                {
                    Type type = ((object) list[0]).GetType();
                    Array obj = Array.CreateInstance(type, 1);
                    info.SetValue(_dataSource, obj, null);
                    control.DataBindings.Clear();

                    Array obj2 = Array.CreateInstance(type, list.Count - 1);
                    int i = 0;
                    int pos = 0;
                    foreach (object o in list)
                    {
                        if (i != row)
                        {
                            obj2.SetValue(o, pos);
                            pos++;
                        }

                        i++;
                    }
                    info.SetValue(_dataSource, obj2, null);
                    control.DataBindings.Add(
                        attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                        attr.NullValue, attr.FormatString);
                }

                ret = 1;
            }

            return ret;
        }

        /// <summary>
        /// DataTable型プロパティにデータを削除する
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="row">削除位置</param>
        /// <returns>削除件数</returns>
        private int _DeleteFromDataTable(PropertyInfo info, int row)
        {
            int ret = 0;
            object target = info.GetValue(_dataSource, null);
            DataTable dt = target as DataTable;
            if (dt != null)
            {
                if (row > dt.Rows.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                dt.Rows.RemoveAt(row);
                ret = 1;
            }
            return ret;
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
        private void _MoveSingleRow(PropertyInfo info, Control control, ControlAttribute attr, int row, MovingDirection direction)
        {
            Type propertyType = info.PropertyType;
            if (propertyType == typeof(IList))
                _MoveRowOfList(info, control, attr, row, direction);
            else if (propertyType.Name == typeof(IList<>).Name)
                _MoveRowOfGenericList(info, control, attr, row, direction);
            else if (propertyType.IsArray)
                _MoveRowOfArray(info, control, attr, row, direction);
            else if (propertyType == typeof(DataTable))
                _MoveRowOfDataTable(info, control, attr, row, direction);
            else
                throw new InvalidCastException(attr.PropertyName);
        }

        /// <summary>
        /// IList型プロパティの行を一つ移動させる
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">対象コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">対象行</param>
        /// <param name="direction">行の移動方向</param>
        private void _MoveRowOfList(PropertyInfo info, Control control, ControlAttribute attr, int row, MovingDirection direction)
        {
            int targetRow;
            if (direction == MovingDirection.Upper)
                targetRow = row - 1;
            else
                targetRow = row + 1;

            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (targetRow >= list.Count)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                IList list2 = new ArrayList();
                object src = list[row];
                int pos = 0;
                foreach (object o in list)
                {
                    if (pos == targetRow)
                        list2.Add(src);
                    else if (pos != row && pos != targetRow)
                        list2.Add(o);
                    else if (pos == row && direction == MovingDirection.Upper)
                        list2.Add(list[row - 1]);
                    else if (pos == row && direction == MovingDirection.Lower)
                        list2.Add(list[row + 1]);

                    pos++;
                }

                info.SetValue(_dataSource, new ArrayList(), null);
                control.DataBindings.Clear();

                info.SetValue(_dataSource, list2, null);
                control.DataBindings.Add(
                    attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                    attr.NullValue, attr.FormatString);

            }
        }

        /// <summary>
        /// Generic.IList型プロパティの行を一つ移動させる
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">対象コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">対象行</param>
        /// <param name="direction">行の移動方向</param>
        private void _MoveRowOfGenericList(PropertyInfo info, Control control, ControlAttribute attr, int row, MovingDirection direction)
        {
            int targetRow;
            if (direction == MovingDirection.Upper)
                targetRow = row - 1;
            else
                targetRow = row + 1;

            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (targetRow >= list.Count)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                Type genericType = target.GetType();
                object obj = Activator.CreateInstance(genericType);
                object obj2 = Activator.CreateInstance(genericType);

                info.SetValue(_dataSource, obj, null);
                control.DataBindings.Clear();

                object src = list[row];
                IList list2 = (IList)obj2;
                int pos = 0;
                foreach (object o in list)
                {
                    if (pos == targetRow)
                        list2.Add(src);
                    else if (pos != row && pos != targetRow)
                        list2.Add(o);
                    else if (pos == row && direction == MovingDirection.Upper)
                        list2.Add(list[row - 1]);
                    else if (pos == row && direction == MovingDirection.Lower)
                        list2.Add(list[row + 1]);

                    pos++;
                }

                info.SetValue(_dataSource, obj2, null);
                control.DataBindings.Add(
                    attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode, attr.NullValue,
                    attr.FormatString);

            }
        }

        /// <summary>
        /// 配列型プロパティの行を一つ移動させる
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">対象コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">対象行</param>
        /// <param name="direction">行の移動方向</param>
        private void _MoveRowOfArray(PropertyInfo info, Control control, ControlAttribute attr, int row, MovingDirection direction)
        {
            int targetRow;
            if (direction == MovingDirection.Upper)
                targetRow = row - 1;
            else
                targetRow = row + 1;

            object target = info.GetValue(_dataSource, null);
            IList list = target as IList;
            if (list != null)
            {
                if (targetRow >= list.Count)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                if (list.Count > 0)
                {
                    Type type = ((object)list[0]).GetType();
                    Array obj = Array.CreateInstance(type, 1);
                    info.SetValue(_dataSource, obj, null);
                    control.DataBindings.Clear();

                    Array obj2 = Array.CreateInstance(type, list.Count);
                    int i = 0;
                    foreach (object o in list)
                    {
                        if (i == targetRow)
                            obj2.SetValue(list[row], i);
                        else if (i != row && i != targetRow)
                            obj2.SetValue(o, i);
                        else if (i == row && direction == MovingDirection.Upper)
                            obj2.SetValue(list[row - 1], i);
                        else if (i == row && direction == MovingDirection.Lower)
                            obj2.SetValue(list[row + 1], i);

                        i++;
                    }
                    info.SetValue(_dataSource, obj2, null);
                    control.DataBindings.Add(
                        attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                        attr.NullValue, attr.FormatString);
                }

            }
        }

        /// <summary>
        /// DataTable型プロパティの行を一つ移動させる
        /// </summary>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">対象コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">対象行</param>
        /// <param name="direction">行の移動方向</param>
        private void _MoveRowOfDataTable(PropertyInfo info, Control control, ControlAttribute attr, int row, MovingDirection direction)
        {
            int targetRow;
            if (direction == MovingDirection.Upper)
                targetRow = row - 1;
            else
                targetRow = row + 1;

            object target = info.GetValue(_dataSource, null);
            DataTable dt = target as DataTable;
            if (dt != null)
            {
                if (targetRow >= dt.Rows.Count)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                DataTable newTable = new DataTable(dt.TableName);
                // 列設定
                DataColumnCollection columns = dt.Columns;
                foreach (DataColumn column in columns)
                {
                    DataColumn newColumn = new DataColumn();

                    Type type = newColumn.GetType();
                    PropertyInfo[] pis = type.GetProperties();
                    foreach (PropertyInfo pi in pis)
                    {
                        Type type2 = column.GetType();
                        PropertyInfo[] pis2 = type2.GetProperties();
                        foreach (PropertyInfo pi2 in pis2)
                        {
                            if (pi.Name == pi2.Name)
                            {
                                MethodInfo[] mis = pi.GetAccessors();
                                bool isSetter = false;
                                foreach (MethodInfo mi in mis)
                                {
                                    if (mi.Name.Substring(0, 3).ToLower() == "set")
                                        isSetter = true;
                                }
                                if (isSetter)
                                {
                                    object obj = pi2.GetValue(column, null);
                                    pi.SetValue(newColumn, obj, null);
                                }
                            }
                        }
                    }

                    newTable.Columns.Add(newColumn);
                }

                // 行移動
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow rowData;
                    if (i == targetRow)
                        rowData = dt.Rows[row];
                    else if (i == row && direction == MovingDirection.Upper)
                        rowData = dt.Rows[row - 1];
                    else if (i == row && direction == MovingDirection.Lower)
                        rowData = dt.Rows[row + 1];
                    else
                        rowData = dt.Rows[i];

                    DataRow addRow = newTable.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        addRow[j] = rowData[j];
                    }
                    newTable.Rows.Add(addRow);
                }
                info.SetValue(_dataSource, new DataTable(dt.TableName), null);
                control.DataBindings.Clear();

                info.SetValue(_dataSource, newTable, null);
                control.DataBindings.Add(
                    attr.ControlProperty, _dataSource, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                    attr.NullValue, attr.FormatString);

            }
        }

    }

    /// <summary>
    /// 移動方向
    /// </summary>
    internal enum MovingDirection
    {
        Upper,
        Lower
    }
}