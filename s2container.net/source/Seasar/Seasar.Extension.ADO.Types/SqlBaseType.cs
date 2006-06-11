#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Data.SqlTypes;
using System.Reflection;

namespace Seasar.Extension.ADO.Types
{
    public abstract class SqlBaseType : BaseValueType
    {
		public SqlBaseType()
        {
        }

		protected override object GetBindValue(object value)
		{
			if (value == null)
			{
				return DBNull.Value;
			}
			INullable nValue = (INullable) value;
			if (nValue.IsNull)
			{
				return DBNull.Value;
			}
			PropertyInfo pi = value.GetType().GetProperty("Value");
			return pi.GetValue(value, null);
		}
    }
}
