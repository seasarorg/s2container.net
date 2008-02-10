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
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class RelationRowCreatorImpl : IRelationRowCreator
    {
        public virtual object CreateRelationRow(IDataReader dr, IRelationPropertyType rpt,
            System.Collections.IList columnNames, System.Collections.Hashtable relKeyValues,
            IDictionary<String, IDictionary<String, IPropertyType>> relationPropertyCache)
        {
            // - - - - - - - 
            // Entry Point!
            // - - - - - - -
            RelationRowCreationResource res = createResourceForRow(dr, rpt, columnNames, relKeyValues, relationPropertyCache);
            return CreateRelationRow(res);

            // *******************************
            // Comment out old style creation.
            // *******************************
            //object row = null;
            //IBeanMetaData bmd = rpt.BeanMetaData;
            //for (int i = 0; i < rpt.KeySize; ++i)
            //{
            //    string columnName = rpt.GetMyKey(i);
            //    if (columnNames.Contains(columnName))
            //    {
            //        if (row == null) row = NewRelationRow(rpt);
            //        if (relKeyValues != null && relKeyValues.ContainsKey(columnName))
            //        {
            //            object value = relKeyValues[columnName];
            //            IPropertyType pt = bmd.GetPropertyTypeByColumnName(rpt.GetYourKey(i));
            //            PropertyInfo pi = pt.PropertyInfo;
            //            if (value != null) pi.SetValue(row, value, null);
            //        }
            //    }
            //    continue;
            //}
            //for (int i = 0; i < bmd.PropertyTypeSize; ++i)
            //{
            //    IPropertyType pt = bmd.GetPropertyType(i);
            //    if (!pt.PropertyInfo.CanWrite) {
            //        continue;
            //    }
            //    String columnName = pt.ColumnName + "_" + rpt.RelationNo;
            //    if (!columnNames.Contains(columnName)) continue;
            //    if (row == null) row = NewRelationRow(rpt);
            //    object value = null;
            //    PropertyInfo pi = pt.PropertyInfo;
            //    if (relKeyValues != null && relKeyValues.ContainsKey(columnName))
            //    {
            //        value = relKeyValues[columnName];
            //    }
            //    else
            //    {
            //        IValueType valueType = pt.ValueType;
            //        value = valueType.GetValue(reader, columnName);
            //    }
            //    if (value != null) pi.SetValue(row, value, null);
            //}
            //return row;
        }

        protected virtual RelationRowCreationResource createResourceForRow(IDataReader dr,
                IRelationPropertyType rpt, System.Collections.IList columnNames, System.Collections.Hashtable relKeyValues,
                IDictionary<String, IDictionary<String, IPropertyType>> relationPropertyCache) {
            RelationRowCreationResource res = new RelationRowCreationResource();
            res.DataReader = dr;
            res.RelationPropertyType = rpt;
            res.ColumnNames = columnNames;
            res.RelKeyValues = relKeyValues;
            res.RelationPropertyCache = relationPropertyCache;
            res.BaseSuffix = "";// as Default
            res.RelationNoSuffix = BuildRelationNoSuffix(rpt);
            res.LimitRelationNestLevel = GetLimitRelationNestLevel();
            res.CurrentRelationNestLevel = 1;// as Default
            res.IsCreateDeadLink = IsCreateDeadLink();
            return res;
        }

        /**
         * @param res The resource of relation row creation. (NotNull)
         * @return Created relation row. (Nullable)
         * @throws SQLException
         */
        protected virtual Object CreateRelationRow(RelationRowCreationResource res) {
            // - - - - - - - - - - - 
            // Recursive Call Point!
            // - - - - - - - - - - -

            // Select句に該当RelationのPropertyが一つも指定されていない場合は、
            // この時点ですぐにreturn null;とする。Java版のS2Daoと同じ仕様に合わせる。[DAO-7]
            if (!res.HasPropertyCacheElement()) {
                return null;
            }

            SetupRelationKeyValue(res);
            SetupRelationAllValue(res);
            return res.Row;
        }

        protected virtual void SetupRelationKeyValue(RelationRowCreationResource res) {
            IRelationPropertyType rpt = res.RelationPropertyType;
            IBeanMetaData bmd = rpt.BeanMetaData;
            for (int i = 0; i < rpt.KeySize; ++i) {
                String columnName = rpt.GetMyKey(i) + res.BaseSuffix;

                if (!res.ContainsColumnName(columnName)) {
                    continue;
                }
                if (!res.HasRowInstance()) {
                    res.Row = NewRelationRow(rpt);
                }
                if (!res.ContainsRelKeyValueIfExists(columnName)) {
                    continue;
                }
                Object value = res.ExtractRelKeyValue(columnName);
                if (value == null) {
                    continue;
                }

                String yourKey = rpt.GetYourKey(i);
                IPropertyType pt = bmd.GetPropertyTypeByColumnName(yourKey);
                PropertyInfo pi = pt.PropertyInfo;
                pi.SetValue(res.Row, value, null);
                continue;
            }
        }

        protected virtual void SetupRelationAllValue(RelationRowCreationResource res) {
            IDictionary<String, IPropertyType> propertyCacheElement = res.ExtractPropertyCacheElement();
            ICollection<String> columnNameCacheElementKeySet = propertyCacheElement.Keys;
            foreach (String columnName in columnNameCacheElementKeySet) {
                IPropertyType pt = propertyCacheElement[columnName];
                res.CurrentPropertyType = pt;
                if (!IsValidRelationPerPropertyLoop(res)) {
                    res.ClearRowInstance();
                    return;
                }
                SetupRelationProperty(res);
            }
            if (!IsValidRelationAfterPropertyLoop(res)) {
                res.ClearRowInstance();
                return;
            }
            res.ClearValidValueCount();
            if (res.HasNextRelationProperty() && res.HasNextRelationLevel()) {
                SetupNextRelationRow(res);
            }
        }

        protected virtual bool IsValidRelationPerPropertyLoop(RelationRowCreationResource res) {
            return true;// Always true as default. This method is for extension(for override).
        }

        protected virtual bool IsValidRelationAfterPropertyLoop(RelationRowCreationResource res) {
            if (res.IsCreateDeadLink) {
                return true;
            }
            return res.HasValidValueCount();
        }

        protected virtual void SetupRelationProperty(RelationRowCreationResource res) {
            String columnName = res.BuildRelationColumnName();
            if (!res.HasRowInstance()) {
                res.Row = NewRelationRow(res);
            }
            RegisterRelationValue(res, columnName);
        }

        protected virtual void RegisterRelationValue(RelationRowCreationResource res, String columnName) {
            IPropertyType pt = res.CurrentPropertyType;
            Object value = null;
            if (res.ContainsRelKeyValueIfExists(columnName)) {
                value = res.ExtractRelKeyValue(columnName);
            } else {
                IValueType valueType = pt.ValueType;
                value = valueType.GetValue(res.DataReader, columnName);
            }
            if (value != null) {
                RegisterRelationValidValue(res, pt, value);
            }
        }

        protected virtual void RegisterRelationValidValue(RelationRowCreationResource res, IPropertyType pt, Object value) {
            res.IncrementValidValueCount();
            PropertyInfo pd = pt.PropertyInfo;
            pd.SetValue(res.Row, value, null);
        }

        // -----------------------------------------------------
        //                                         Next Relation
        //                                         -------------
        protected virtual void SetupNextRelationRow(RelationRowCreationResource res) {
            IBeanMetaData nextBmd = res.GetRelationBeanMetaData();
            Object row = res.Row;
            res.BackupRelationPropertyType();
            res.IncrementCurrentRelationNestLevel();
            try {
                for (int i = 0; i < nextBmd.RelationPropertyTypeSize; ++i) {
                    IRelationPropertyType nextRpt = nextBmd.GetRelationPropertyType(i);
                    SetupNextRelationRowElement(res, row, nextRpt);
                }
            } finally {
                res.Row = row;
                res.RestoreRelationPropertyType();
                res.DecrementCurrentRelationNestLevel();
            }
        }

        protected virtual void SetupNextRelationRowElement(RelationRowCreationResource res, Object row, IRelationPropertyType nextRpt) {
            if (nextRpt == null) {
                return;
            }
            res.ClearRowInstance();
            res.RelationPropertyType = nextRpt;

            String baseSuffix = res.RelationNoSuffix;
            String additionalRelationNoSuffix = BuildRelationNoSuffix(nextRpt);
            res.BackupSuffixAndPrepare(baseSuffix, additionalRelationNoSuffix);
            try {
                Object relationRow = CreateRelationRow(res);
                if (relationRow != null) {
                    nextRpt.PropertyInfo.SetValue(row, relationRow, null);
                }
            } finally {
                res.RestoreSuffix();
            }
        }

        // ===================================================================================
        //                                                             Property Cache Creation
        //                                                             =======================
        public virtual IDictionary<String, IDictionary<String, IPropertyType>> CreateRelationPropertyCache(System.Collections.IList columnNames, IBeanMetaData bmd) {
            IDictionary<String, IDictionary<String, IPropertyType>> relationPropertyCache = NewRelationPropertyCache();
            for (int i = 0; i < bmd.RelationPropertyTypeSize; ++i) {
                IRelationPropertyType rpt = bmd.GetRelationPropertyType(i);
                String baseSuffix = "";
                String relationNoSuffix = BuildRelationNoSuffix(rpt);
                RelationRowCreationResource res = CreateResourceForPropertyCache(
                    rpt, columnNames, relationPropertyCache, baseSuffix,
                    relationNoSuffix, GetLimitRelationNestLevel());
                if (rpt == null) {
                    continue;
                }
                SetupPropertyCache(res);
            }
            return relationPropertyCache;
        }

        protected virtual RelationRowCreationResource CreateResourceForPropertyCache(
                IRelationPropertyType rpt, System.Collections.IList columnNames,
                IDictionary<String, IDictionary<String, IPropertyType>> relationPropertyCache, String baseSuffix,
                String relationNoSuffix, int limitRelationNestLevel) {
            RelationRowCreationResource res = new RelationRowCreationResource();
            res.RelationPropertyType = rpt;
            res.ColumnNames = columnNames;
            res.RelationPropertyCache = relationPropertyCache;
            res.BaseSuffix = baseSuffix;
            res.RelationNoSuffix = relationNoSuffix;
            res.LimitRelationNestLevel = limitRelationNestLevel;
            res.CurrentRelationNestLevel = 1;// as Default
            return res;
        }

        protected virtual void SetupPropertyCache(RelationRowCreationResource res) {
            // - - - - - - - - - - - 
            // Recursive Call Point!
            // - - - - - - - - - - -
            // Cacheの初期化をする。この時点では空っぽである。
            // Cacheするべきものが一つも無い場合でも、
            // 「一つも無い」という状態がCacheされることになる。。
            res.InitializePropertyCacheElement();

            if (!IsPropertyRelation(res)) {
                return;
            }

            // Set up property cache about current beanMetaData.
            IBeanMetaData nextBmd = res.GetRelationBeanMetaData();
            for (int i = 0; i < nextBmd.PropertyTypeSize; ++i) {
                IPropertyType pt = nextBmd.GetPropertyType(i);
                res.CurrentPropertyType = pt;
                if (!IsTargetProperty(res)) {
                    continue;
                }
                SetupPropertyCacheElement(res);
            }

            // Set up next relation.
            if (res.HasNextRelationProperty() && res.HasNextRelationLevel()) {
                res.BackupRelationPropertyType();
                res.IncrementCurrentRelationNestLevel();
                try {
                    SetupNextPropertyCache(res, nextBmd);
                } finally {
                    res.RestoreRelationPropertyType();
                    res.DecrementCurrentRelationNestLevel();
                }
            }
        }

        protected virtual void SetupPropertyCacheElement(RelationRowCreationResource res) {
            String columnName = res.BuildRelationColumnName();
            if (!res.ContainsColumnName(columnName)) {
                return;
            }
            res.SavePropertyCacheElement();
        }

        // -----------------------------------------------------
        //                                         Next Relation
        //                                         -------------
        protected virtual void SetupNextPropertyCache(RelationRowCreationResource res, IBeanMetaData nextBmd) {
            for (int i = 0; i < nextBmd.RelationPropertyTypeSize; ++i) {
                IRelationPropertyType nextNextRpt = nextBmd.GetRelationPropertyType(i);
                res.RelationPropertyType = nextNextRpt;
                SetupNextPropertyCacheElement(res, nextNextRpt);
            }
        }

        protected virtual void SetupNextPropertyCacheElement(RelationRowCreationResource res, IRelationPropertyType nextNextRpt) {
            String baseSuffix = res.RelationNoSuffix;
            String additionalRelationNoSuffix = BuildRelationNoSuffix(nextNextRpt);
            res.BackupSuffixAndPrepare(baseSuffix, additionalRelationNoSuffix);
            try {
                SetupPropertyCache(res);// Recursive call!
            } finally {
                res.RestoreSuffix();
            }
        }

        // -----------------------------------------------------
        //                                                Common
        //                                                ------
        protected virtual IDictionary<String, IDictionary<String, IPropertyType>> NewRelationPropertyCache() {
            return new Dictionary<String, IDictionary<String, IPropertyType>>();
        }

        // ===================================================================================
        //                                                                        Common Logic
        //                                                                        ============
        protected virtual String BuildRelationNoSuffix(IRelationPropertyType rpt) {
            return "_" + rpt.RelationNo;
        }

        protected virtual Object NewRelationRow(RelationRowCreationResource res) {
            return NewRelationRow(res.RelationPropertyType);
        }

        protected virtual Object NewRelationRow(IRelationPropertyType rpt) {
            return ClassUtil.NewInstance(rpt.PropertyInfo.PropertyType);
        }

        // ===================================================================================
        //                                                                     Extension Point
        //                                                                     ===============
        protected virtual bool IsPropertyRelation(RelationRowCreationResource res) {
            // - - - - - - - - - - - - - - - - - - - - - - - -
            // Extension Point!
            //  --> 該当のRelationを処理対象とするか否か。
            // - - - - - - - - - - - - - - - - - - - - - - - -
            return true;
        }
        
        protected virtual bool IsTargetProperty(RelationRowCreationResource res) {
            // - - - - - - - - - - - - - - - - - - - - - - - -
            // Extension Point!
            //  --> 該当のPropertyを処理対象とするか否か。
            // - - - - - - - - - - - - - - - - - - - - - - - -
            IPropertyType pt = res.CurrentPropertyType;
            return pt.PropertyInfo.CanWrite;
        }

        protected virtual bool IsCreateDeadLink() {
            // - - - - - - - - - - - - - - - - - - - - - - - -
            // Extension Point!
            //  --> 参照切れの場合にInstanceを作成するか否か。
            // - - - - - - - - - - - - - - - - - - - - - - - -
            return true;// 以前の仕様のまま(空Entityを作成する)とする。
        }

        protected virtual int GetLimitRelationNestLevel() {
            // - - - - - - - - - - - - - - - - - - - - - - - - -
            // Extension Point!
            //  --> RelationのNestを何レベルまで許可するか否か。
            // - - - - - - - - - - - - - - - - - - - - - - - - -
            return 1;// 以前の仕様のまま(1階層まで)とする。
        }
    }
}
