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
using System.Text;
using Seasar.Dao.Attrs;
using Seasar.Dao.Id;
using Seasar.Extension.ADO;
using Seasar.Framework.Beans;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class BeanMetaDataImpl : DtoMetaDataImpl, IBeanMetaData
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected string tableName;
        protected Hashtable propertyTypesByColumnName = new Hashtable(StringComparer.OrdinalIgnoreCase);
        protected ArrayList relationProeprtyTypes = new ArrayList();
        protected string[] primaryKeys = new string[0];
        protected string autoSelectList;
        protected bool relation;
        protected IIdentifierGenerator identifierGenerator;
        protected string versionNoPropertyName = "VersionNo";
        protected string timestampPropertyName = "Timestamp";
        protected string modifiedPropertyNamesPropertyName = "ModifiedPropertyNames";
        protected string clearModifiedPropertyNamesMethodName = "ClearModifiedPropertyNames";
        protected string versionNoBindingName;
        protected string timestampBindingName;

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
            this.relation = relation;
            AnnotationReaderFactory = annotationReaderFactory;
            Initialize(dbMetaData, dbms);
        }

        public virtual void Initialize(IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            beanAnnotationReader = AnnotationReaderFactory.CreateBeanAnnotationReader(BeanType);
            SetupTableName(BeanType);
            SetupVersionNoPropertyName(BeanType);
            SetupTimestampPropertyName(BeanType);
            SetupProperty(BeanType, dbMetaData, dbms);
            SetupMethodInfo();
            SetupDatabaseMetaData(BeanType, dbMetaData, dbms);
            SetupPropertiesByColumnName();
        }

        protected virtual IAnnotationReaderFactory AnnotationReaderFactory { get; set; }

        #region IBeanMetaData メンバ

        public string TableName => tableName;

        public virtual IPropertyType VersionNoPropertyType => GetPropertyType(versionNoPropertyName);

        public virtual string VersionNoPropertyName => versionNoPropertyName;

        public virtual string VersionNoBindingName => versionNoBindingName;

        public virtual bool HasVersionNoPropertyType => HasPropertyType(versionNoPropertyName);

        public virtual IPropertyType TimestampPropertyType => GetPropertyType(timestampPropertyName);

        public virtual string TimestampPropertyName => timestampPropertyName;

        public virtual string TimestampBindingName => timestampBindingName;

        public virtual bool HasTimestampPropertyType => HasPropertyType(timestampPropertyName);

        public virtual string ModifiedPropertyNamesPropertyName => modifiedPropertyNamesPropertyName;

        public virtual bool HasModifiedPropertyNamesPropertyName => HasPropertyType(modifiedPropertyNamesPropertyName);

        public virtual string ClearModifiedPropertyNamesMethodName => clearModifiedPropertyNamesMethodName;

        public virtual bool HasClearModifiedPropertyNamesMethodName => HasMethodInfo(clearModifiedPropertyNamesMethodName);

        public virtual string ConvertFullColumnName(string alias)
        {
            if (HasPropertyTypeByColumnName(alias))
                return tableName + "." + alias;
            var index = alias.LastIndexOf('_');
            if (index < 0)
                throw new ColumnNotFoundRuntimeException(tableName, alias);
            var columnName = alias.Substring(0, index);
            var relnoStr = alias.Substring(index + 1);
            var relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch (Exception)
            {
                throw new ColumnNotFoundRuntimeException(tableName, alias);
            }
            var rpt = GetRelationPropertyType(relno);
            if (!rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName))
                throw new ColumnNotFoundRuntimeException(tableName, alias);
            return rpt.PropertyName + "." + columnName;
        }

        public virtual IPropertyType GetPropertyTypeByAliasName(string aliasName)
        {
            if (HasPropertyTypeByColumnName(aliasName))
                return GetPropertyTypeByColumnName(aliasName);
            var index = aliasName.LastIndexOf('_');
            if (index < 0)
                throw new ColumnNotFoundRuntimeException(tableName, aliasName);
            var columnName = aliasName.Substring(0, index);
            var relnoStr = aliasName.Substring(index + 1);
            var relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch (Exception)
            {
                throw new ColumnNotFoundRuntimeException(tableName, columnName);
            }
            var rpt = GetRelationPropertyType(relno);
            if (!rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName))
                throw new ColumnNotFoundRuntimeException(tableName, aliasName);
            return rpt.BeanMetaData.GetPropertyTypeByAliasName(columnName);
        }

        public virtual IPropertyType GetPropertyTypeByColumnName(string columnName)
        {
            var propertyType = (IPropertyType) propertyTypesByColumnName[columnName];
            if (propertyType == null)
                throw new ColumnNotFoundRuntimeException(tableName, columnName);

            return propertyType;
        }

        public virtual bool HasPropertyTypeByColumnName(string columnName) => propertyTypesByColumnName[columnName] != null;

        public virtual bool HasPropertyTypeByAliasName(string aliasName)
        {
            if (HasPropertyTypeByColumnName(aliasName)) return true;
            var index = aliasName.LastIndexOf('_');
            if (index < 0) return false;
            var columnName = aliasName.Substring(0, index);
            var relnoStr = aliasName.Substring(index + 1);
            var relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch (Exception)
            {
                return false;
            }
            if (relno >= RelationPropertyTypeSize) return false;
            var rpt = GetRelationPropertyType(relno);
            return rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName);
        }

        public virtual int RelationPropertyTypeSize => relationProeprtyTypes.Count;

        public virtual IRelationPropertyType GetRelationPropertyType(int index) => (IRelationPropertyType) relationProeprtyTypes[index];

        IRelationPropertyType IBeanMetaData.GetRelationPropertyType(string propertyName)
        {
            for (var i = 0; i < RelationPropertyTypeSize; ++i)
            {
                var rpt = (IRelationPropertyType) relationProeprtyTypes[i];
                if (rpt != null
                        && String.Compare(rpt.PropertyName, propertyName, StringComparison.OrdinalIgnoreCase) == 0)
                    return rpt;
            }
            throw new PropertyNotFoundRuntimeException(BeanType, propertyName);
        }

        /// <summary>
        /// entity内の更新フラグをOFFにする。
        /// ClearModifiedPropertyNamesメソッドが存在する場合はそれを呼び出す。
        /// 上記メソッドがなく、かつModifiedPropertyNamesプロパティがある場合は
        /// そのプロパティの値がもつClear()メソッドを呼び出す。
        /// どちらも持たない場合は何もせずに処理を終わる
        /// </summary>
        /// <remarks>DAONET-57</remarks>
        /// <param name="bean">エンティティ</param>
        public virtual void ClearModifiedPropertyNames(object bean)
        {
            if ( HasClearModifiedPropertyNamesMethodName )
            {
                var mi = GetMethodInfo(ClearModifiedPropertyNamesMethodName);
                MethodUtil.Invoke(mi, bean, null);
//                mi.Invoke(bean, null);
            }
            else if ( HasModifiedPropertyNamesPropertyName )
            {
                var pt = GetPropertyType(ModifiedPropertyNamesPropertyName);
//                var modifiedPropertyNames = (IDictionary)pt.PropertyInfo.GetValue(bean, null);
                var modifiedPropertyNames =
                    (IDictionary) PropertyUtil.GetValue(bean, bean.GetExType(), pt.PropertyInfo.Name);
                modifiedPropertyNames.Clear();
            }
        }

        public virtual int PrimaryKeySize => primaryKeys.Length;

        public virtual string GetPrimaryKey(int index) => primaryKeys[index];

        public virtual IIdentifierGenerator IdentifierGenerator => identifierGenerator;

        public virtual string AutoSelectList
        {
            get
            {
                lock (this)
                {
                    if (autoSelectList != null)
                        return autoSelectList;
                    SetupAutoSelectList();
                    return autoSelectList;
                }
            }
        }

        public virtual bool IsRelation => relation;

        public virtual IDictionary GetModifiedPropertyNames(object bean) 
        {
            var propertyNames = modifiedPropertyNamesPropertyName;
            if ( !HasModifiedPropertyNamesPropertyName )
            {
                throw new NotFoundModifiedPropertiesRuntimeException(bean.GetExType().Name, propertyNames);
            }
            var modifiedPropertyType = GetPropertyType(propertyNames);
//            object value = modifiedPropertyType.PropertyInfo.GetValue(bean, null);
            var value = PropertyUtil.GetValue(bean, bean.GetExType(), modifiedPropertyType.PropertyInfo.Name);
            var names = (IDictionary)value;
            return names;
        }

        #endregion

        protected virtual void SetupTableName(Type beanType)
        {
            var ta = beanAnnotationReader.GetTable();
            if (ta != null)
            {
                tableName = ta;
            }
            else
            {
                tableName = beanType.Name;
            }
        }

        protected virtual void SetupVersionNoPropertyName(Type beanType)
        {
            var vna = beanAnnotationReader.GetVersionNoProteryName();
            if (vna != null)
            {
                versionNoPropertyName = vna;
            }

            var i = 0;
            do
            {
                versionNoBindingName = versionNoPropertyName + i++;
            } while (HasPropertyType(versionNoBindingName));
        }

        protected virtual void SetupTimestampPropertyName(Type beanType)
        {
            var tsa = beanAnnotationReader.GetTimestampPropertyName();
            if (tsa != null)
            {
                timestampPropertyName = tsa;
            }

            var i = 0;
            do
            {
                timestampBindingName = timestampPropertyName + i++;
            } while (HasPropertyType(timestampBindingName));

            if (timestampBindingName.Equals(versionNoBindingName))
            {
                timestampBindingName = timestampPropertyName + i++;
            }
        }

        protected virtual void SetupProperty(Type beanType, IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            foreach (var pi in beanType.GetProperties())
            {
                IPropertyType pt = null;
                var relnoAttr = beanAnnotationReader.GetRelnoAttribute(pi);
                if (relnoAttr != null)
                {
                    if (!relation)
                    {
                        var rpt = CreateRelationPropertyType(
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
                    var idAttr = beanAnnotationReader.GetIdAttribute(pi, dbms);
                    if (idAttr != null)
                    {
                        identifierGenerator = IdentifierGeneratorFactory.CreateIdentifierGenerator(
                            pi.Name, dbms, idAttr);
                        if (pt != null)
                        {
                            primaryKeys = new[] { pt.ColumnName };
                            pt.IsPrimaryKey = true;
                        }
                    }
                }
            }
        }

        protected virtual void SetupDatabaseMetaData(Type beanType, IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            SetupPropertyPersistentAndColumnName(beanType, dbMetaData);
            SetupPrimaryKey(dbMetaData, dbms);
        }

        protected virtual void SetupPrimaryKey(IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            if (IdentifierGenerator == null)
            {
                var pkeyList = new ArrayList();
                var primaryKeySet = dbMetaData.GetPrimaryKeySet(tableName);
                for (var i = 0; i < PropertyTypeSize; ++i)
                {
                    var pt = GetPropertyType(i);
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
                primaryKeys = (string[]) pkeyList.ToArray(typeof(string));
                identifierGenerator = IdentifierGeneratorFactory
                    .CreateIdentifierGenerator(null, dbms);
            }
        }

        protected virtual void SetupPropertyPersistentAndColumnName(Type beanType, IDatabaseMetaData dbMetaData)
        {
            var columnSet = dbMetaData.GetColumnSet(tableName);
            if (columnSet == null || columnSet.Count == 0)
            {
                _logger.Log("WDAO0002", new object[] { tableName });
            }
            else
            {
                for (var enu = columnSet.GetEnumerator(); enu.MoveNext(); )
                {
                    var columnName = (string) enu.Current;
                    var noUnderscoreColumnName = columnName.Replace("_", string.Empty);
                    var hasProperty = false;
                    for (var i = 0; i < PropertyTypeSize; ++i)
                    {
                        var pt = GetPropertyType(i);
                        if (String.Compare(pt.ColumnName, columnName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            hasProperty = true;
                        }
                        else
                        {
                            if (String.Compare(pt.ColumnName, noUnderscoreColumnName, StringComparison.OrdinalIgnoreCase) == 0)
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

            var props = beanAnnotationReader.GetNoPersisteneProps();
            if (props != null)
            {
                foreach (var prop in props)
                {
                    var pt = GetPropertyType(prop.Trim());
                    pt.IsPersistent = false;
                }
            }
            for (var i = 0; i < PropertyTypeSize; ++i)
            {
                var pt = GetPropertyType(i);
                if (columnSet == null || !columnSet.Contains(pt.ColumnName))
                {
                    pt.IsPersistent = false;
                }
            }
        }

        protected virtual void SetupPropertiesByColumnName()
        {
            for (var i = 0; i < PropertyTypeSize; ++i)
            {
                var pt = GetPropertyType(i);
                propertyTypesByColumnName[pt.ColumnName] = pt;
            }
        }

        protected virtual IRelationPropertyType CreateRelationPropertyType(Type beanType,
            PropertyInfo propertyInfo, RelnoAttribute relnoAttr,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            var myKeys = new string[0];
            var yourKeys = new string[0];
            var relkeys = beanAnnotationReader.GetRelationKey(propertyInfo);
            if (relkeys != null)
            {
                var myKeyList = new ArrayList();
                var yourKeyList = new ArrayList();
                foreach (var token in relkeys.Split(
                    '\t', '\n', '\r', '\f', ','))
                {
                    var index = token.IndexOf(':');
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
            var rpt = CreateRelationPropertyType(propertyInfo,
                                          relnoAttr, myKeys, yourKeys, dbMetaData, dbms);
            return rpt;
        }

        protected virtual IRelationPropertyType CreateRelationPropertyType(PropertyInfo propertyInfo,
            RelnoAttribute relnoAttr, string[] myKeys, string[] yourKeys,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            var relationBmd = CreateRelationBeanMetaData(propertyInfo, dbMetaData, dbms);
            return new RelationPropertyTypeImpl(propertyInfo, relnoAttr.Relno, myKeys, yourKeys, relationBmd);
        }

        protected virtual IBeanMetaData CreateRelationBeanMetaData(PropertyInfo propertyInfo, IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            var bmdImpl = new BeanMetaDataImpl(propertyInfo.PropertyType, dbMetaData, dbms, AnnotationReaderFactory, true);
            return bmdImpl;
        }

        protected virtual void AddRelationPropertyType(IRelationPropertyType rpt)
        {
            for (var i = relationProeprtyTypes.Count; i <= rpt.RelationNo; ++i)
            {
                relationProeprtyTypes.Add(null);
            }
            relationProeprtyTypes[rpt.RelationNo] = rpt;
        }

        protected virtual void SetupAutoSelectList()
        {
            var buf = new StringBuilder(100);
            buf.Append("SELECT ");
            for (var i = 0; i < PropertyTypeSize; ++i)
            {
                var pt = GetPropertyType(i);
                if (pt.IsPersistent)
                {
                    buf.Append(tableName);
                    buf.Append(".");
                    buf.Append(pt.ColumnName);
                    Console.WriteLine(pt.ColumnName);
                    buf.Append(", ");
                }
            }
            foreach (IRelationPropertyType rpt in relationProeprtyTypes)
            {
                var bmd = rpt.BeanMetaData;
                for (var i = 0; i < bmd.PropertyTypeSize; ++i)
                {
                    var pt = bmd.GetPropertyType(i);
                    if (pt.IsPersistent)
                    {
                        var columnName = pt.ColumnName;
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
            autoSelectList = buf.ToString();
        }
    }
}
