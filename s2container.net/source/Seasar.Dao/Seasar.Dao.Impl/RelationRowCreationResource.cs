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
        private IDataReader dataReader;

        /** Relation row. Initialized at first or initialied after. */
        private Object row;

        /** Relation property type. */
        private IRelationPropertyType relationPropertyType;

        /** The set of column name. */
        private System.Collections.IList columnNames;

        /** The map of rel key values. */
        private System.Collections.Hashtable relKeyValues;

        /** The map of relation property cache. */
        private IDictionary<String, IDictionary<String, IPropertyType>> relationPropertyCache;

        /** The suffix of base object. */
        private String baseSuffix;

        /** The suffix of relation no. */
        private String relationNoSuffix;

        /** The limit of relation nest leve. */
        private int limitRelationNestLevel;

        /** The current relation nest level. Default is one. */
        private int currentRelationNestLevel;

        /** Current property type. This variable is temporary. */
        private IPropertyType currentPropertyType;

        /** The temporary variable for relation property type. */
        private IRelationPropertyType tmpRelationPropertyType;

        /** The temporary variable for base suffix. */
        private String tmpBaseSuffix;

        /** The temporary variable for relation no suffix. */
        private String tmpRelationNoSuffix;

        /** The count of valid value. */
        private int validValueCount;

        /** Does it create dead link? */
        private bool createDeadLink;

        // ===================================================================================
        //                                                                            Behavior
        //                                                                            ========
        // -----------------------------------------------------
        //                                                   row
        //                                                   ---
        public bool HasRowInstance() {
            return row != null;
        }

        public void ClearRowInstance() {
            row = null;
        }

        // -----------------------------------------------------
        //                                  relationPropertyType
        //                                  --------------------
        public IBeanMetaData GetRelationBeanMetaData() {
            return relationPropertyType.BeanMetaData;
        }

        public bool HasNextRelationProperty() {
            return GetRelationBeanMetaData().RelationPropertyTypeSize > 0;
        }

        public void BackupRelationPropertyType() {
            tmpRelationPropertyType = relationPropertyType;
        }

        public void RestoreRelationPropertyType() {
            relationPropertyType = tmpRelationPropertyType;
        }

        // -----------------------------------------------------
        //                                           columnNames
        //                                           -----------
        public bool ContainsColumnName(String columnName) {
            return columnNames.Contains(columnName);
        }

        // -----------------------------------------------------
        //                                          relKeyValues
        //                                          ------------
        public bool ExistsRelKeyValues() {
            return relKeyValues != null;
        }

        public bool ContainsRelKeyValue(String key) {
            return relKeyValues.ContainsKey(key);
        }

        public bool ContainsRelKeyValueIfExists(String key) {
            return ExistsRelKeyValues() && relKeyValues.ContainsKey(key);
        }

        public Object ExtractRelKeyValue(String key) {
            return relKeyValues[key];
        }

        // -----------------------------------------------------
        //                                 relationPropertyCache
        //                                 ---------------------
        public void InitializePropertyCacheElement() {
            if (!relationPropertyCache.ContainsKey(relationNoSuffix)) {
                relationPropertyCache.Add(relationNoSuffix, new Dictionary<String, IPropertyType>());
            }
        }

        public bool HasPropertyCacheElement() {
            IDictionary<String, IPropertyType> propertyCacheElement = ExtractPropertyCacheElement();
            return propertyCacheElement != null && propertyCacheElement.Count > 0;
        }

        public IDictionary<String, IPropertyType> ExtractPropertyCacheElement() {
            return relationPropertyCache[relationNoSuffix];
        }

        public void SavePropertyCacheElement() {
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
        public String BuildRelationColumnName() {
            return currentPropertyType.ColumnName + relationNoSuffix;
        }

        public void AddRelationNoSuffix(String AdditionalRelationNoSuffix) {
            relationNoSuffix = relationNoSuffix + AdditionalRelationNoSuffix;
        }

        public void BackupSuffixAndPrepare(String baseSuffix,
                String additionalRelationNoSuffix) {
            BackupBaseSuffix();
            BackupRelationNoSuffix();
            this.baseSuffix = baseSuffix;
            AddRelationNoSuffix(additionalRelationNoSuffix);
        }

        public void RestoreSuffix() {
            RestoreBaseSuffix();
            RestoreRelationNoSuffix();
        }

        private void BackupBaseSuffix() {
            tmpBaseSuffix = baseSuffix;
        }

        private void RestoreBaseSuffix() {
            baseSuffix = tmpBaseSuffix;
        }

        private void BackupRelationNoSuffix() {
            tmpRelationNoSuffix = relationNoSuffix;
        }

        private void RestoreRelationNoSuffix() {
            relationNoSuffix = tmpRelationNoSuffix;
        }

        // -----------------------------------------------------
        //                                     relationNestLevel
        //                                     -----------------
        public bool HasNextRelationLevel() {
            return currentRelationNestLevel < limitRelationNestLevel;
        }

        public void IncrementCurrentRelationNestLevel() {
            ++currentRelationNestLevel;
        }

        public void DecrementCurrentRelationNestLevel() {
            --currentRelationNestLevel;
        }

        // -----------------------------------------------------
        //                                       validValueCount
        //                                       ---------------
        public void IncrementValidValueCount() {
            ++validValueCount;
        }

        public void ClearValidValueCount() {
            validValueCount = 0;
        }

        public bool HasValidValueCount() {
            return validValueCount > 0;
        }

        // ===================================================================================
        //                                                                            Accessor
        //                                                                            ========
        public IDataReader DataReader {
            get { return dataReader; }
            set { dataReader = value; }
        }

        public Object Row {
            get { return row; }
            set { row = value; }
        }

        public IRelationPropertyType RelationPropertyType {
            get { return relationPropertyType; }
            set { relationPropertyType = value; }
        }

        public System.Collections.IList ColumnNames {
            get { return columnNames; }
            set { columnNames = value; }
        }

        public System.Collections.Hashtable RelKeyValues {
            get { return relKeyValues; }
            set { relKeyValues = value; }
        }

        public IDictionary<String, IDictionary<String, IPropertyType>> RelationPropertyCache {
            get { return relationPropertyCache; }
            set { relationPropertyCache = value; }
        }

        public String BaseSuffix {
            get { return baseSuffix; }
            set { baseSuffix = value; }
        }

        public String RelationNoSuffix {
            get { return relationNoSuffix; }
            set { relationNoSuffix = value; }
        }

        public int LimitRelationNestLevel {
            get { return limitRelationNestLevel; }
            set { limitRelationNestLevel = value; }
        }

        public int CurrentRelationNestLevel {
            get { return currentRelationNestLevel; }
            set { currentRelationNestLevel = value; }
        }

        public IPropertyType CurrentPropertyType {
            get { return currentPropertyType; }
            set { currentPropertyType = value; }
        }

        public int ValidValueCount {
            get { return validValueCount; }
            set { validValueCount = value; }
        }

        public bool IsCreateDeadLink {
            get { return createDeadLink; }
            set { createDeadLink = value; }
        }
    }
}
