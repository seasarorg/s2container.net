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
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class RowCreatorImpl : IRowCreator
    {
        /// <summary>
        /// 1行分のオブジェクトを作成する
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columns">Columnのメタデータ</param>
        /// <param name="beanType">オブジェクトの型</param>
        /// <returns>1行分のEntity型のオブジェクト</returns>
        public virtual object CreateRow(IDataReader reader, IColumnMetaData[] columns, Type beanType) {
            object row = NewBean(beanType);
            foreach (IColumnMetaData column in columns) {
                object value = column.ValueType.GetValue(reader, column.ColumnName);
                column.PropertyInfo.SetValue(row, value, null);
            }
            return row;
        }

        protected virtual object NewBean(Type beanType) {
            return ClassUtil.NewInstance(beanType);
        }

        /// <summary>
        /// Columnのメタデータを作成する
        /// </summary>
        /// <param name="columnNames">カラム名のリスト</param>
        /// <param name="beanMetaData">メタ情報</param>
        /// <returns>Columnのメタデータの配列</returns>
        public virtual IColumnMetaData[] CreateColumnMetaData(IList columnNames, IBeanMetaData beanMetaData)
        {
            System.Collections.Generic.IDictionary<string, string> names = null;
            System.Collections.Generic.List<IColumnMetaData> columnMetaDataList =
                new System.Collections.Generic.List<IColumnMetaData>();

            for (int i = 0; i < beanMetaData.PropertyTypeSize; ++i)
            {
                IPropertyType pt = beanMetaData.GetPropertyType(i);

                // [DAONET-56] (2007/08/29)
                // Performance向上のためにSetterの無いPropertyは対象にしないようする。
                if (!IsTargetProperty(pt)) {
                    continue;
                }

                string columnName;

                columnName = FindColumnName(columnNames, pt.ColumnName);

                if (columnName != null)
                {
                    columnMetaDataList.Add(NewColumnMetaDataImpl(pt, columnName));
                    continue;
                }

                columnName = FindColumnName(columnNames, pt.PropertyName);

                if (columnName != null)
                {
                    columnMetaDataList.Add(NewColumnMetaDataImpl(pt, columnName));
                    continue;
                }

                if (!pt.IsPersistent)
                {
                    if (names == null)
                    {
                        names = new System.Collections.Generic.Dictionary<string, string>();
                        foreach (string name in columnNames)
                        {
                            names[name.Replace("_", string.Empty).ToUpper()] = name;
                        }
                    }
                    if (names.ContainsKey(pt.ColumnName.ToUpper()))
                    {
                        columnMetaDataList.Add(NewColumnMetaDataImpl(
                            pt, names[pt.ColumnName.ToUpper()]));
                    }

                }
            }
            return columnMetaDataList.ToArray();
        }

        /// <summary>
        /// カラム名のリストから大文字小文字を区別せずに一致するカラム名を探す
        /// </summary>
        /// <param name="columnNames">検索対象のカラム名のリスト</param>
        /// <param name="columnName">探し出すカラム名</param>
        /// <returns>見つかったカラム名（カラム名のリストから取得したカラム名）</returns>
        protected virtual string FindColumnName(IList columnNames, string columnName)
        {
            foreach (string realColumnName in columnNames)
            {
                if (string.Compare(realColumnName, columnName, true) == 0)
                {
                    return realColumnName;
                }
            }
            return null;
        }

        protected virtual bool IsTargetProperty(IPropertyType pt) {
            return pt.PropertyInfo.CanWrite;
        }

        protected virtual ColumnMetaDataImpl NewColumnMetaDataImpl(IPropertyType propertyType, string columnName) {
            return new ColumnMetaDataImpl(propertyType, columnName);
        }
    }
}
