#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Util;

namespace Seasar.Extension.DataSets.Types
{
    public class BinaryType : ObjectType, IColumnType
    {
        #region IColumnType ÉÅÉìÉo

        public override object Convert(object value, string formatPattern)
        {
            if (IsNullable(value))
            {
                return DBNull.Value;
            }
            return BinaryConversionUtil.ToBinary(value);
        }

        public override DbType GetDbType()
        {
            return DbType.Binary;
        }

        public override Type GetColumnType()
        {
            return ValueTypes.BYTE_ARRAY_TYPE;
        }

        #endregion

        protected override bool DoEquals(object arg1, object arg2)
        {
            if (arg1.GetType() == GetColumnType() && arg1.GetType() == GetColumnType())
            {
                return ArrayEquals((Array) arg1, (Array) arg2);
            }
            return false;
        }

        bool ArrayEquals(Array arg1, Array arg2)
        {
            if (arg1.Length != arg2.Length)
            {
                return false;
            }
            for (int i = 0; i < arg1.Length; i++)
            {
                if (!arg1.GetValue(i).Equals(arg2.GetValue(i)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
