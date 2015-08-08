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

using System.Collections;
using System.Collections.Generic;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class RelationRowCreatorImpl : IRelationRowCreator
    {
        public virtual object CreateRelationRow(IDataReader dr, IRelationPropertyType rpt,
            IList columnNames, Hashtable relKeyValues,
            IDictionary<string, IDictionary<string, IPropertyType>> relationPropertyCache)
        {
            // - - - - - - - 
            // Entry Point!
            // - - - - - - -
            var res = CreateResourceForRow(dr, rpt, columnNames, relKeyValues, relationPropertyCache);
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
            //    string columnName = pt.ColumnName + "_" + rpt.RelationNo;
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

        protected virtual RelationRowCreationResource CreateResourceForRow(IDataReader dr,
                IRelationPropertyType rpt, IList columnNames, Hashtable relKeyValues,
                IDictionary<string, IDictionary<string, IPropertyType>> relationPropertyCache) {
            var res = new RelationRowCreationResource
            {
                DataReader = dr,
                RelationPropertyType = rpt,
                ColumnNames = columnNames,
                RelKeyValues = relKeyValues,
                RelationPropertyCache = relationPropertyCache,
                BaseSuffix = string.Empty,
                RelationNoSuffix = BuildRelationNoSuffix(rpt),
                LimitRelationNestLevel = GetLimitRelationNestLevel(),
                CurrentRelationNestLevel = 1,
                IsCreateDeadLink = IsCreateDeadLink()
            };
            // as Default
            // as Default
            return res;
        }

        /**
         * @param res The resource of relation row creation. (NotNull)
         * @return Created relation row. (Nullable)
         * @throws SQLException
         */
        protected virtual object CreateRelationRow(RelationRowCreationResource res) {
            // - - - - - - - - - - - 
            // Recursive Call Point!
            // - - - - - - - - - - -

            // Select��ɊY��Relation��Property�����w�肳��Ă��Ȃ��ꍇ�́A
            // ���̎��_�ł�����return null;�Ƃ���BJava�ł�S2Dao�Ɠ����d�l�ɍ��킹��B[DAO-7]
            if (!res.HasPropertyCacheElement()) {
                return null;
            }

            SetupRelationKeyValue(res);
            SetupRelationAllValue(res);
            return res.Row;
        }

        protected virtual void SetupRelationKeyValue(RelationRowCreationResource res) {
            var rpt = res.RelationPropertyType;
            var bmd = rpt.BeanMetaData;
            for (var i = 0; i < rpt.KeySize; ++i) {
                var columnName = rpt.GetMyKey(i) + res.BaseSuffix;

                if (!res.ContainsColumnName(columnName)) {
                    continue;
                }
                if (!res.HasRowInstance()) {
                    res.Row = NewRelationRow(rpt);
                }
                if (!res.ContainsRelKeyValueIfExists(columnName)) {
                    continue;
                }
                var value = res.ExtractRelKeyValue(columnName);
                if (value == null) {
                    continue;
                }

                var yourKey = rpt.GetYourKey(i);
                var pt = bmd.GetPropertyTypeByColumnName(yourKey);
                var pi = pt.PropertyInfo;
//                pi.SetValue(res.Row, value, null);
                PropertyUtil.SetValue(res.Row, res.Row.GetExType(), pi.Name, pi.PropertyType, value);
            }
        }

        protected virtual void SetupRelationAllValue(RelationRowCreationResource res) {
            var propertyCacheElement = res.ExtractPropertyCacheElement();
            var columnNameCacheElementKeySet = propertyCacheElement.Keys;
            foreach (var columnName in columnNameCacheElementKeySet) 
            {
                var pt = propertyCacheElement[columnName];
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
            var columnName = res.BuildRelationColumnName();
            if (!res.HasRowInstance()) {
                res.Row = NewRelationRow(res);
            }
            RegisterRelationValue(res, columnName);
        }

        protected virtual void RegisterRelationValue(RelationRowCreationResource res, string columnName) {
            var pt = res.CurrentPropertyType;
            object value;
            if (res.ContainsRelKeyValueIfExists(columnName)) {
                value = res.ExtractRelKeyValue(columnName);
            } else {
                var valueType = pt.ValueType;
                value = valueType.GetValue(res.DataReader, columnName);
            }
            if (value != null) {
                RegisterRelationValidValue(res, pt, value);
            }
        }

        protected virtual void RegisterRelationValidValue(RelationRowCreationResource res, IPropertyType pt, object value) {
            res.IncrementValidValueCount();
            var pd = pt.PropertyInfo;
//            pd.SetValue(res.Row, value, null);
            PropertyUtil.SetValue(res.Row, res.Row.GetExType(), pd.Name, pd.PropertyType, value);
        }

        // -----------------------------------------------------
        //                                         Next Relation
        //                                         -------------
        protected virtual void SetupNextRelationRow(RelationRowCreationResource res) {
            var nextBmd = res.GetRelationBeanMetaData();
            var row = res.Row;
            res.BackupRelationPropertyType();
            res.IncrementCurrentRelationNestLevel();
            try {
                for (var i = 0; i < nextBmd.RelationPropertyTypeSize; ++i) {
                    var nextRpt = nextBmd.GetRelationPropertyType(i);
                    SetupNextRelationRowElement(res, row, nextRpt);
                }
            } finally {
                res.Row = row;
                res.RestoreRelationPropertyType();
                res.DecrementCurrentRelationNestLevel();
            }
        }

        protected virtual void SetupNextRelationRowElement(RelationRowCreationResource res, object row, IRelationPropertyType nextRpt) {
            if (nextRpt == null) {
                return;
            }
            res.ClearRowInstance();
            res.RelationPropertyType = nextRpt;

            var baseSuffix = res.RelationNoSuffix;
            var additionalRelationNoSuffix = BuildRelationNoSuffix(nextRpt);
            res.BackupSuffixAndPrepare(baseSuffix, additionalRelationNoSuffix);
            try {
                var relationRow = CreateRelationRow(res);
                if (relationRow != null) {
//                    nextRpt.PropertyInfo.SetValue(row, relationRow, null);
                    var pd = nextRpt.PropertyInfo;
                    PropertyUtil.SetValue(row, row.GetExType(), pd.Name, pd.PropertyType, relationRow);
                }
            } finally {
                res.RestoreSuffix();
            }
        }

        // ===================================================================================
        //                                                             Property Cache Creation
        //                                                             =======================
        public virtual IDictionary<string, IDictionary<string, IPropertyType>> CreateRelationPropertyCache(IList columnNames, IBeanMetaData bmd) {
            var relationPropertyCache = NewRelationPropertyCache();
            for (var i = 0; i < bmd.RelationPropertyTypeSize; ++i) {
                var rpt = bmd.GetRelationPropertyType(i);
                var baseSuffix = string.Empty;
                var relationNoSuffix = BuildRelationNoSuffix(rpt);
                var res = CreateResourceForPropertyCache(
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
                IRelationPropertyType rpt, IList columnNames,
                IDictionary<string, IDictionary<string, IPropertyType>> relationPropertyCache, string baseSuffix,
                string relationNoSuffix, int limitRelationNestLevel) {
            var res = new RelationRowCreationResource
            {
                RelationPropertyType = rpt,
                ColumnNames = columnNames,
                RelationPropertyCache = relationPropertyCache,
                BaseSuffix = baseSuffix,
                RelationNoSuffix = relationNoSuffix,
                LimitRelationNestLevel = limitRelationNestLevel,
                CurrentRelationNestLevel = 1
            };
            // as Default
            return res;
        }

        protected virtual void SetupPropertyCache(RelationRowCreationResource res) {
            // - - - - - - - - - - - 
            // Recursive Call Point!
            // - - - - - - - - - - -
            // Cache�̏����������B���̎��_�ł͋���ۂł���B
            // Cache����ׂ���̂��������ꍇ�ł�A
            // �u�������v�Ƃ�����Ԃ�Cache����邱�ƂɂȂ�B�B
            res.InitializePropertyCacheElement();

            if (!IsPropertyRelation(res)) {
                return;
            }

            // Set up property cache about current beanMetaData.
            var nextBmd = res.GetRelationBeanMetaData();
            for (var i = 0; i < nextBmd.PropertyTypeSize; ++i) {
                var pt = nextBmd.GetPropertyType(i);
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
            var columnName = res.BuildRelationColumnName();
            if (!res.ContainsColumnName(columnName)) {
                return;
            }
            res.SavePropertyCacheElement();
        }

        // -----------------------------------------------------
        //                                         Next Relation
        //                                         -------------
        protected virtual void SetupNextPropertyCache(RelationRowCreationResource res, IBeanMetaData nextBmd) {
            for (var i = 0; i < nextBmd.RelationPropertyTypeSize; ++i) {
                var nextNextRpt = nextBmd.GetRelationPropertyType(i);
                res.RelationPropertyType = nextNextRpt;
                SetupNextPropertyCacheElement(res, nextNextRpt);
            }
        }

        protected virtual void SetupNextPropertyCacheElement(RelationRowCreationResource res, IRelationPropertyType nextNextRpt) {
            var baseSuffix = res.RelationNoSuffix;
            var additionalRelationNoSuffix = BuildRelationNoSuffix(nextNextRpt);
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
        protected virtual IDictionary<string, IDictionary<string, IPropertyType>> NewRelationPropertyCache() {
            return new Dictionary<string, IDictionary<string, IPropertyType>>();
        }

        // ===================================================================================
        //                                                                        Common Logic
        //                                                                        ============
        protected virtual string BuildRelationNoSuffix(IRelationPropertyType rpt) {
            return "_" + rpt.RelationNo;
        }

        protected virtual object NewRelationRow(RelationRowCreationResource res) {
            return NewRelationRow(res.RelationPropertyType);
        }

        protected virtual object NewRelationRow(IRelationPropertyType rpt) {
            return ClassUtil.NewInstance(rpt.PropertyInfo.PropertyType);
        }

        // ===================================================================================
        //                                                                     Extension Point
        //                                                                     ===============
        protected virtual bool IsPropertyRelation(RelationRowCreationResource res) {
            // - - - - - - - - - - - - - - - - - - - - - - - -
            // Extension Point!
            //  --> �Y����Relation������ΏۂƂ��邩�ۂ��B
            // - - - - - - - - - - - - - - - - - - - - - - - -
            return true;
        }
        
        protected virtual bool IsTargetProperty(RelationRowCreationResource res) {
            // - - - - - - - - - - - - - - - - - - - - - - - -
            // Extension Point!
            //  --> �Y����Property������ΏۂƂ��邩�ۂ��B
            // - - - - - - - - - - - - - - - - - - - - - - - -
            var pt = res.CurrentPropertyType;
            return pt.PropertyInfo.CanWrite;
        }

        protected virtual bool IsCreateDeadLink() {
            // - - - - - - - - - - - - - - - - - - - - - - - -
            // Extension Point!
            //  --> �Q�Ɛ؂�̏ꍇ��Instance��쐬���邩�ۂ��B
            // - - - - - - - - - - - - - - - - - - - - - - - -
            return true;// �ȑO�̎d�l�̂܂�(��Entity��쐬����)�Ƃ���B
        }

        protected virtual int GetLimitRelationNestLevel() {
            // - - - - - - - - - - - - - - - - - - - - - - - - -
            // Extension Point!
            //  --> Relation��Nest������x���܂ŋ����邩�ۂ��B
            // - - - - - - - - - - - - - - - - - - - - - - - - -
            return 1;// �ȑO�̎d�l�̂܂�(1�K�w�܂�)�Ƃ���B
        }
    }
}
