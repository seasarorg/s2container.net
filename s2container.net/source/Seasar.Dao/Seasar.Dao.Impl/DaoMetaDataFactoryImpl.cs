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
using System.Text;
using System.Collections;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Impl
{
    public class DaoMetaDataFactoryImpl : IDaoMetaDataFactory
    {
        private readonly Hashtable _daoMetaDataCache = new Hashtable();
        protected IDataSource _dataSource;
        protected ICommandFactory _commandFactory;
        protected IDataReaderFactory _dataReaderFactory;
        protected IDataReaderHandlerFactory _dataReaderHandlerFactory = Impl.DataReaderHandlerFactory.INSTANCE;
        protected IAnnotationReaderFactory _annotationReaderFactory;
        protected IDatabaseMetaData _dbMetaData;
        protected string _sqlFileEncoding = Encoding.Default.WebName;
        protected string[] _insertPrefixes;
        protected string[] _updatePrefixes;
        protected string[] _deletePrefixes;

        public DaoMetaDataFactoryImpl(IDataSource dataSource,
            ICommandFactory commandFactory, IAnnotationReaderFactory annotationReaderFactory,
            IDataReaderFactory dataReaderFactory)
        {
            _dataSource = dataSource;
            _commandFactory = commandFactory;
            _annotationReaderFactory = annotationReaderFactory;
            _dataReaderFactory = dataReaderFactory;
            _dbMetaData = new DatabaseMetaDataImpl(_dataSource);
        }

        public IDataSource DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }

        public ICommandFactory CommandFactory
        {
            get { return _commandFactory; }
            set { _commandFactory = value; }
        }

        public IAnnotationReaderFactory AnnotationReaderFactory
        {
            get { return _annotationReaderFactory; }
            set { _annotationReaderFactory = value; }
        }

        public IDataReaderFactory DataReaderFactory
        {
            get { return _dataReaderFactory; }
            set { _dataReaderFactory = value; }
        }

        public IDataReaderHandlerFactory DataReaderHandlerFactory
        {
            get { return _dataReaderHandlerFactory; }
            set { _dataReaderHandlerFactory = value; }
        }

        public IDatabaseMetaData DBMetaData
        {
            get { return _dbMetaData; }
            set { _dbMetaData = value; }
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
            DaoMetaDataImpl dmd = CreateDaoMetaDataImpl();
            dmd.DaoType = daoType;
            dmd.DataSource = _dataSource;
            dmd.CommandFactory = _commandFactory;
            dmd.DataReaderFactory = _dataReaderFactory;
            if (_dataReaderHandlerFactory == null)
            {
                _dataReaderHandlerFactory = new DataReaderHandlerFactory();
            }
            dmd.DataReaderHandlerFactory = _dataReaderHandlerFactory;
            dmd.AnnotationReaderFactory = _annotationReaderFactory;
            if (_dbMetaData == null)
            {
                _dbMetaData = new DatabaseMetaDataImpl(_dataSource);
            }
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

        protected virtual DaoMetaDataImpl CreateDaoMetaDataImpl()
        {
            return new DaoMetaDataImpl();
        }
    }
}
