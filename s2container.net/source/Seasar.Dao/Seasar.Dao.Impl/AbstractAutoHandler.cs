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
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

#if NHIBERNATE_NULLABLES
using Nullables;
#endif

namespace Seasar.Dao.Impl
{
    public abstract class AbstractAutoHandler : BasicHandler, IUpdateHandler
    {
        protected AbstractAutoHandler(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, IPropertyType[] propertyTypes)
        {
            DataSource = dataSource;
            CommandFactory = commandFactory;
            BeanMetaData = beanMetaData;
            PropertyTypes = propertyTypes;
        }

        public new IDataSource DataSource
        {
            get { return base.DataSource; }
            set
            {
                if (value is ConnectionHolderDataSource)
                {
                    base.DataSource = value;
                }
                else
                {
                    base.DataSource = new ConnectionHolderDataSource(value);
                }
            }
        }

        public IBeanMetaData BeanMetaData { get; }

        protected static Logger Logger { get; } = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected object[] BindVariables { get; set; }

        protected Type[] BindVariableTypes { get; set; }

        protected DateTime Timestamp { get; set; } = DateTime.MinValue;

        protected int VersionNo { get; set; } = Int32.MinValue;

        protected IPropertyType[] PropertyTypes { get; set; }

        #region IUpdateHandler ÉÅÉìÉo

        public int Execute(object[] args)
        {
            var connection = Connection;
            try
            {
                return Execute(connection, args[0]);
            }
            finally
            {
                var holderDataSoure = DataSource as ConnectionHolderDataSource;
                if (holderDataSoure != null)
                {
                    holderDataSoure.ReleaseConnection();
                    holderDataSoure.CloseConnection(connection);
                }
                else
                {
                    throw new NullReferenceException("datasouce is null");
                }
            }
        }

        public int Execute(object[] args, Type[] argTypes)
        {
            return Execute(args);
        }

        public int Execute(object[] args, Type[] argTypes, string[] argNames)
        {
            return Execute(args);
        }

        #endregion

        protected int Execute(IDbConnection connection, object bean)
        {
            PreUpdateBean(bean);
            SetupBindVariables(bean);
            if (Logger.IsDebugEnabled) Logger.Debug(GetCompleteSql(BindVariables));
            var cmd = Command(connection);
            var ret = -1;
            try
            {
                BindArgs(cmd, BindVariables, BindVariableTypes);
                ret = CommandFactory.ExecuteNonQuery(DataSource, cmd);
            }
            finally
            {
                CommandUtil.Close(cmd);
            }
            PostUpdateBean(bean);
            return ret;
        }

        protected virtual void PreUpdateBean(object bean)
        {
        }

        protected virtual void PostUpdateBean(object bean)
        {
        }

        protected abstract void SetupBindVariables(object bean);

        protected void SetupInsertBindVariables(object bean)
        {
            var varList = new ArrayList();
            var varTypeList = new ArrayList();
            for (var i = 0; i < PropertyTypes.Length; ++i)
            {
                var pt = PropertyTypes[i];
                if (String.Compare(pt.PropertyName, BeanMetaData.TimestampPropertyName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Timestamp = DateTime.Now;
                    SetupTimestampVariableList(varList, pt);
                }
                else if (String.Compare(pt.PropertyName, BeanMetaData.VersionNoPropertyName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    VersionNo = 0;
                    varList.Add(ConversionUtil.ConvertTargetType(VersionNo, pt.PropertyInfo.PropertyType));
                }
                else
                {
//                    varList.Add(pt.PropertyInfo.GetValue(bean, null));
                    varList.Add(PropertyUtil.GetValue(bean, bean.GetExType(), pt.PropertyInfo.Name));
                }
                varTypeList.Add(pt.PropertyInfo.PropertyType);
            }
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[]) varTypeList.ToArray(typeof(Type));
        }

        protected void SetupUpdateBindVariables(object bean)
        {
            var varList = new ArrayList();
            var varTypeList = new ArrayList();
            for (var i = 0; i < PropertyTypes.Length; ++i)
            {
                var pt = PropertyTypes[i];
                if (String.Compare(pt.PropertyName, BeanMetaData.TimestampPropertyName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Timestamp = DateTime.Now;
                    SetupTimestampVariableList(varList, pt);
                }
                else if (String.Compare(pt.PropertyName, BeanMetaData.VersionNoPropertyName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    SetupVersionNoValiableList(varList, pt, bean);
                }
                else
                {
//                    varList.Add(pt.PropertyInfo.GetValue(bean, null));
                    varList.Add(PropertyUtil.GetValue(bean, bean.GetExType(), pt.PropertyInfo.Name));
                }
                varTypeList.Add(pt.PropertyInfo.PropertyType);
            }
            AddAutoUpdateWhereBindVariables(varList, varTypeList, bean);
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[]) varTypeList.ToArray(typeof(Type));
        }

        protected void SetupDeleteBindVariables(object bean)
        {
            var varList = new ArrayList();
            var varTypeList = new ArrayList();
            AddAutoUpdateWhereBindVariables(varList, varTypeList, bean);
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[]) varTypeList.ToArray(typeof(Type));
        }

        protected void AddAutoUpdateWhereBindVariables(ArrayList varList, ArrayList varTypeList,
            object bean)
        {
            var bmd = BeanMetaData;
            for (var i = 0; i < bmd.PrimaryKeySize; ++i)
            {
                var pt = bmd.GetPropertyTypeByColumnName(bmd.GetPrimaryKey(i));
                var pi = pt.PropertyInfo;
//                varList.Add(pi.GetValue(bean, null));
                varList.Add(PropertyUtil.GetValue(bean, bean.GetExType(), pi.Name));
                varTypeList.Add(pi.PropertyType);
            }
            if (bmd.HasVersionNoPropertyType)
            {
                var pt = bmd.VersionNoPropertyType;
                var pi = pt.PropertyInfo;
                Logger.Debug(pi.Name);
//                varList.Add(pi.GetValue(bean, null));
                varList.Add(PropertyUtil.GetValue(bean, bean.GetExType(), pi.Name));
                varTypeList.Add(pi.PropertyType);
            }
            if (bmd.HasTimestampPropertyType)
            {
                var pt = bmd.TimestampPropertyType;
                var pi = pt.PropertyInfo;
//                varList.Add(pi.GetValue(bean, null));
                varList.Add(PropertyUtil.GetValue(bean, bean.GetExType(), pi.Name));
                varTypeList.Add(pi.PropertyType);
            }
        }

        protected void UpdateTimestampIfNeed(object bean)
        {
            if (Timestamp != DateTime.MinValue)
            {
                var pi = BeanMetaData.TimestampPropertyType.PropertyInfo;
                SetupTimestampPropertyInfo(pi, bean);
            }
        }

        protected void UpdateVersionNoIfNeed(object bean)
        {
            if (VersionNo != Int32.MinValue)
            {
                var pi = BeanMetaData.VersionNoPropertyType.PropertyInfo;
                SetupVersionNoPropertyInfo(pi, bean);
            }
        }

        protected void SetupTimestampVariableList(IList varList, IPropertyType pt)
        {
            if (pt.PropertyType == typeof(DateTime))
            {
                varList.Add(Timestamp);
            }
#if NHIBERNATE_NULLABLES
            else if (pt.PropertyType == typeof(Nullables.NullableDateTime))
            {
                varList.Add(new Nullables.NullableDateTime(Timestamp));
            }
#endif
#if !NET_1_1
            else if (pt.PropertyType == typeof(DateTime?))
            {
                varList.Add(Timestamp);
            }
#endif
            else if (pt.PropertyType == typeof(SqlDateTime))
            {
                varList.Add(new SqlDateTime(Timestamp));
            }
            else
            {
                throw new WrongPropertyTypeOfTimestampException(pt.PropertyName, pt.PropertyType.Name);
            }
        }

        protected void SetupTimestampPropertyInfo(PropertyInfo pi, object bean)
        {
            if (pi.PropertyType == typeof(DateTime))
            {
//                pi.SetValue(bean, Timestamp, null);
                PropertyUtil.SetValue(bean, bean.GetExType(), pi.Name, pi.PropertyType, Timestamp);
            }
#if NHIBERNATE_NULLABLES
            else if (pi.PropertyType == typeof(Nullables.NullableDateTime))
            {
                pi.SetValue(bean, new Nullables.NullableDateTime(Timestamp), null);
            }
#endif
#if !NET_1_1
            else if (pi.PropertyType == typeof(DateTime?))
            {
//                pi.SetValue(bean, Timestamp, null);
                PropertyUtil.SetValue(bean, bean.GetExType(), pi.Name, pi.PropertyType,Timestamp);
            }
#endif
            else if (pi.PropertyType == typeof(SqlDateTime))
            {
//              pi.SetValue(bean, new SqlDateTime(Timestamp), null);
                PropertyUtil.SetValue(bean, bean.GetExType(), pi.Name, pi.PropertyType, new SqlDateTime(Timestamp));
            }
            else
            {
                throw new WrongPropertyTypeOfTimestampException(pi.Name, pi.PropertyType.Name);
            }
        }

        protected void SetupVersionNoValiableList(IList varList, IPropertyType pt, object bean)
        {
//            object value = pt.PropertyInfo.GetValue(bean, null);
            var value = PropertyUtil.GetValue(bean, bean.GetExType(), pt.PropertyInfo.Name);
#if NHIBERNATE_NULLABLES
            if (value is INullableType)
            {
                INullableType nullableValue = (INullableType)value;
                if (nullableValue.HasValue) 
                {
                    value = nullableValue.Value;
                }
                else 
                {
                    value = 0;
                }
            }
#endif
            var intValue = Convert.ToInt32(value) + 1;
            VersionNo = intValue;
            varList.Add(ConversionUtil.ConvertTargetType(VersionNo, pt.PropertyInfo.PropertyType));
        }

        protected void SetupVersionNoPropertyInfo(PropertyInfo pi, object bean)
        {
//            pi.SetValue(bean, ConversionUtil.ConvertTargetType(VersionNo, pi.PropertyType), null);
            PropertyUtil.SetValue(bean, bean.GetExType(), pi.Name, pi.PropertyType, ConversionUtil.ConvertTargetType(VersionNo, pi.PropertyType));
        }
    }
}
