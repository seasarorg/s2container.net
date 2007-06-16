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
using System.Reflection;
using System.Text;
using Seasar.Dao.Attrs;
using Seasar.Dao.Id;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Framework.Beans;
using Seasar.Framework.Log;

namespace Seasar.Dao.Impl
{
    public class BeanMetaDataImpl : DtoMetaDataImpl, IBeanMetaData
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected string _tableName;

#if NET_1_1
        private Hashtable _propertyTypesByColumnName = new Hashtable(
            CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
#else
        protected Hashtable _propertyTypesByColumnName =
            new Hashtable(StringComparer.OrdinalIgnoreCase);
#endif

        protected ArrayList _relationProeprtyTypes = new ArrayList();
        protected string[] _primaryKeys = new string[0];
        protected string _autoSelectList;
        protected bool _relation;
        protected IIdentifierGenerator _identifierGenerator;
        protected string _versionNoPropertyName = "VersionNo";
        protected string _timestampPropertyName = "Timestamp";
        protected string _versionNoBindingName;
        protected string _timestampBindingName;
        private IAnnotationReaderFactory _annotationReaderFactory;

        public BeanMetaDataImpl()
        {
        }

        public BeanMetaDataImpl(Type beanType, IDatabaseMetaData dbMetaData,
            IDbms dbms, IAnnotationReaderFactory annotationReaderFactory)
            : this(beanType, dbMetaData, dbms, annotationReaderFactory, false)
        {
        }

        public BeanMetaDataImpl(Type beanType, IDatabaseMetaData dbMetaData,
            IDbms dbms, IAnnotationReaderFactory annotationReaderFactory, bool relation)
        {
            BeanType = beanType;
            _relation = relation;
            AnnotationReaderFactory = annotationReaderFactory;
            Initialize(dbMetaData, dbms);
        }

        public void Initialize(IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            _beanAnnotationReader = AnnotationReaderFactory.CreateBeanAnnotationReader(BeanType);
            SetupTableName(BeanType);
            SetupVersionNoPropertyName(BeanType);
            SetupTimestampPropertyName(BeanType);
            SetupProperty(BeanType, dbMetaData, dbms);
            SetupDatabaseMetaData(BeanType, dbMetaData, dbms);
            SetupPropertiesByColumnName();
        }

        protected IAnnotationReaderFactory AnnotationReaderFactory
        {
            get { return _annotationReaderFactory; }
            set { _annotationReaderFactory = value; }
        }

        #region IBeanMetaData ÉÅÉìÉo

        public string TableName
        {
            get { return _tableName; }
        }

        public IPropertyType VersionNoPropertyType
        {
            get { return GetPropertyType(_versionNoPropertyName); }
        }

        public string VersionNoPropertyName
        {
            get { return _versionNoPropertyName; }
        }

        public string VersionNoBindingName
        {
            get { return _versionNoBindingName; }
        }

        public bool HasVersionNoPropertyType
        {
            get { return HasPropertyType(_versionNoPropertyName); }
        }

        public IPropertyType TimestampPropertyType
        {
            get { return GetPropertyType(_timestampPropertyName); }
        }

        public string TimestampPropertyName
        {
            get { return _timestampPropertyName; }
        }

        public string TimestampBindingName
        {
            get { return _timestampBindingName; }
        }

        public bool HasTimestampPropertyType
        {
            get { return HasPropertyType(_timestampPropertyName); }
        }

        public string ConvertFullColumnName(string alias)
        {
            if (HasPropertyTypeByColumnName(alias))
                return _tableName + "." + alias;
            int index = alias.LastIndexOf('_');
            if (index < 0)
                throw new ColumnNotFoundRuntimeException(_tableName, alias);
            string columnName = alias.Substring(0, index);
            string relnoStr = alias.Substring(index + 1);
            int relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch (Exception)
            {
                throw new ColumnNotFoundRuntimeException(_tableName, alias);
            }
            IRelationPropertyType rpt = GetRelationPropertyType(relno);
            if (!rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName))
                throw new ColumnNotFoundRuntimeException(_tableName, alias);
            return rpt.PropertyName + "." + columnName;
        }

        public IPropertyType GetPropertyTypeByAliasName(string aliasName)
        {
            if (HasPropertyTypeByColumnName(aliasName))
                return GetPropertyTypeByColumnName(aliasName);
            int index = aliasName.LastIndexOf('_');
            if (index < 0)
                throw new ColumnNotFoundRuntimeException(_tableName, aliasName);
            string columnName = aliasName.Substring(0, index);
            string relnoStr = aliasName.Substring(index + 1);
            int relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch (Exception)
            {
                throw new ColumnNotFoundRuntimeException(_tableName, columnName);
            }
            IRelationPropertyType rpt = GetRelationPropertyType(relno);
            if (!rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName))
                throw new ColumnNotFoundRuntimeException(_tableName, aliasName);
            return rpt.BeanMetaData.GetPropertyTypeByAliasName(columnName);
        }

        public IPropertyType GetPropertyTypeByColumnName(string columnName)
        {
            IPropertyType propertyType = (IPropertyType) _propertyTypesByColumnName[columnName];
            if (propertyType == null)
                throw new ColumnNotFoundRuntimeException(_tableName, columnName);

            return propertyType;
        }

        public bool HasPropertyTypeByColumnName(string columnName)
        {
            return _propertyTypesByColumnName[columnName] != null;
        }

        public bool HasPropertyTypeByAliasName(string aliasName)
        {
            if (HasPropertyTypeByColumnName(aliasName)) return true;
            int index = aliasName.LastIndexOf('_');
            if (index < 0) return false;
            string columnName = aliasName.Substring(0, index);
            string relnoStr = aliasName.Substring(index + 1);
            int relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch (Exception)
            {
                return false;
            }
            if (relno >= RelationPropertyTypeSize) return false;
            IRelationPropertyType rpt = GetRelationPropertyType(relno);
            return rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName);
        }

        public int RelationPropertyTypeSize
        {
            get { return _relationProeprtyTypes.Count; }
        }

        public IRelationPropertyType GetRelationPropertyType(int index)
        {
            return (IRelationPropertyType) _relationProeprtyTypes[index];
        }

        IRelationPropertyType IBeanMetaData.GetRelationPropertyType(string propertyName)
        {
            for (int i = 0; i < RelationPropertyTypeSize; ++i)
            {
                IRelationPropertyType rpt = (IRelationPropertyType) _relationProeprtyTypes[i];
                if (rpt != null
                        && string.Compare(rpt.PropertyName, propertyName, true) == 0)
                    return rpt;
            }
            throw new PropertyNotFoundRuntimeException(BeanType, propertyName);
        }

        public int PrimaryKeySize
        {
            get { return _primaryKeys.Length; }
        }

        public string GetPrimaryKey(int index)
        {
            return _primaryKeys[index];
        }

        public IIdentifierGenerator IdentifierGenerator
        {
            get { return _identifierGenerator; }
        }

        public string AutoSelectList
        {
            get
            {
                lock (this)
                {
                    if (_autoSelectList != null)
                        return _autoSelectList;
                    SetupAutoSelectList();
                    return _autoSelectList;
                }
            }
        }

        public bool IsRelation
        {
            get { return _relation; }
        }

        #endregion

        protected virtual void SetupTableName(Type beanType)
        {
            string ta = _beanAnnotationReader.GetTable();
            if (ta != null)
            {
                _tableName = ta;
            }
            else
            {
                _tableName = beanType.Name;
            }
        }

        protected virtual void SetupVersionNoPropertyName(Type beanType)
        {
            string vna = _beanAnnotationReader.GetVersionNoProteryName();
            if (vna != null)
            {
                _versionNoPropertyName = vna;
            }

            int i = 0;
            do
            {
                _versionNoBindingName = _versionNoPropertyName + i++;
            } while (HasPropertyType(_versionNoBindingName));
        }

        protected virtual void SetupTimestampPropertyName(Type beanType)
        {
            string tsa = _beanAnnotationReader.GetTimestampPropertyName();
            if (tsa != null)
            {
                _timestampPropertyName = tsa;
            }

            int i = 0;
            do
            {
                _timestampBindingName = _timestampPropertyName + i++;
            } while (HasPropertyType(_timestampBindingName));

            if (_timestampBindingName.Equals(_versionNoBindingName))
            {
                _timestampBindingName = _timestampPropertyName + i++;
            }
        }

        protected virtual void SetupProperty(Type beanType, IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            foreach (PropertyInfo pi in beanType.GetProperties())
            {
                IPropertyType pt = null;
                RelnoAttribute relnoAttr = _beanAnnotationReader.GetRelnoAttribute(pi);
                if (relnoAttr != null)
                {
                    if (!_relation)
                    {
                        IRelationPropertyType rpt = CreateRelationPropertyType(
                            beanType, pi, relnoAttr, dbMetaData, dbms);
                        AddRelationPropertyType(rpt);
                    }
                }
                else
                {
                    pt = CreatePropertyType(pi);
                    AddPropertyType(pt);
                }
                if (IdentifierGenerator == null)
                {
                    IDAttribute idAttr = _beanAnnotationReader.GetIdAttribute(pi, dbms);
                    if (idAttr != null)
                    {
                        _identifierGenerator = IdentifierGeneratorFactory.CreateIdentifierGenerator(
                            pi.Name, dbms, idAttr);
                        if (pt != null)
                        {
                            _primaryKeys = new string[] { pt.ColumnName };
                            pt.IsPrimaryKey = true;
                        }
                    }
                }
            }
        }

        protected virtual void SetupDatabaseMetaData(Type beanType,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            SetupPropertyPersistentAndColumnName(beanType, dbMetaData);
            SetupPrimaryKey(dbMetaData, dbms);
        }

        protected virtual void SetupPrimaryKey(IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            if (IdentifierGenerator == null)
            {
                ArrayList pkeyList = new ArrayList();
                IList primaryKeySet = dbMetaData.GetPrimaryKeySet(_tableName);
                for (int i = 0; i < PropertyTypeSize; ++i)
                {
                    IPropertyType pt = GetPropertyType(i);
                    if (primaryKeySet != null && primaryKeySet.Contains(pt.ColumnName))
                    {
                        pt.IsPrimaryKey = true;
                        pkeyList.Add(pt.ColumnName);
                    }
                    else
                    {
                        pt.IsPrimaryKey = false;
                    }
                }
                _primaryKeys = (string[]) pkeyList.ToArray(typeof(string));
                _identifierGenerator = IdentifierGeneratorFactory
                    .CreateIdentifierGenerator(null, dbms);
            }
        }

        protected virtual void SetupPropertyPersistentAndColumnName(Type beanType,
            IDatabaseMetaData dbMetaData)
        {
            IList columnSet = dbMetaData.GetColumnSet(_tableName);
            if (columnSet == null || columnSet.Count == 0)
            {
                _logger.Log("WDAO0002", new object[] { _tableName });
            }
            else
            {
                for (IEnumerator enu = columnSet.GetEnumerator(); enu.MoveNext(); )
                {
                    string columnName = (string) enu.Current;
                    string noUnderscoreColumnName = columnName.Replace("_", string.Empty);
                    bool hasProperty = false;
                    for (int i = 0; i < PropertyTypeSize; ++i)
                    {
                        IPropertyType pt = GetPropertyType(i);
                        if (string.Compare(pt.ColumnName, columnName, true) == 0)
                        {
                            hasProperty = true;
                        }
                        else
                        {
                            if (string.Compare(pt.ColumnName, noUnderscoreColumnName, true) == 0)
                            {
                                hasProperty = true;
                            }
                        }
                        if (hasProperty)
                        {
                            pt.ColumnName = columnName;
                            break;
                        }
                    }
                }
            }

            string[] props = _beanAnnotationReader.GetNoPersisteneProps();
            if (props != null)
            {
                foreach (string prop in props)
                {
                    IPropertyType pt = GetPropertyType(prop.Trim());
                    pt.IsPersistent = false;
                }
            }
            for (int i = 0; i < PropertyTypeSize; ++i)
            {
                IPropertyType pt = GetPropertyType(i);
                if (columnSet == null || !columnSet.Contains(pt.ColumnName))
                {
                    pt.IsPersistent = false;
                }
            }
        }

        protected virtual void SetupPropertiesByColumnName()
        {
            for (int i = 0; i < PropertyTypeSize; ++i)
            {
                IPropertyType pt = GetPropertyType(i);
                _propertyTypesByColumnName[pt.ColumnName] = pt;
            }
        }

        protected virtual IRelationPropertyType CreateRelationPropertyType(Type beanType,
            PropertyInfo propertyInfo, RelnoAttribute relnoAttr,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            string[] myKeys = new string[0];
            string[] yourKeys = new string[0];
            string relkeys = _beanAnnotationReader.GetRelationKey(propertyInfo);
            if (relkeys != null)
            {
                ArrayList myKeyList = new ArrayList();
                ArrayList yourKeyList = new ArrayList();
                foreach (string token in relkeys.Split(
                    '\t', '\n', '\r', '\f', ','))
                {
                    int index = token.IndexOf(':');
                    if (index > 0)
                    {
                        myKeyList.Add(token.Substring(0, index));
                        yourKeyList.Add(token.Substring(index + 1));
                    }
                    else
                    {
                        myKeyList.Add(token);
                        yourKeyList.Add(token);
                    }
                }
                myKeys = (string[]) myKeyList.ToArray(typeof(string));
                yourKeys = (string[]) yourKeyList.ToArray(typeof(string));
            }
            IRelationPropertyType rpt = CreateRelationPropertyType(propertyInfo,
                                          relnoAttr, myKeys, yourKeys, dbMetaData, dbms);
            return rpt;
        }

        protected virtual IRelationPropertyType CreateRelationPropertyType(PropertyInfo propertyInfo,
            RelnoAttribute relnoAttr, string[] myKeys, string[] yourKeys,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            IBeanMetaData relationBmd = CreateRelationBeanMetaData(propertyInfo, dbMetaData, dbms);
            return new RelationPropertyTypeImpl(propertyInfo, relnoAttr.Relno, myKeys, yourKeys, relationBmd);
        }

        protected virtual IBeanMetaData CreateRelationBeanMetaData(PropertyInfo propertyInfo, IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            BeanMetaDataImpl bmdImpl = new BeanMetaDataImpl(propertyInfo.PropertyType, dbMetaData, dbms, AnnotationReaderFactory, true);
            return bmdImpl;
        }

        protected virtual void AddRelationPropertyType(IRelationPropertyType rpt)
        {
            for (int i = _relationProeprtyTypes.Count; i <= rpt.RelationNo; ++i)
            {
                _relationProeprtyTypes.Add(null);
            }
            _relationProeprtyTypes[rpt.RelationNo] = rpt;
        }

        protected virtual void SetupAutoSelectList()
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append("SELECT ");
            for (int i = 0; i < PropertyTypeSize; ++i)
            {
                IPropertyType pt = GetPropertyType(i);
                if (pt.IsPersistent)
                {
                    buf.Append(_tableName);
                    buf.Append(".");
                    buf.Append(pt.ColumnName);
                    buf.Append(", ");
                }
            }
            foreach (IRelationPropertyType rpt in _relationProeprtyTypes)
            {
                IBeanMetaData bmd = rpt.BeanMetaData;
                for (int i = 0; i < bmd.PropertyTypeSize; ++i)
                {
                    IPropertyType pt = bmd.GetPropertyType(i);
                    if (pt.IsPersistent)
                    {
                        string columnName = pt.ColumnName;
                        buf.Append(rpt.PropertyName);
                        buf.Append(".");
                        buf.Append(columnName);
                        buf.Append(" AS ");
                        buf.Append(pt.ColumnName).Append("_");
                        buf.Append(rpt.RelationNo);
                        buf.Append(", ");
                    }
                }
            }
            buf.Length = buf.Length - 2;
            _autoSelectList = buf.ToString();
        }
    }
}
