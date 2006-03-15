#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Data;
using Seasar.Extension.ADO.Types;

namespace Seasar.Extension.ADO.Impl
{
    public class ObjectDataReaderHandler : IDataReaderHandler
    {
        public ObjectDataReaderHandler()
        {
        }

        #region IDataReaderHandler ÉÅÉìÉo

        public object Handle(System.Data.IDataReader dataReader)
        {
//            if(dataReader.Read())
//            {
//                DataTable dataTable = dataReader.GetSchemaTable();
//                IValueType valueType = ValueTypes.GetValueType(dataTable.Rows[0]["DataType"]);
//                return valueType.GetValue(dataReader, 1);
//            }
//            else
//            {
//                return null;
//            }
            return null;
        }

        #endregion
    }
}
