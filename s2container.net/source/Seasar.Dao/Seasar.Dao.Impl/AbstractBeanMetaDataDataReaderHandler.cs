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

using System.Collections;
using System.Data;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractBeanMetaDataDataReaderHandler
        : IDataReaderHandler
    {
        private readonly IBeanMetaData _beanMetaData;

        public AbstractBeanMetaDataDataReaderHandler(IBeanMetaData beanMetaData)
        {
            _beanMetaData = beanMetaData;
        }

        public IBeanMetaData BeanMetaData
        {
            get { return _beanMetaData; }
        }

        /// <summary>
        /// Columnのメタデータを作成する
        /// </summary>
        /// <param name="columnNames">カラム名のリスト</param>
        /// <returns>Columnのメタデータの配列</returns>
        protected virtual IColumnMetaData[] CreateColumnMetaData(IList columnNames)
        {
#if NET_1_1
            IDictionary names = null;
            ArrayList columnMetaDataList = new ArrayList();
#else
            System.Collections.Generic.IDictionary<string, string> names = null;
            System.Collections.Generic.List<IColumnMetaData> columnMetaDataList =
                new System.Collections.Generic.List<IColumnMetaData>();
#endif

            for (int i = 0; i < _beanMetaData.PropertyTypeSize; ++i)
            {
                IPropertyType pt = _beanMetaData.GetPropertyType(i);
                string columnName;

                columnName = FindColumnName(columnNames, pt.ColumnName);

                if (columnName != null)
                {
                    columnMetaDataList.Add(new ColumnMetaDataImpl(pt, columnName));
                    continue;
                }

                columnName = FindColumnName(columnNames, pt.PropertyName);

                if (columnName != null)
                {
                    columnMetaDataList.Add(new ColumnMetaDataImpl(pt, columnName));
                    continue;
                }

                if (!pt.IsPersistent)
                {
                    if (names == null)
                    {
#if NET_1_1
                        names = new Hashtable();
#else
                        names = new System.Collections.Generic.Dictionary<string, string>();
#endif
                        foreach (string name in columnNames)
                        {
                            names[name.Replace("_", string.Empty).ToUpper()] = name;
                        }
                    }
#if NET_1_1
                    if (names.Contains(pt.ColumnName.ToUpper()))
                    {
                        columnMetaDataList.Add(new ColumnMetaDataImpl(
                            pt, (string) names[pt.ColumnName.ToUpper()]));
                    }
#else
                    if (names.ContainsKey(pt.ColumnName.ToUpper()))
                    {
                        columnMetaDataList.Add(new ColumnMetaDataImpl(
                            pt, names[pt.ColumnName.ToUpper()]));
                    }
#endif

                }
            }

#if NET_1_1
            return (IColumnMetaData[]) columnMetaDataList.ToArray(typeof(IColumnMetaData));
#else
            return columnMetaDataList.ToArray();
#endif

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

        /// <summary>
        /// 1行分のオブジェクトを作成する
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columns">Columnのメタデータ</param>
        /// <returns>1行分のEntity型のオブジェクト</returns>
        protected virtual object CreateRow(IDataReader reader, IColumnMetaData[] columns)
        {
            object row = ClassUtil.NewInstance(_beanMetaData.BeanType);

            foreach (IColumnMetaData column in columns)
            {
                object value = column.ValueType.GetValue(reader, column.ColumnName);
                column.PropertyInfo.SetValue(row, value, null);
            }

            return row;
        }

        protected virtual object CreateRelationRow(IDataReader reader, IRelationPropertyType rpt,
            IList columnNames, Hashtable relKeyValues)
        {
            object row = null;
            IBeanMetaData bmd = rpt.BeanMetaData;
            for (int i = 0; i < rpt.KeySize; ++i)
            {
                string columnName = rpt.GetMyKey(i);
                if (columnNames.Contains(columnName))
                {
                    if (row == null) row = CreateRelationRow(rpt);
                    if (relKeyValues != null && relKeyValues.ContainsKey(columnName))
                    {
                        object value = relKeyValues[columnName];
                        IPropertyType pt = bmd.GetPropertyTypeByColumnName(rpt.GetYourKey(i));
                        PropertyInfo pi = pt.PropertyInfo;
                        if (value != null) pi.SetValue(row, value, null);
                    }
                }
                continue;
            }
            for (int i = 0; i < bmd.PropertyTypeSize; ++i)
            {
                IPropertyType pt = bmd.GetPropertyType(i);
                string columnName = pt.ColumnName + "_" + rpt.RelationNo;
                if (!columnNames.Contains(columnName)) continue;
                if (row == null) row = CreateRelationRow(rpt);
                object value = null;
                PropertyInfo pi = pt.PropertyInfo;
                if (relKeyValues != null && relKeyValues.ContainsKey(columnName))
                {
                    value = relKeyValues[columnName];
                }
                else
                {
                    IValueType valueType = pt.ValueType;
                    value = valueType.GetValue(reader, columnName);
                }
                if (value != null) pi.SetValue(row, value, null);
            }
            return row;
        }

        protected virtual object CreateRelationRow(IRelationPropertyType rpt)
        {
            return ClassUtil.NewInstance(rpt.PropertyInfo.PropertyType);
        }

        protected virtual IList CreateColumnNames(DataTable dt)
        {
            IList columnNames = new CaseInsentiveSet();
            foreach (DataRow row in dt.Rows)
            {
                string columnName = (string) row["ColumnName"];
                columnNames.Add(columnName);
            }
            return columnNames;
        }

        #region IDataReaderHandler メンバ

        public virtual object Handle(System.Data.IDataReader dataReader)
        {
            return null;
        }

        #endregion
    }
}
