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

using Seasar.Dao;
using Seasar.Dao.Impl;
using Seasar.Dao.Interceptors;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Aop;

namespace Seasar.Quill.Dao.Impl
{
    /// <summary>
    /// 一般的なDaoSetting実装クラス
    /// </summary>
    public class TypicalDaoSetting : AbstractDaoSetting
    {
        protected override void SetupDao(IDataSource dataSource)
        {
            var commandFacoty = CreateCommandFactory();
            var dataReaderFactory = CreateDataReaderFactory(commandFacoty);
            var annotationReaderFactory = CreateAnnotationReaderFactory();
            var factory = CreateDaoMetaDataFactory(
                dataSource, commandFacoty, annotationReaderFactory, dataReaderFactory);
            daoInterceptor = CreateS2DaoInterceptor(factory);
        }

        protected virtual ICommandFactory CreateCommandFactory() => new BasicCommandFactory();

        protected virtual IDataReaderFactory CreateDataReaderFactory(ICommandFactory commandFactory) 
            => new BasicDataReaderFactory(commandFactory);

        protected virtual IAnnotationReaderFactory CreateAnnotationReaderFactory() => new FieldAnnotationReaderFactory();

        protected virtual IDaoMetaDataFactory CreateDaoMetaDataFactory(
            IDataSource dataSource, ICommandFactory commandFactory,
            IAnnotationReaderFactory annotationReaderFactory, IDataReaderFactory dataReaderFactory)
        {
            return new DaoMetaDataFactoryImpl(
                dataSource, commandFactory, annotationReaderFactory, dataReaderFactory);
        }

        protected virtual IMethodInterceptor CreateS2DaoInterceptor(IDaoMetaDataFactory factory) => new S2DaoInterceptor(factory);
    }
}
