#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public class BeanDataSetMetaDataDataReaderHandler : IDataReaderHandler
    {
        protected const int DEFAULT_TABLE_NUM = 1;
        private readonly Type _returnType;

        public BeanDataSetMetaDataDataReaderHandler(Type returnType)
        {
            _returnType = returnType;
        }

        public virtual object Handle(IDataReader dataReader)
        {
            DataSet dataSet = (DataSet)Activator.CreateInstance(_returnType);
            Handle(dataReader, dataSet);
            return dataSet;
        }

        protected virtual void Handle(IDataReader dataReader, DataSet dataSet)
        {
            if ( dataSet.Tables.Count == 0 )
            {
                DataTable table = new DataTable();
                dataSet.Tables.Add(table);
            }
            DataTable[] tables = new DataTable[dataSet.Tables.Count];
            dataSet.Tables.CopyTo(tables, 0);
            dataSet.Load(dataReader, LoadOption.OverwriteChanges, tables);
        }
    }
}
