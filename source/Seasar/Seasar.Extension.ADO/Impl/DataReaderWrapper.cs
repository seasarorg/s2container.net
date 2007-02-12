#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
        private IDataReader original;

        public DataReaderWrapper(IDataReader original)
        {
            this.original = original;
        }

        #region IDataReader ÉÅÉìÉo

        public virtual void Close()
        {
            original.Close();
        }

        public virtual int Depth
        {
            get { return original.Depth; }
        }

        public virtual DataTable GetSchemaTable()
        {
            return original.GetSchemaTable();
        }

        public virtual bool IsClosed
        {
            get { return original.IsClosed; }
        }

        public virtual bool NextResult()
        {
            return original.NextResult();
        }

        public virtual bool Read()
        {
            return original.Read();
        }

        public virtual int RecordsAffected
        {
            get { return original.RecordsAffected; }
        }

        #endregion

        #region IDisposable ÉÅÉìÉo

        public virtual void Dispose()
        {
            original.Dispose();
        }

        #endregion

        #region IDataRecord ÉÅÉìÉo

        public virtual int FieldCount
        {
            get { return original.FieldCount; }
        }

        public virtual bool GetBoolean(int i)
        {
            return original.GetBoolean(i);
        }

        public virtual byte GetByte(int i)
        {
            return original.GetByte(i);
        }

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return original.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public virtual char GetChar(int i)
        {
            return original.GetChar(i);
        }

        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return original.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public virtual IDataReader GetData(int i)
        {
            return original.GetData(i);
        }

        public virtual string GetDataTypeName(int i)
        {
            return original.GetDataTypeName(i);
        }

        public virtual DateTime GetDateTime(int i)
        {
            return original.GetDateTime(i);
        }

        public virtual decimal GetDecimal(int i)
        {
            return original.GetDecimal(i);
        }

        public virtual double GetDouble(int i)
        {
            return original.GetDouble(i);
        }

        public virtual Type GetFieldType(int i)
        {
            return original.GetFieldType(i);
        }

        public virtual float GetFloat(int i)
        {
            return original.GetFloat(i);
        }

        public virtual Guid GetGuid(int i)
        {
            return original.GetGuid(i);
        }

        public virtual short GetInt16(int i)
        {
            return original.GetInt16(i);
        }

        public virtual int GetInt32(int i)
        {
            return original.GetInt32(i);
        }

        public virtual long GetInt64(int i)
        {
            return original.GetInt64(i);
        }

        public virtual string GetName(int i)
        {
            return original.GetName(i);
        }

        public virtual int GetOrdinal(string name)
        {
            return original.GetOrdinal(name);
        }

        public virtual string GetString(int i)
        {
            return original.GetString(i);
        }

        public virtual object GetValue(int i)
        {
            return original.GetValue(i);
        }

        public virtual int GetValues(object[] values)
        {
            return original.GetValues(values);
        }

        public virtual bool IsDBNull(int i)
        {
            return original.IsDBNull(i);
        }

        public virtual object this[string name]
        {
            get { return original[name]; }
        }

        public virtual object this[int i]
        {
            get { return original[i]; }
        }

        #endregion
    }
}
