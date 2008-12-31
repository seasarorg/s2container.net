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

using System.Reflection;
using System.Windows.Forms;
using Seasar.Windows.Attr;

namespace Seasar.Windows.Seasar.Windows.Utils
{
    /// <summary>
    /// GridViewコントロールバインディングユーティリティ
    /// </summary>
    public interface IBindingUtil
    {
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
        int AddData(ref object source, PropertyInfo info, ref Control control, ControlAttribute attr, object data, int row);

        /// <summary>
        /// プロパティにデータを削除する
        /// </summary>
        /// <param name="source">データソース</param>
        /// <param name="info">プロパティ情報</param>
        /// <param name="control">コントロール</param>
        /// <param name="attr">属性</param>
        /// <param name="row">削除位置</param>
        /// <returns>削除件数</returns>
        int DeleteData(ref object source, PropertyInfo info, ref Control control, ControlAttribute attr, int row);

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
        void MoveRow(ref object source, PropertyInfo info, ref Control control, ControlAttribute attr, int row, MovingDirection direction);
    }

    /// <summary>
    /// 移動方向
    /// </summary>
    public enum MovingDirection
    {
        Upper,
        Lower
    }
}