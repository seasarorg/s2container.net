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
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Impl
{
    public class DaoMetaDataFactoryImpl : IDaoMetaDataFactory
    {
        private readonly Hashtable _daoMetaDataCache = new Hashtable();
        protected IDataSource dataSource;
        protected ICommandFactory commandFactory;
        protected IDataReaderFactory dataReaderFactory;
        protected IDataReaderHandlerFactory dataReaderHandlerFactory = Impl.DataReaderHandlerFactory.INSTANCE;
        protected IAnnotationReaderFactory annotationReaderFactory;
        protected IDatabaseMetaData dbMetaData;
        protected string sqlFileEncoding = Encoding.Default.WebName;
        protected string[] insertPrefixes;
        protected string[] updatePrefixes;
        protected string[] deletePrefixes;

        public DaoMetaDataFactoryImpl(IDataSource dataSource,
            ICommandFactory commandFactory, IAnnotationReaderFactory annotationReaderFactory,
            IDataReaderFactory dataReaderFactory)
        {
            this.dataSource = dataSource;
            this.commandFactory = commandFactory;
            this.annotationReaderFactory = annotationReaderFactory;
            this.dataReaderFactory = dataReaderFactory;
            dbMetaData = new DatabaseMetaDataImpl(this.dataSource);
        }

        public IDataSource DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public ICommandFactory CommandFactory
        {
            get { return commandFactory; }
            set { commandFactory = value; }
        }

        public IAnnotationReaderFactory AnnotationReaderFactory
        {
            get { return annotationReaderFactory; }
            set { annotationReaderFactory = value; }
        }

        public IDataReaderFactory DataReaderFactory
        {
            get { return dataReaderFactory; }
            set { dataReaderFactory = value; }
        }

        public IDataReaderHandlerFactory DataReaderHandlerFactory
        {
            get { return dataReaderHandlerFactory; }
            set { dataReaderHandlerFactory = value; }
        }

        public IDatabaseMetaData DbMetaData
        {
            get { return dbMetaData; }
            set { dbMetaData = value; }
        }

        public string[] InsertPrefixes
        {
            set { insertPrefixes = value; }
        }

        public string[] UpdatePrefixes
        {
            set { updatePrefixes = value; }
        }

        public string[] DeletePrefixes
        {
            set { deletePrefixes = value; }
        }

        public string SqlFileEncoding
        {
            set { sqlFileEncoding = value; }
        }

        #region IDaoMetaDataFactory ƒƒ“ƒo

        public IDaoMetaData GetDaoMetaData(Type daoType)
        {
            lock (this)
            {
                var key = daoType.FullName;
                var dmd = (IDaoMetaData) _daoMetaDataCache[key];
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
            var dmd = CreateDaoMetaDataImpl();
            dmd.DaoType = daoType;
            dmd.DataSource = dataSource;
            dmd.CommandFactory = commandFactory;
            dmd.DataReaderFactory = dataReaderFactory;
            if (dataReaderHandlerFactory == null)
            {
                dataReaderHandlerFactory = new DataReaderHandlerFactory();
            }
            dmd.DataReaderHandlerFactory = dataReaderHandlerFactory;
            dmd.AnnotationReaderFactory = annotationReaderFactory;
            if (dbMetaData == null)
            {
                dbMetaData = new DatabaseMetaDataImpl(dataSource);
            }
            dmd.DatabaseMetaData = dbMetaData;
            if (sqlFileEncoding != null)
            {
                dmd.SqlFileEncoding = sqlFileEncoding;
            }
            if (insertPrefixes != null)
            {
                dmd.InsertPrefixes = insertPrefixes;
            }
            if (updatePrefixes != null)
            {
                dmd.UpdatePrefixes = updatePrefixes;
            }
            if (deletePrefixes != null)
            {
                dmd.DeletePrefixes = deletePrefixes;
            }
            dmd.Initialize();
            return dmd;
        }

        protected virtual DaoMetaDataImpl CreateDaoMetaDataImpl() => new DaoMetaDataImpl();
    }
}
