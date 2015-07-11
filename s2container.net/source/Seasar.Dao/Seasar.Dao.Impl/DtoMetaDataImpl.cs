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
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Beans;

namespace Seasar.Dao.Impl
{
    public class DtoMetaDataImpl : IDtoMetaData
    {
        private Type _beanType;

        private readonly Hashtable _propertyTypes = new Hashtable(
            StringComparer.OrdinalIgnoreCase);

        private readonly Hashtable _methodInfos = new Hashtable(
            StringComparer.OrdinalIgnoreCase);

        protected IBeanAnnotationReader _beanAnnotationReader;

        protected DtoMetaDataImpl()
        {
        }

        public DtoMetaDataImpl(Type beanType, IBeanAnnotationReader beanAnnotationReader)
        {
            BeanType = beanType;
            BeanAnnotationReader = beanAnnotationReader;
            Initialize();
        }

        public virtual void Initialize()
        {
            SetupPropertyType();
            SetupMethodInfo();
        }

        #region IDtoMetaData メンバ

        public Type BeanType
        {
            get
            {
                return _beanType;
            }
            set
            {
                _beanType = value;
            }
        }

        public int PropertyTypeSize
        {
            get
            {
                return _propertyTypes.Count;
            }
        }

        public int MethodInfoSize
        {
            get
            {
                return _methodInfos.Count;
            }
        }

        public IBeanAnnotationReader BeanAnnotationReader
        {
            set { _beanAnnotationReader = value; }
        }

        public IPropertyType GetPropertyType(int index)
        {
            IEnumerator enu = _propertyTypes.Keys.GetEnumerator();
            for (int i = -1; i < index; ++i) enu.MoveNext();
            return (IPropertyType) _propertyTypes[enu.Current];
        }

        public MethodInfo GetMethodInfo(int index)
        {
            IEnumerator enu = _methodInfos.Keys.GetEnumerator();
            for ( int i = -1; i < index; ++i ) enu.MoveNext();
            return (MethodInfo)_methodInfos[enu.Current];
        }

        public IPropertyType GetPropertyType(string propertyName)
        {
            IPropertyType propertyType = (IPropertyType) _propertyTypes[propertyName];
            if (propertyType == null)
                throw new PropertyNotFoundRuntimeException(_beanType, propertyName);
            return propertyType;
        }

        public MethodInfo GetMethodInfo(string methodName)
        {
            MethodInfo methodInfo = (MethodInfo)_methodInfos[methodName];
            if ( methodInfo == null )
                throw new MethodNotFoundRuntimeException(_beanType, methodName, null);
            return methodInfo;
        }

        public bool HasPropertyType(string propertyName)
        {
            return _propertyTypes.Contains(propertyName);
        }

        public bool HasMethodInfo(string methodName)
        {
            return _methodInfos.Contains(methodName);
        }

        #endregion

        protected virtual void SetupPropertyType()
        {
            foreach (PropertyInfo pi in _beanType.GetProperties())
            {
                IPropertyType pt = CreatePropertyType(pi);
                AddPropertyType(pt);
            }
        }

        protected virtual void SetupMethodInfo()
        {
            foreach ( MethodInfo mi in _beanType.GetMethods() )
            {
                AddMethodInfo(mi);
            }
        }

        protected virtual IPropertyType CreatePropertyType(PropertyInfo pi)
        {
            string columnName = _beanAnnotationReader.GetColumn(pi);
            if (columnName == null)
            {
                columnName = pi.Name;
            }
            IValueType valueType = ValueTypes.GetValueType(pi.PropertyType);
            IPropertyType pt = new PropertyTypeImpl(pi, valueType, columnName);
            return pt;
        }

        protected virtual void AddPropertyType(IPropertyType propertyType)
        {
            _propertyTypes.Add(propertyType.PropertyName, propertyType);
        }

        protected virtual void AddMethodInfo(MethodInfo methodInfo)
        {
            //  引数をもたないメソッド情報のみ保持
            if ( methodInfo.GetParameters().Length == 0 )
            {
                _methodInfos.Add(methodInfo.Name, methodInfo);
            }
        }
    }
}
