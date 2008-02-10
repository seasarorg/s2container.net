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
using System.Collections.Generic;
using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public class RelationRowCreationResource
    {
        // ===================================================================================
        //                                                                           Attribute
        //                                                                           =========
        /** Data reader. */
        protected IDataReader dataReader;

        /** Relation row. Initialized at first or initialied after. */
        protected Object row;

        /** Relation property type. */
        protected IRelationPropertyType relationPropertyType;

        /** The set of column name. */
        protected System.Collections.IList columnNames;

        /** The map of relation key values. */
        protected System.Collections.Hashtable relKeyValues;

        /** The map of relation property cache. */
        protected IDictionary<String, IDictionary<String, IPropertyType>> relationPropertyCache;

        /** The suffix of base object. */
        protected String baseSuffix;

        /** The suffix of relation no. */
        protected String relationNoSuffix;

        /** The limit of relation nest leve. */
        protected int limitRelationNestLevel;

        /** The current relation nest level. Default is one. */
        protected int currentRelationNestLevel;

        /** Current property type. This variable is temporary. */
        protected IPropertyType currentPropertyType;

        /** The count of valid value. */
        protected int validValueCount;

        /** Does it create dead link? */
        protected bool createDeadLink;

        /** The backup of relation property type. */
        protected Stack<IRelationPropertyType> relationPropertyTypeBackup;

        /** The backup of base suffix. */
        protected Stack<String> baseSuffixBackup;

        /** The backup of relation suffix. */
        protected Stack<String> relationNoSuffixBackup;

        // ===================================================================================
        //                                                                            Behavior
        //                                                                            ========
        // -----------------------------------------------------
        //                                                   row
        //                                                   ---
        public virtual bool HasRowInstance() {
            return row != null;
        }

        public virtual void ClearRowInstance() {
            row = null;
        }

        // -----------------------------------------------------
        //                                  relationPropertyType
        //                                  --------------------
        public virtual IBeanMetaData GetRelationBeanMetaData() {
            return relationPropertyType.BeanMetaData;
        }

        public virtual bool HasNextRelationProperty() {
            return GetRelationBeanMetaData().RelationPropertyTypeSize > 0;
        }

        public virtual void BackupRelationPropertyType() {
            GetOrCreateRelationPropertyTypeBackup().Push(RelationPropertyType);
        }

        public virtual void RestoreRelationPropertyType() {
            RelationPropertyType = GetOrCreateRelationPropertyTypeBackup().Pop();
        }

        public virtual Stack<IRelationPropertyType> GetOrCreateRelationPropertyTypeBackup() {
            if (relationPropertyTypeBackup == null) {
                relationPropertyTypeBackup = new Stack<IRelationPropertyType>();
            }
            return relationPropertyTypeBackup;
        }

        // -----------------------------------------------------
        //                                           columnNames
        //                                           -----------
        public virtual bool ContainsColumnName(String columnName) {
            return columnNames.Contains(columnName);
        }

        // -----------------------------------------------------
        //                                          relKeyValues
        //                                          ------------
        public virtual bool ExistsRelKeyValues() {
            return relKeyValues != null;
        }

        public virtual bool ContainsRelKeyValue(String key) {
            return relKeyValues.ContainsKey(key);
        }

        public virtual bool ContainsRelKeyValueIfExists(String key) {
            return ExistsRelKeyValues() && relKeyValues.ContainsKey(key);
        }

        public virtual Object ExtractRelKeyValue(String key) {
            return relKeyValues[key];
        }

        // -----------------------------------------------------
        //                                 relationPropertyCache
        //                                 ---------------------
        public virtual void InitializePropertyCacheElement() {
            if (!relationPropertyCache.ContainsKey(relationNoSuffix)) {
                relationPropertyCache.Add(relationNoSuffix, new Dictionary<String, IPropertyType>());
            }
        }

        public virtual bool HasPropertyCacheElement() {
            IDictionary<String, IPropertyType> propertyCacheElement = ExtractPropertyCacheElement();
            return propertyCacheElement != null && propertyCacheElement.Count > 0;
        }

        public virtual IDictionary<String, IPropertyType> ExtractPropertyCacheElement() {
            return relationPropertyCache[relationNoSuffix];
        }

        public virtual void SavePropertyCacheElement() {
            if (!HasPropertyCacheElement()) {
                InitializePropertyCacheElement();
            }
            IDictionary<String, IPropertyType> propertyCacheElement = ExtractPropertyCacheElement();
            String columnName = BuildRelationColumnName();
            if (propertyCacheElement.ContainsKey(columnName)) {
                return;
            }
            if (!propertyCacheElement.ContainsKey(columnName)) {
                propertyCacheElement.Add(columnName, currentPropertyType);
            }
        }

        // -----------------------------------------------------
        //                                                suffix
        //                                                ------
        public virtual String BuildRelationColumnName() {
            return currentPropertyType.ColumnName + relationNoSuffix;
        }

        public virtual void AddRelationNoSuffix(String AdditionalRelationNoSuffix) {
            relationNoSuffix = relationNoSuffix + AdditionalRelationNoSuffix;
        }

        public virtual void BackupSuffixAndPrepare(String baseSuffix,
                String additionalRelationNoSuffix) {
            BackupBaseSuffix();
            BackupRelationNoSuffix();
            this.baseSuffix = baseSuffix;
            AddRelationNoSuffix(additionalRelationNoSuffix);
        }

        public virtual void RestoreSuffix() {
            RestoreBaseSuffix();
            RestoreRelationNoSuffix();
        }

        protected virtual void BackupBaseSuffix() {
            GetOrCreateBaseSuffixBackup().Push(BaseSuffix);
        }

        protected virtual void RestoreBaseSuffix() {
            BaseSuffix = GetOrCreateBaseSuffixBackup().Pop();
        }

        public virtual Stack<String> GetOrCreateBaseSuffixBackup() {
            if (baseSuffixBackup == null) {
                baseSuffixBackup = new Stack<String>();
            }
            return baseSuffixBackup;
        }

        protected virtual void BackupRelationNoSuffix() {
            GetOrCreateRelationNoSuffixBackup().Push(RelationNoSuffix);
        }

        protected virtual void RestoreRelationNoSuffix() {
            RelationNoSuffix = GetOrCreateRelationNoSuffixBackup().Pop();
        }

        public virtual Stack<String> GetOrCreateRelationNoSuffixBackup() {
            if (relationNoSuffixBackup == null) {
                relationNoSuffixBackup = new Stack<String>();
            }
            return relationNoSuffixBackup;
        }

        // -----------------------------------------------------
        //                                     relationNestLevel
        //                                     -----------------
        public virtual bool HasNextRelationLevel() {
            return currentRelationNestLevel < limitRelationNestLevel;
        }

        public virtual void IncrementCurrentRelationNestLevel() {
            ++currentRelationNestLevel;
        }

        public virtual void DecrementCurrentRelationNestLevel() {
            --currentRelationNestLevel;
        }

        // -----------------------------------------------------
        //                                       validValueCount
        //                                       ---------------
        public virtual void IncrementValidValueCount() {
            ++validValueCount;
        }

        public virtual void ClearValidValueCount() {
            validValueCount = 0;
        }

        public virtual bool HasValidValueCount() {
            return validValueCount > 0;
        }

        // ===================================================================================
        //                                                                            Accessor
        //                                                                            ========
        public virtual IDataReader DataReader {
            get { return dataReader; }
            set { dataReader = value; }
        }

        public virtual Object Row {
            get { return row; }
            set { row = value; }
        }

        public virtual IRelationPropertyType RelationPropertyType {
            get { return relationPropertyType; }
            set { relationPropertyType = value; }
        }

        public virtual System.Collections.IList ColumnNames {
            get { return columnNames; }
            set { columnNames = value; }
        }

        public virtual System.Collections.Hashtable RelKeyValues {
            get { return relKeyValues; }
            set { relKeyValues = value; }
        }

        public virtual IDictionary<String, IDictionary<String, IPropertyType>> RelationPropertyCache {
            get { return relationPropertyCache; }
            set { relationPropertyCache = value; }
        }

        public virtual String BaseSuffix {
            get { return baseSuffix; }
            set { baseSuffix = value; }
        }

        public virtual String RelationNoSuffix {
            get { return relationNoSuffix; }
            set { relationNoSuffix = value; }
        }

        public virtual int LimitRelationNestLevel {
            get { return limitRelationNestLevel; }
            set { limitRelationNestLevel = value; }
        }

        public virtual int CurrentRelationNestLevel {
            get { return currentRelationNestLevel; }
            set { currentRelationNestLevel = value; }
        }

        public virtual IPropertyType CurrentPropertyType {
            get { return currentPropertyType; }
            set { currentPropertyType = value; }
        }

        public virtual int ValidValueCount {
            get { return validValueCount; }
            set { validValueCount = value; }
        }

        public virtual bool IsCreateDeadLink {
            get { return createDeadLink; }
            set { createDeadLink = value; }
        }
    }
}
