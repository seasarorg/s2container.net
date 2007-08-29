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

using System.Collections.Generic;
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

        protected IRowCreator _rowCreator;// [DAONET-56] (2007/08/29)

        protected IRelationRowCreator _relationRowCreator;// [DAONET-56] (2007/08/29)

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="beanMetaData">Bean meta data. (NotNull)</param>
        /// <param name="rowCreator">Row creator. (NotNull)</param>
        public AbstractBeanMetaDataDataReaderHandler(IBeanMetaData beanMetaData, IRowCreator rowCreator, IRelationRowCreator relationRowCreator)
        {
            _beanMetaData = beanMetaData;
            _rowCreator = rowCreator;
            _relationRowCreator = relationRowCreator;
        }

        public IBeanMetaData BeanMetaData
        {
            get { return _beanMetaData; }
        }

        /// <summary>
        /// Columnのメタデータを作成する
        /// </summary>
        /// <param name="columnNames">カラム名のリスト (NotNull)</param>
        /// <returns>Columnのメタデータの配列 (NotNull)</returns>
        protected virtual IColumnMetaData[] CreateColumnMetaData(System.Collections.IList columnNames)
        {
            return _rowCreator.CreateColumnMetaData(columnNames, _beanMetaData);
        }

        /// <summary>
        /// 1行分のオブジェクトを作成する
        /// </summary>
        /// <param name="reader">IDataReader (NotNull)</param>
        /// <param name="columns">Columnのメタデータ (NotNull)</param>
        /// <returns>1行分のEntity型のオブジェクト (NotNull)</returns>
        protected virtual object CreateRow(IDataReader reader, IColumnMetaData[] columns)
        {
            return _rowCreator.CreateRow(reader, columns, _beanMetaData.BeanType);
        }

        /// <summary>
        /// 関連オブジェクトのColumnのメタデータを作成する
        /// </summary>
        /// <param name="columnNames">カラム名のリスト (NotNull)</param>
        /// <returns>関連オブジェクトのColumnのメタデータのDictionary (NotNull)</returns>
        protected virtual IDictionary<string, IDictionary<string, IColumnMetaData>> CreateRelationColumnMetaData(System.Collections.IList columnNames)
        {
            return _relationRowCreator.CreateRelationColumnMetaData(columnNames, _beanMetaData);
        }

        /// <summary>
        /// 1行分のオブジェクトに関連するオブジェクトを作成する
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="rpt">Relation property type. (NotNull)</param>
        /// <param name="columnNames">The list of column name. (NotNull)</param>
        /// <param name="relKeyValues">The hashtable of rel key values. (NotNull)</param>
        /// <param name="relationColumnMetaDataCache">The dictionary of relation column meta data cache. (NotNull)</param>
        /// <returns>1行分のEntity型のオブジェクト (Nullable)</returns>
        protected virtual object CreateRelationRow(IDataReader reader, IRelationPropertyType rpt,
            System.Collections.IList columnNames, System.Collections.Hashtable relKeyValues,
            IDictionary<string, IDictionary<string, IColumnMetaData>> relationColumnMetaDataCache)
        {
            return _relationRowCreator.CreateRelationRow(reader, rpt, columnNames, relKeyValues, relationColumnMetaDataCache);
        }



        protected virtual bool IsTargetProperty(IPropertyType pt) {// [DAONET-56] (2007/08/29)
            return pt.PropertyInfo.CanWrite;
        }

        protected virtual object CreateRelationRow(IRelationPropertyType rpt)
        {
            return ClassUtil.NewInstance(rpt.PropertyInfo.PropertyType);
        }

        protected virtual System.Collections.IList CreateColumnNames(DataTable dt)
        {
            System.Collections.IList columnNames = new CaseInsentiveSet();
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
