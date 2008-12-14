#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Dao.Unit
{
    public class S2DaoTestCase : S2TestCase
    {
        protected virtual IDbms Dbms
        {
            get { return DbmsManager.GetDbms(DataSource); }
        }

        private IAnnotationReaderFactory _annotationReaderFactory = new FieldAnnotationReaderFactory();

        public IAnnotationReaderFactory AnnotationReaderFactory
        {
            get { return _annotationReaderFactory; }
            set { _annotationReaderFactory = value; }
        }

        private IDataReaderFactory _dataReaderFactory;

        public IDataReaderFactory DataReaderFactory
        {
            get
            {
                if (_dataReaderFactory == null)
                {
                    _dataReaderFactory = BasicDataReaderFactory.INSTANCE;
                }
                return _dataReaderFactory;
            }
            set { _dataReaderFactory = value; }
        }

        private IDataReaderHandlerFactory _dataReaderHandlerFactory;

        public IDataReaderHandlerFactory DataReaderHandlerFactory
        {
            get
            {
                if (_dataReaderHandlerFactory == null)
                {
                    _dataReaderHandlerFactory = new DataReaderHandlerFactory();
                }
                return _dataReaderHandlerFactory;
            }
            set { _dataReaderHandlerFactory = value; }
        }

        protected virtual BeanMetaDataImpl CreateBeanMetaData(Type beanType)
        {
            return CreateBeanMetaData(beanType, Dbms);
        }

        protected virtual BeanMetaDataImpl CreateBeanMetaData(Type beanType, IDbms dbms)
        {
            BeanMetaDataImpl beanMetaData = new BeanMetaDataImpl(
                beanType,
                new DatabaseMetaDataImpl(DataSource),
                dbms,
                AnnotationReaderFactory,
                false
                );
            return beanMetaData;
        }

        protected virtual IDaoMetaData CreateDaoMetaData(Type daoType)
        {
            DaoMetaDataFactoryImpl dmdf = new DaoMetaDataFactoryImpl(DataSource, CommandFactory, AnnotationReaderFactory, DataReaderFactory);
            dmdf.DataReaderHandlerFactory = DataReaderHandlerFactory;
            return dmdf.GetDaoMetaData(daoType);
        }
    }
}