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
using System.Collections;
using System.Data;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Log;
using Seasar.Framework.Util;
using Nullables;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractAutoHandler : BasicHandler, IUpdateHandler
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBeanMetaData _beanMetaData;
        private object[] _bindVariables;
        private Type[] _bindVariableTypes;
        private DateTime _timestamp = DateTime.MinValue;
        private Int32 _versionNo = Int32.MinValue;
        private IPropertyType[] _propertyTypes;

        public AbstractAutoHandler(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, IPropertyType[] propertyTypes)
        {
            DataSource = dataSource;
            CommandFactory = commandFactory;
            _beanMetaData = beanMetaData;
            _propertyTypes = propertyTypes;
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

        public IBeanMetaData BeanMetaData
        {
            get { return _beanMetaData; }
        }

        protected static Logger Logger
        {
            get { return _logger; }
        }

        protected object[] BindVariables
        {
            get { return _bindVariables; }
            set { _bindVariables = value; }
        }

        protected Type[] BindVariableTypes
        {
            get { return _bindVariableTypes; }
            set { _bindVariableTypes = value; }
        }

        protected DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        protected int VersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }

        protected IPropertyType[] PropertyTypes
        {
            get { return _propertyTypes; }
            set { _propertyTypes = value; }
        }

        #region IUpdateHandler ÉÅÉìÉo

        public int Execute(object[] args)
        {
            IDbConnection connection = Connection;
            try
            {
                return Execute(connection, args[0]);
            }
            finally
            {
                ConnectionHolderDataSource holderDataSoure = DataSource as ConnectionHolderDataSource;
                holderDataSoure.ReleaseConnection();
                DataSourceUtil.CloseConnection(holderDataSoure, connection);
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
            if (_logger.IsDebugEnabled) _logger.Debug(GetCompleteSql(_bindVariables));
            IDbCommand cmd = Command(connection);
            int ret = -1;
            try
            {
                BindArgs(cmd, _bindVariables, _bindVariableTypes);
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
            ArrayList varList = new ArrayList();
            ArrayList varTypeList = new ArrayList();
            for (int i = 0; i < _propertyTypes.Length; ++i)
            {
                IPropertyType pt = _propertyTypes[i];
                if (string.Compare(pt.PropertyName, BeanMetaData.TimestampPropertyName, true) == 0)
                {
                    Timestamp = DateTime.Now;
                    SetupTimestampVariableList(varList, pt);
                }
                else if (pt.PropertyName.Equals(BeanMetaData.VersionNoPropertyName))
                {
                    VersionNo = 0;
                    varList.Add(ConversionUtil.ConvertTargetType(VersionNo, pt.PropertyInfo.PropertyType));
                }
                else
                {
                    varList.Add(pt.PropertyInfo.GetValue(bean, null));
                }
                varTypeList.Add(pt.PropertyInfo.PropertyType);
            }
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[]) varTypeList.ToArray(typeof(Type));
        }

        protected void SetupUpdateBindVariables(object bean)
        {
            ArrayList varList = new ArrayList();
            ArrayList varTypeList = new ArrayList();
            for (int i = 0; i < _propertyTypes.Length; ++i)
            {
                IPropertyType pt = _propertyTypes[i];
                if (string.Compare(pt.PropertyName, BeanMetaData.TimestampPropertyName, true) == 0)
                {
                    Timestamp = DateTime.Now;
                    SetupTimestampVariableList(varList, pt);
                }
                else if (string.Compare(pt.PropertyName, BeanMetaData.VersionNoPropertyName, true) == 0)
                {
                    SetupVersionNoValiableList(varList, pt, bean);
                }
                else
                {
                    varList.Add(pt.PropertyInfo.GetValue(bean, null));
                }
                varTypeList.Add(pt.PropertyInfo.PropertyType);
            }
            AddAutoUpdateWhereBindVariables(varList, varTypeList, bean);
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[]) varTypeList.ToArray(typeof(Type));
        }

        protected void SetupDeleteBindVariables(object bean)
        {
            ArrayList varList = new ArrayList();
            ArrayList varTypeList = new ArrayList();
            AddAutoUpdateWhereBindVariables(varList, varTypeList, bean);
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[]) varTypeList.ToArray(typeof(Type));
        }

        protected void AddAutoUpdateWhereBindVariables(ArrayList varList, ArrayList varTypeList,
            object bean)
        {
            IBeanMetaData bmd = BeanMetaData;
            for (int i = 0; i < bmd.PrimaryKeySize; ++i)
            {
                IPropertyType pt = bmd.GetPropertyTypeByColumnName(bmd.GetPrimaryKey(i));
                PropertyInfo pi = pt.PropertyInfo;
                varList.Add(pi.GetValue(bean, null));
                varTypeList.Add(pi.PropertyType);
            }
            if (bmd.HasVersionNoPropertyType)
            {
                IPropertyType pt = bmd.VersionNoPropertyType;
                PropertyInfo pi = pt.PropertyInfo;
                Logger.Debug(pi.Name);
                varList.Add(pi.GetValue(bean, null));
                varTypeList.Add(pi.PropertyType);
            }
            if (bmd.HasTimestampPropertyType)
            {
                IPropertyType pt = bmd.TimestampPropertyType;
                PropertyInfo pi = pt.PropertyInfo;
                varList.Add(pi.GetValue(bean, null));
                varTypeList.Add(pi.PropertyType);
            }
        }

        protected void UpdateTimestampIfNeed(object bean)
        {
            if (Timestamp != DateTime.MinValue)
            {
                PropertyInfo pi = BeanMetaData.TimestampPropertyType.PropertyInfo;
                SetupTimestampPropertyInfo(pi, bean);
            }
        }

        protected void UpdateVersionNoIfNeed(object bean)
        {
            if (VersionNo != Int32.MinValue)
            {
                PropertyInfo pi = BeanMetaData.VersionNoPropertyType.PropertyInfo;
                SetupVersionNoPropertyInfo(pi, bean);
            }
        }

        protected void SetupTimestampVariableList(IList varList, IPropertyType pt)
        {
            if (pt.PropertyType == typeof(DateTime))
            {
                varList.Add(Timestamp);
            }
            else if (pt.PropertyType == typeof(Nullables.NullableDateTime))
            {
                varList.Add(new Nullables.NullableDateTime(Timestamp));
            }
#if !NET_1_1
            else if (pt.PropertyType == typeof(DateTime?))
            {
                varList.Add(Timestamp);
            }
#endif
            else
            {
                throw new WrongPropertyTypeOfTimestampException(pt.PropertyName, pt.PropertyType.Name);
            }
        }

        protected void SetupTimestampPropertyInfo(PropertyInfo pi, object bean)
        {
            if (pi.PropertyType == typeof(DateTime))
            {
                pi.SetValue(bean, Timestamp, null);
            }
            else if (pi.PropertyType == typeof(Nullables.NullableDateTime))
            {
                pi.SetValue(bean, new Nullables.NullableDateTime(Timestamp), null);
            }
#if !NET_1_1
            else if (pi.PropertyType == typeof(DateTime?))
            {
                pi.SetValue(bean, new DateTime?(Timestamp), null);
            }
#endif
            else
            {
                throw new WrongPropertyTypeOfTimestampException(pi.Name, pi.PropertyType.Name);
            }
        }

        protected void SetupVersionNoValiableList(IList varList, IPropertyType pt, object bean)
        {
            object value = pt.PropertyInfo.GetValue(bean, null);
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
            int intValue = Convert.ToInt32(value) + 1;
            VersionNo = intValue;
            varList.Add(ConversionUtil.ConvertTargetType(VersionNo, pt.PropertyInfo.PropertyType));
        }

        protected void SetupVersionNoPropertyInfo(PropertyInfo pi, object bean)
        {
            pi.SetValue(bean, ConversionUtil.ConvertTargetType(VersionNo, pi.PropertyType), null);
        }
    }
}
