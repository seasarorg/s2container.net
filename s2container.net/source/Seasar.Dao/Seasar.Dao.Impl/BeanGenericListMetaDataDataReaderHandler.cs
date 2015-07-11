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
using System.Data;

namespace Seasar.Dao.Impl
{
    public class BeanGenericListMetaDataDataReaderHandler
        : BeanListMetaDataDataReaderHandler
    {
        public BeanGenericListMetaDataDataReaderHandler(
            IBeanMetaData beanMetaData, IRowCreator rowCreator, IRelationRowCreator relationRowCreator)
            : base(beanMetaData, rowCreator, relationRowCreator)
        {
        }

        public override object Handle(IDataReader dataReader)
        {
            Type generic = typeof(System.Collections.Generic.List<>);
            Type constructed = generic.MakeGenericType(BeanMetaData.BeanType);

            object list = Activator.CreateInstance(constructed);

            Handle(dataReader, (IList) list);

            return list;
        }
    }
}
