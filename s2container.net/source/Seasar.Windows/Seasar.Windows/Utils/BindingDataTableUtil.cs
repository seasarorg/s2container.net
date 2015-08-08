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
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using Seasar.Framework.Util;
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
            var ret = 0;
            var target = info.GetValue(source, null);
            var dt = target as DataTable;
            if (dt != null)
            {
                if (row > dt.Rows.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                var newTable = new DataTable(dt.TableName);
                // 列設定
                var columns = dt.Columns;
                foreach (DataColumn column in columns)
                {
                    var newColumn = new DataColumn();

                    var type = newColumn.GetExType();
                    var pis = type.GetProperties();
                    foreach (var pi in pis)
                    {
                        var type2 = column.GetExType();
                        var pis2 = type2.GetProperties();
                        foreach (var pi2 in pis2)
                        {
                            if (pi.Name == pi2.Name)
                            {
                                var mis = pi.GetAccessors();
                                var isSetter = false;
                                foreach (var mi in mis)
                                {
                                    if (mi.Name.Substring(0, 3).ToLower() == "set")
                                        isSetter = true;
                                }
                                if (isSetter)
                                {
                                    var obj = pi2.GetValue(column, null);
                                    pi.SetValue(newColumn, obj, null);
                                }
                            }
                        }
                    }

                    newTable.Columns.Add(newColumn);
                }

                // 行追加
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var addRow = newTable.NewRow();
                    var rowData = dt.Rows[i];
                    var newRow = data as DataRow;
                    if (newRow != null)
                    {
                        for (var j = 0; j < dt.Columns.Count; j++)
                        {
                            addRow[j] = newRow[j];
                        }
                    }

                    if (i == row)
                        newTable.Rows.Add(addRow);

                    addRow = newTable.NewRow();
                    for (var j = 0; j < dt.Columns.Count; j++)
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
            var ret = 0;
            var target = info.GetValue(source, null);
            var dt = target as DataTable;
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

            var target = info.GetValue(source, null);
            var dt = target as DataTable;
            if (dt != null)
            {
                if (targetRow >= dt.Rows.Count)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                var newTable = new DataTable(dt.TableName);
                // 列設定
                var columns = dt.Columns;
                foreach (DataColumn column in columns)
                {
                    var newColumn = new DataColumn();

                    var type = newColumn.GetExType();
                    var pis = type.GetProperties();
                    foreach (var pi in pis)
                    {
                        var type2 = column.GetExType();
                        var pis2 = type2.GetProperties();
                        foreach (var pi2 in pis2)
                        {
                            if (pi.Name == pi2.Name)
                            {
                                var mis = pi.GetAccessors();
                                var isSetter = false;
                                foreach (var mi in mis)
                                {
                                    if (mi.Name.Substring(0, 3).ToLower() == "set")
                                        isSetter = true;
                                }
                                if (isSetter)
                                {
                                    var obj = pi2.GetValue(column, null);
                                    pi.SetValue(newColumn, obj, null);
                                }
                            }
                        }
                    }

                    newTable.Columns.Add(newColumn);
                }

                // 行移動
                for (var i = 0; i < dt.Rows.Count; i++)
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

                    var addRow = newTable.NewRow();
                    for (var j = 0; j < dt.Columns.Count; j++)
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
