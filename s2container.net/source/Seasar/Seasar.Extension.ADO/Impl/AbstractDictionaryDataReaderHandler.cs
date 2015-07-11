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

namespace Seasar.Extension.ADO.Impl
{
    public abstract class AbstractDictionaryDataReaderHandler : IDataReaderHandler
    {
        protected IDictionary CreateRow(IDataReader reader, IPropertyType[] propertyTypes)
        {
#if NET_1_1
            IDictionary row = new Hashtable(
                new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
#else
            IDictionary row = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
#endif
            for (int i = 0; i < propertyTypes.Length; ++i)
            {
                object value = propertyTypes[i].ValueType.GetValue(
                    reader,
                    i
                    );
                row.Add(propertyTypes[i].PropertyName, value);
            }
            return row;
        }

        #region IDataReaderHandler ƒƒ“ƒo

        public abstract object Handle(IDataReader dataReader);

        #endregion
    }
}
