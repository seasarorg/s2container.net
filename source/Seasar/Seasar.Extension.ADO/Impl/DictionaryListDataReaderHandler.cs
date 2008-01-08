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

using System.Collections;
using System.Data;

namespace Seasar.Extension.ADO.Impl
{
    public class DictionaryListDataReaderHandler : AbstractDictionaryDataReaderHandler, IDataReaderHandler
    {
        #region IDataReaderHandler �����o

        public override object Handle(IDataReader reader)
        {
            IPropertyType[] propertyTypes = PropertyTypeUtil.CreatePropertyTypes(reader.GetSchemaTable());
            IList list = new ArrayList();
            while (reader.Read())
            {
                list.Add(CreateRow(reader, propertyTypes));
            }
            return list;
        }

        #endregion
    }
}
