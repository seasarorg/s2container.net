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
using System.Runtime.Serialization;
using Seasar.Framework.Exceptions;

namespace Seasar.Extension.ADO
{
    [Serializable]
    public class ColumnNotFoundRuntimeException : SRuntimeException
    {
        private readonly string _tableName;
        private readonly string _columnName;

        public ColumnNotFoundRuntimeException(string tableName, string columnName)
            : base("ESSR0068", new object[] { tableName, columnName })
        {
            this._tableName = tableName;
            this._columnName = columnName;
        }

        public ColumnNotFoundRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _tableName = info.GetString("_tableName");
            _columnName = info.GetString("_columnName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_tableName", _tableName, typeof(string));
            info.AddValue("_columnName", _columnName, typeof(string));
            base.GetObjectData(info, context);
        }

        public string TableName
        {
            get { return _tableName; }
        }

        public string ColumnName
        {
            get { return _columnName; }
        }
    }
}
