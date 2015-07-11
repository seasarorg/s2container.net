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
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class ObjectArrayDataReaderHandler : AbstractObjectListDataReaderHandler
    {
        private readonly Type _elementType;

        public ObjectArrayDataReaderHandler(Type elementType)
        {
            _elementType = elementType;
        }

        public override object Handle(IDataReader dataReader)
        {
            ArrayList resultList = new ArrayList();
            Handle(dataReader, resultList);

            Array returnArray = Array.CreateInstance(_elementType, resultList.Count);
            for (int i = 0; i < resultList.Count; i++)
            {
                returnArray.SetValue(ConversionUtil.ConvertTargetType(resultList[i], _elementType), i);
            }

            return returnArray;
        }
    }
}
