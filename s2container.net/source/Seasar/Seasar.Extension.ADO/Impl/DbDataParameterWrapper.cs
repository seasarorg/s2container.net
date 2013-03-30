#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
    public class DbDataParameterWrapper : MarshalByRefObject, IDbDataParameter
    {
        private readonly IDbDataParameter _original;

        public DbDataParameterWrapper(IDbDataParameter original)
        {
            _original = original;
        }

        public virtual ParameterDirection Direction
        {
            get { return _original.Direction; }
            set { _original.Direction = value; }
        }

        public virtual DbType DbType
        {
            get { return _original.DbType; }
            set { _original.DbType = value; }
        }

        public virtual object Value
        {
            get { return _original.Value; }
            set { _original.Value = value; }
        }

        public virtual bool IsNullable
        {
            get { return _original.IsNullable; }
        }

        public virtual DataRowVersion SourceVersion
        {
            get { return _original.SourceVersion; }
            set { _original.SourceVersion = value; }
        }

        public virtual string ParameterName
        {
            get { return _original.ParameterName; }
            set { _original.ParameterName = value; }
        }

        public virtual string SourceColumn
        {
            get { return _original.SourceColumn; }
            set { _original.SourceColumn = value; }
        }

        public virtual byte Precision
        {
            get { return _original.Precision; }
            set { _original.Precision = value; }
        }

        public virtual byte Scale
        {
            get { return _original.Scale; }
            set { _original.Scale = value; }
        }

        public virtual int Size
        {
            get { return _original.Size; }
            set { _original.Size = value; }
        }

        public virtual IDbDataParameter Original
        {
            get { return _original; }
        }
    }
}
