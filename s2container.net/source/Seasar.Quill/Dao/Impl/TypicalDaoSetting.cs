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

using Seasar.Dao.Impl;
using Seasar.Dao.Interceptors;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Quill.Dao.Impl
{
    /// <summary>
    /// 一般的なDaoSetting実装クラス
    /// </summary>
    public class TypicalDaoSetting : AbstractDaoSetting
    {
        protected override void SetupDao(IDataSource dataSource)
        {
            BasicCommandFactory commandFacoty = new BasicCommandFactory();
            BasicDataReaderFactory dataReaderFactory = new BasicDataReaderFactory(commandFacoty);
            FieldAnnotationReaderFactory annotationReaderFactory = new FieldAnnotationReaderFactory();
            _daoMetaDataFactory = new DaoMetaDataFactoryImpl(
                dataSource, commandFacoty, annotationReaderFactory, dataReaderFactory);
            _daoInterceptor = new S2DaoInterceptor(_daoMetaDataFactory);
        }
    }
}
