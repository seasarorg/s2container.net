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
using Nullables;

namespace Seasar.Extension.DataSets.Types
{
    public class ObjectType : IColumnType
    {
        #region IColumnType ÉÅÉìÉo

        public virtual object Convert(object value, string formatPattern)
        {
            if (IsNullable(value))
            {
                return DBNull.Value;
            }
            return value;
        }

        public bool Equals1(object arg1, object arg2)
        {
            return DoEquals(arg1, arg2);
        }

        public virtual string ToDbTypeString()
        {
            return "VARCHAR";
        }

        public virtual DbType GetDbType()
        {
            return DbType.Object;
        }

        public virtual Type GetColumnType()
        {
            return typeof(object);
        }

        #endregion

        protected virtual bool DoEquals(object arg1, object arg2)
        {
            try
            {
                if (IsNullable(arg1))
                {
                    arg1 = DBNull.Value;
                }
                else
                {
                    arg1 = Convert(arg1, null);
                }
            }
            catch
            {
                return false;
            }
            try
            {
                if (IsNullable(arg2))
                {
                    arg2 = DBNull.Value;
                }
                else
                {
                    arg2 = Convert(arg2, null);
                }
            }
            catch
            {
                return false;
            }
            return arg1 != null ? arg1.Equals(arg2) : arg1 == arg2;
        }

        protected bool IsNullable(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (value == DBNull.Value)
            {
                return true;
            }
            if (value is INullableType)
            {
                if (!((INullableType) value).HasValue)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
