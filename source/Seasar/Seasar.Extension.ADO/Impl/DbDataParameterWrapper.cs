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
using System.Data;

namespace Seasar.Extension.ADO.Impl
{
    public class DbDataParameterWrapper : MarshalByRefObject, IDataParameter, IDbDataParameter
    {
        private readonly IDbDataParameter original;

        public DbDataParameterWrapper(IDbDataParameter original)
        {
            this.original = original;
        }

        public virtual ParameterDirection Direction
        {
            get { return original.Direction; }
            set { original.Direction = value; }
        }

        public virtual DbType DbType
        {
            get { return original.DbType; }
            set { original.DbType = value; }
        }

        public virtual object Value
        {
            get { return original.Value; }
            set { original.Value = value; }
        }

        public virtual bool IsNullable
        {
            get { return original.IsNullable; }
        }

        public virtual DataRowVersion SourceVersion
        {
            get { return original.SourceVersion; }
            set { original.SourceVersion = value; }
        }

        public virtual string ParameterName
        {
            get { return original.ParameterName; }
            set { original.ParameterName = value; }
        }

        public virtual string SourceColumn
        {
            get { return original.SourceColumn; }
            set { original.SourceColumn = value; }
        }

        public virtual byte Precision
        {
            get { return original.Precision; }
            set { original.Precision = value; }
        }

        public virtual byte Scale
        {
            get { return original.Scale; }
            set { original.Scale = value; }
        }

        public virtual int Size
        {
            get { return original.Size; }
            set { original.Size = value; }
        }

        public virtual IDbDataParameter Original
        {
            get { return original; }
        }
    }
}