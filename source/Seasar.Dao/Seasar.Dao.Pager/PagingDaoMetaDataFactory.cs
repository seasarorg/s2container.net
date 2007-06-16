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
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Pager
{
    public class PagingDaoMetaDataFactory : DaoMetaDataFactoryImpl
    {
        public PagingDaoMetaDataFactory(IDataSource dataSource,
            ICommandFactory commandFactory, IAnnotationReaderFactory readerFactory,
            IDataReaderFactory dataReaderFactory)
            : base(dataSource, commandFactory, readerFactory, dataReaderFactory)
        {
        }

        protected override IDaoMetaData CreateDaoMetaData(Type daoType)
        {
            DaoMetaDataImpl dmd = new PagingDaoMetaData();
            dmd.DaoType = daoType;
            dmd.DataSource = _dataSource;
            dmd.CommandFactory = _commandFactory;
            dmd.DataReaderFactory = _dataReaderFactory;
            dmd.AnnotationReaderFactory = _readerFactory;
            dmd.DatabaseMetaData = _dbMetaData;
            if (_sqlFileEncoding != null)
            {
                dmd.SqlFileEncoding = _sqlFileEncoding;
            }
            if (_insertPrefixes != null)
            {
                dmd.InsertPrefixes = _insertPrefixes;
            }
            if (_updatePrefixes != null)
            {
                dmd.UpdatePrefixes = _updatePrefixes;
            }
            if (_deletePrefixes != null)
            {
                dmd.DeletePrefixes = _deletePrefixes;
            }
            dmd.Initialize();
            return dmd;
        }
    }
}