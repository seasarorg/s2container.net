#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using System.Collections.Generic;
using System.Data;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
	public class ObjectGenericListDataReaderHandler : AbstractObjectListDataReaderHandler
	{
        private Type _elementType;

        public ObjectGenericListDataReaderHandler(Type elementType)
        {
            _elementType = elementType;
        }

        public override object Handle(IDataReader dataReader)
        {
            Type listType = typeof(List<>);
            Type genericType = listType.MakeGenericType(_elementType);
            object resultList = Activator.CreateInstance(genericType);
            Handle(dataReader, (IList)resultList);

            return resultList;
        }

        protected override object GetValue(object val)
        {
            return ConversionUtil.ConvertTargetType(val, _elementType);
        }
    }
}
