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
using System.Data;

namespace Seasar.Extension.ADO.Impl
{
    public class DataReaderWrapper : IDataReader
    {
        private readonly IDataReader _original;

        public DataReaderWrapper(IDataReader original)
        {
            _original = original;
        }

        #region IDataReader ƒƒ“ƒo

        public virtual void Close()
        {
            _original.Close();
        }

        public virtual int Depth => _original.Depth;

        public virtual DataTable GetSchemaTable() => _original.GetSchemaTable();

        public virtual bool IsClosed => _original.IsClosed;

        public virtual bool NextResult() => _original.NextResult();

        public virtual bool Read() => _original.Read();

        public virtual int RecordsAffected => _original.RecordsAffected;

        #endregion

        #region IDisposable ƒƒ“ƒo

        public virtual void Dispose()
        {
            _original.Dispose();
        }

        #endregion

        #region IDataRecord ƒƒ“ƒo

        public virtual int FieldCount => _original.FieldCount;

        public virtual bool GetBoolean(int i) => _original.GetBoolean(i);

        public virtual byte GetByte(int i) => _original.GetByte(i);

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return _original.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public virtual char GetChar(int i) => _original.GetChar(i);

        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return _original.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public virtual IDataReader GetData(int i) => _original.GetData(i);

        public virtual string GetDataTypeName(int i) => _original.GetDataTypeName(i);

        public virtual DateTime GetDateTime(int i) => _original.GetDateTime(i);

        public virtual decimal GetDecimal(int i) => _original.GetDecimal(i);

        public virtual double GetDouble(int i) => _original.GetDouble(i);

        public virtual Type GetFieldType(int i) => _original.GetFieldType(i);

        public virtual float GetFloat(int i) => _original.GetFloat(i);

        public virtual Guid GetGuid(int i) => _original.GetGuid(i);

        public virtual short GetInt16(int i) => _original.GetInt16(i);

        public virtual int GetInt32(int i) => _original.GetInt32(i);

        public virtual long GetInt64(int i) => _original.GetInt64(i);

        public virtual string GetName(int i) => _original.GetName(i);

        public virtual int GetOrdinal(string name) => _original.GetOrdinal(name);

        public virtual string GetString(int i) => _original.GetString(i);

        public virtual object GetValue(int i) => _original.GetValue(i);

        public virtual int GetValues(object[] values) => _original.GetValues(values);

        public virtual bool IsDBNull(int i) => _original.IsDBNull(i);

        public virtual object this[string name] => _original[name];

        public virtual object this[int i] => _original[i];

        #endregion
    }
}
