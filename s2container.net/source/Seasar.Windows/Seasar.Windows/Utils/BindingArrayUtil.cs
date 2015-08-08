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
using System.Reflection;
using System.Windows.Forms;
using Seasar.Framework.Util;
using Seasar.Windows.Attr;

namespace Seasar.Windows.Seasar.Windows.Utils
{
    /// <summary>
    /// GridViewコントロールに配列をバインドする実装クラス
    /// </summary>
    public class BindingArrayUtil : IBindingUtil
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
            if (data == null)
                throw new ArgumentNullException(String.Format(SWFMessages.FSWF0001, "data"));

            var ret = 0;
            var target = PropertyUtil.GetValue(source, source.GetExType(), info.Name);
//            var target = info.GetValue(source, null);
            var list = target as IList;
            if (list != null)
            {
                if (row > list.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                if (list.Count > 0)
                {
                    var type = list[0].GetExType();
                    var obj = ArrayUtil.NewInstance(type, 1);
//                    var obj = Array.CreateInstance(type, 1);
                    PropertyUtil.SetValue(source, source.GetExType(), info.Name, info.PropertyType, obj);
//                    info.SetValue(source, obj, null);
                    control.DataBindings.Clear();

                    var obj2 = ArrayUtil.NewInstance(type, list.Count + 1);
//                    var obj2 = Array.CreateInstance(type, list.Count + 1);
                    var i = 0;
                    foreach (var o in list)
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
                    PropertyUtil.SetValue(source, source.GetExType(), info.Name, info.PropertyType, obj2);
//                    info.SetValue(source, obj2, null);
                    control.DataBindings.Add(
                        attr.ControlProperty, source, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                        attr.NullValue, attr.FormatString);
                }

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
            var target = PropertyUtil.GetValue(source, source.GetExType(), info.Name);
//            var target = info.GetValue(source, null);
            var list = target as IList;
            if (list != null)
            {
                if (row > list.Count - 1)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                if (list.Count > 0)
                {
                    var type = list[0].GetExType();
                    var obj = ArrayUtil.NewInstance(type, 1);
//                    var obj = Array.CreateInstance(type, 1);
                    PropertyUtil.SetValue(source, source.GetExType(), info.Name, info.PropertyType, obj);
//                    info.SetValue(source, obj, null);
                    control.DataBindings.Clear();

                    var obj2 = ArrayUtil.NewInstance(type, list.Count - 1);
//                    var obj2 = Array.CreateInstance(type, list.Count - 1);
                    var i = 0;
                    var pos = 0;
                    foreach (var o in list)
                    {
                        if (i != row)
                        {
                            obj2.SetValue(o, pos);
                            pos++;
                        }

                        i++;
                    }
                    PropertyUtil.SetValue(source, source.GetExType(), info.Name, info.PropertyType, obj2);
//                    info.SetValue(source, obj2, null);
                    control.DataBindings.Add(
                        attr.ControlProperty, source, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                        attr.NullValue, attr.FormatString);
                }

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

            var target = PropertyUtil.GetValue(source, source.GetExType(), info.Name);
//            var target = info.GetValue(source, null);
            var list = target as IList;
            if (list != null)
            {
                if (targetRow >= list.Count)
                    throw new ArgumentOutOfRangeException(String.Format(SWFMessages.FSWF0003, "row"));

                if (list.Count > 0)
                {
                    var type = list[0].GetExType();
                    var obj = ArrayUtil.NewInstance(type, 1);
//                    var obj = Array.CreateInstance(type, 1);
                    PropertyUtil.SetValue(source, source.GetExType(), info.Name, info.PropertyType, obj);
//                    info.SetValue(source, obj, null);
                    control.DataBindings.Clear();

                    var obj2 = ArrayUtil.NewInstance(type, list.Count);
//                    var obj2 = Array.CreateInstance(type, list.Count);
                    var i = 0;
                    foreach (var o in list)
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
                    PropertyUtil.SetValue(source, source.GetExType(), info.Name, info.PropertyType, obj2);
//                    info.SetValue(source, obj2, null);
                    control.DataBindings.Add(
                        attr.ControlProperty, source, info.Name, attr.FormattingEnabled, attr.UpdateMode,
                        attr.NullValue, attr.FormatString);
                }

            }
        }

        #endregion
    }
}
