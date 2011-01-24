#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Seasar.Windows.Attr;

namespace Seasar.Windows.Seasar.Windows.Utils
{
    /// <summary>
    /// GridViewコントロールにDataTableをバインドする実装クラス
    /// </summary>
    public class BindingDataTableUtil : IBindingUtil
    {
        #region IBindingUtil Members

        /// <summary>
        /// プロパティにデータを挿入する
        /// </summary>
        /// <param name="source">データソース</param>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="data">挿入するデータ</param>
        /// <param name="row">挿入位置</param>
        /// <returns>挿入件数</returns>
        public int AddData(ref object source, PropertyInfo info, ref Control control, ControlAttribute attr, object data, int row)
        {
            int ret = 0;
            object target = info.GetValue(source, null);
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
                info.SetValue(source, new DataTable(dt.TableName), null);
                control.DataBindings.Clear();

                info.SetValue(source, newTable, null);
                control.DataBindings.Add(
                    attr.ControlProperty, source, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                    attr.NullValue, attr.FormatString);

                ret = 1;
            }
            return ret;
        }

        /// <summary>
        /// プロパティにデータを削除する
        /// </summary>
        /// <param name="source">データソース</param>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">削除位置</param>
        /// <returns>削除件数</returns>
        public int DeleteData(ref object source, PropertyInfo info, ref Control control, ControlAttribute attr, int row)
        {
            int ret = 0;
            object target = info.GetValue(source, null);
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
        /// <param name="source">データソース</param>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性情報</param>
        /// <param name="row">対象行</param>
        /// <param name="direction">移動する方向</param>
        /// <returns>削除行数</returns>
        public void MoveRow(ref object source, PropertyInfo info, ref Control control, ControlAttribute attr, int row,
                            MovingDirection direction)
        {
            int targetRow;
            if (direction == MovingDirection.Upper)
                targetRow = row - 1;
            else
                targetRow = row + 1;

            object target = info.GetValue(source, null);
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
                info.SetValue(source, new DataTable(dt.TableName), null);
                control.DataBindings.Clear();

                info.SetValue(source, newTable, null);
                control.DataBindings.Add(
                    attr.ControlProperty, source, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                    attr.NullValue, attr.FormatString);

            }
        }

        #endregion
    }
}
