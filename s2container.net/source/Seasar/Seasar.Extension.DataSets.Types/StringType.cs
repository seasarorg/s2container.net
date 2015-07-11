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
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.Types
{
    public class StringType : ObjectType, IColumnType
    {
        #region IColumnType ÉÅÉìÉo

        public override object Convert(object value, string formatPattern)
        {
            if (IsNullable(value))
            {
                return DBNull.Value;
            }
            string s = StringConversionUtil.ToString(value, formatPattern);
            if (s != null)
            {
                s = s.Trim();
            }
            if (s == string.Empty)
            {
                s = null;
            }
            return s;
        }

        public override string ToDbTypeString()
        {
            return "VARCHAR";
        }

        public override DbType GetDbType()
        {
            return DbType.String;
        }

        public override Type GetColumnType()
        {
            return typeof(string);
        }

        #endregion
    }
}
