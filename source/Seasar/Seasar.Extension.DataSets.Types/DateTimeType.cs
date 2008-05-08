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

using System;
using System.Data;

namespace Seasar.Extension.DataSets.Types
{
    public class DateTimeType : ObjectType, IColumnType
    {
        #region IColumnType ÉÅÉìÉo

        public override object Convert(object value, string formatPattern)
        {
            if (IsNullable(value))
            {
                return DBNull.Value;
            }
            else if (value is DateTime)
            {
                DateTime d = (DateTime) value;
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
            }
            else if (value is string)
            {
                return DateTime.ParseExact(value as string, formatPattern, null);
            }
            return DBNull.Value;
        }

        public override string ToDbTypeString()
        {
            return "DATE";
        }

        public override DbType GetDbType()
        {
            return DbType.DateTime;
        }

        public override Type GetColumnType()
        {
            return typeof(DateTime);
        }

        #endregion
    }
}
