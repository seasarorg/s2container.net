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
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Dao.Unit
{
    public class S2DaoTestCase : S2TestCase
    {
        private IAnnotationReaderFactory _annotationReaderFactory;

        protected virtual IDbms Dbms
        {
            get
            {
                return DbmsManager.GetDbms(DataSource);
            }
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
                GetAnnotationReaderFactory(),
                false
                );
            return beanMetaData;
        }

        protected virtual DaoMetaDataImpl CreateDaoMetaData(Type daoType)
        {
            DaoMetaDataImpl dmd = new DaoMetaDataImpl();
            dmd.DaoType = daoType;
            dmd.DataSource = DataSource;
            dmd.CommandFactory = BasicCommandFactory.INSTANCE;
            dmd.DataReaderFactory = BasicDataReaderFactory.INSTANCE;
            dmd.AnnotationReaderFactory = GetAnnotationReaderFactory();
            dmd.DatabaseMetaData = new DatabaseMetaDataImpl(DataSource);
            dmd.Initialize();
            return dmd;
        }

        protected virtual IAnnotationReaderFactory GetAnnotationReaderFactory()
        {
            if (_annotationReaderFactory == null)
            {
                _annotationReaderFactory = new FieldAnnotationReaderFactory();
            }
            return _annotationReaderFactory;
        }
    }
}