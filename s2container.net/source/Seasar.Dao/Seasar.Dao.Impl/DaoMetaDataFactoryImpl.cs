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
using System.Text;
using System.Collections;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Impl
{
    public class DaoMetaDataFactoryImpl : IDaoMetaDataFactory
    {
        private readonly Hashtable _daoMetaDataCache = new Hashtable();
        protected readonly IDataSource _dataSource;
        protected readonly ICommandFactory _commandFactory;
        protected readonly IDataReaderFactory _dataReaderFactory;
        protected readonly IAnnotationReaderFactory _readerFactory;
        protected readonly IDatabaseMetaData _dbMetaData;
        protected string _sqlFileEncoding = Encoding.Default.WebName;
        protected string[] _insertPrefixes;
        protected string[] _updatePrefixes;
        protected string[] _deletePrefixes;

        public DaoMetaDataFactoryImpl(IDataSource dataSource,
            ICommandFactory commandFactory, IAnnotationReaderFactory readerFactory,
            IDataReaderFactory dataReaderFactory)
        {
            _dataSource = dataSource;
            _commandFactory = commandFactory;
            _readerFactory = readerFactory;
            _dataReaderFactory = dataReaderFactory;
            _dbMetaData = new DatabaseMetaDataImpl(dataSource);
        }

        public string[] InsertPrefixes
        {
            set { _insertPrefixes = value; }
        }

        public string[] UpdatePrefixes
        {
            set { _updatePrefixes = value; }
        }

        public string[] DeletePrefixes
        {
            set { _deletePrefixes = value; }
        }

        public string SqlFileEncoding
        {
            set { _sqlFileEncoding = value; }
        }

        #region IDaoMetaDataFactory ÉÅÉìÉo

        public IDaoMetaData GetDaoMetaData(Type daoType)
        {
            lock (this)
            {
                string key = daoType.FullName;
                IDaoMetaData dmd = (IDaoMetaData) _daoMetaDataCache[key];
                if (dmd != null)
                {
                    return dmd;
                }
                dmd = CreateDaoMetaData(daoType);
                _daoMetaDataCache[key] = dmd;
                return dmd;
            }
        }

        #endregion

        protected virtual IDaoMetaData CreateDaoMetaData(Type daoType)
        {
            DaoMetaDataImpl dmd = new DaoMetaDataImpl();
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
