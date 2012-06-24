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

namespace Seasar.Extension.ADO.Impl
{
    public class BooleanToIntParameter : DbDataParameterWrapper
    {
        public BooleanToIntParameter(IDbDataParameter original)
            : base(original)
        {
        }

        public override DbType DbType
        {
            get { return base.DbType; }
            set { base.DbType = ConvertDbType(value); }
        }

        public override object Value
        {
            get { return base.Value; }
            set { base.Value = ConvertValue(value); }
        }

        private DbType ConvertDbType(DbType dbType)
        {
            if (dbType == DbType.Boolean)
            {
                return DbType.Int32;
            }
            else
            {
                return dbType;
            }
        }

        private object ConvertValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return value;
            }
            if (value.GetType() == typeof(bool))
            {
                bool b = (bool) value;
                return Convert.ToInt32(b);
            }
            return value;
        }
    }
}
