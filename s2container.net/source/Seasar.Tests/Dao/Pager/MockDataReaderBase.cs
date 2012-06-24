#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

namespace Seasar.Tests.Dao.Pager
{
    public class MockDataReaderBase : IDataReader
    {
        #region IDataReader ÉÅÉìÉo

        public virtual void Close()
        {
        }

        public virtual int Depth
        {
            get { return 0; }
        }

        public virtual DataTable GetSchemaTable()
        {
            return null;
        }

        public virtual bool IsClosed
        {
            get { return false; }
        }

        public virtual bool NextResult()
        {
            return false;
        }

        public virtual bool Read()
        {
            return true;
        }

        public virtual int RecordsAffected
        {
            get { return 0; }
        }

        #endregion

        #region IDisposable ÉÅÉìÉo

        public virtual void Dispose()
        {
        }

        #endregion

        #region IDataRecord ÉÅÉìÉo

        public virtual int FieldCount
        {
            get { return 0; }
        }

        public virtual bool GetBoolean(int i)
        {
            return true;
        }

        public virtual byte GetByte(int i)
        {
            return 0;
        }

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        public virtual char GetChar(int i)
        {
            return 'x';
        }

        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        public virtual IDataReader GetData(int i)
        {
            return null;
        }

        public virtual string GetDataTypeName(int i)
        {
            return null;
        }

        public virtual DateTime GetDateTime(int i)
        {
            return DateTime.MinValue;
        }

        public virtual decimal GetDecimal(int i)
        {
            return 0;
        }

        public virtual double GetDouble(int i)
        {
            return 0;
        }

        public virtual Type GetFieldType(int i)
        {
            return null;
        }

        public virtual float GetFloat(int i)
        {
            return 0;
        }

        public virtual Guid GetGuid(int i)
        {
            return Guid.Empty;
        }

        public virtual short GetInt16(int i)
        {
            return 0;
        }

        public virtual int GetInt32(int i)
        {
            return 0;
        }

        public virtual long GetInt64(int i)
        {
            return 0;
        }

        public virtual string GetName(int i)
        {
            return null;
        }

        public virtual int GetOrdinal(string name)
        {
            return 0;
        }

        public virtual string GetString(int i)
        {
            return null;
        }

        public virtual object GetValue(int i)
        {
            return null;
        }

        public virtual int GetValues(object[] values)
        {
            return 0;
        }

        public virtual bool IsDBNull(int i)
        {
            return true;
        }

        public virtual object this[string name]
        {
            get { return null; }
        }

        public virtual object this[int i]
        {
            get { return null; }
        }

        #endregion
    }
}
