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
using System.Reflection;
using Nullables;

namespace Seasar.Extension.Unit
{
	public class BeanReader : Seasar.Extension.DataSets.IDataReader
	{
		private DataSet dataSet_;

		private DataTable table_;

		protected BeanReader() : this(null)
		{
		}

		public BeanReader(object bean)
		{
			dataSet_ = new DataSet();
			table_ = dataSet_.Tables.Add("Bean");

			if (bean != null) 
			{
				Type beanType = bean.GetType();
				SetupColumns(beanType);
				SetupRow(beanType, bean);
			}
		}

		protected void SetupColumns(Type beanType) 
		{
			foreach (PropertyInfo pi in beanType.GetProperties()) 
			{
				Type propertyType = GetPrimitiveType(pi);
                table_.Columns.Add(pi.Name, propertyType);
			}
		}

		protected void SetupRow(Type beanType, object bean) 
		{
			DataRow row = table_.NewRow();
			foreach (PropertyInfo pi in beanType.GetProperties()) 
			{
				row[pi.Name] = GetPrimitiveValue(pi, bean);
			}
			table_.Rows.Add(row);
			row.AcceptChanges();
		}

		private Type GetPrimitiveType(PropertyInfo pi)
		{
			Type propertyType = pi.PropertyType;
			if (propertyType.GetInterface(typeof(INullableType).Name) != null)
			{
				ConstructorInfo[] constructorInfos = propertyType.GetConstructors();
				ParameterInfo[] parameterInfos = constructorInfos[0].GetParameters();
				propertyType = parameterInfos[0].ParameterType;
			}
			else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				propertyType = Nullable.GetUnderlyingType(propertyType);
			}
			return propertyType;
		}

		private object GetPrimitiveValue(PropertyInfo pi, object bean)
		{
			object value = pi.GetValue(bean, null);

			if (value is INullableType)
			{
				INullableType nullableType = (INullableType) value;
				if (nullableType.HasValue)
				{
					return nullableType.Value;
				}
				else
				{
					return DBNull.Value;
				}
			}

			if (value == null)
			{
				return DBNull.Value;
			}

			return value;
		}

		#region IDataReader ÉÅÉìÉo

		public DataSet Read()
		{
			return dataSet_;
		}

		#endregion
	}
}
