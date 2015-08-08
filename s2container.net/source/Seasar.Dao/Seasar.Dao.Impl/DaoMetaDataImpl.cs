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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Dao.Dbms;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Beans;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public class DaoMetaDataImpl : IDaoMetaData
    {
        private static readonly Regex _startWithOrderByReplacePattern =
            new Regex(@"/\*.*?\*/");

        private static readonly Regex _startWithBeginCommentPattern =
            new Regex(@"/\*BEGIN\*/\s*WHERE .+", RegexOptions.IgnoreCase);

        private static readonly Regex _withBeginCommentToReplaceANDPattern =
            new Regex(@"/\*BEGIN\*/\s*WHERE", RegexOptions.IgnoreCase);

        private const string NOT_SINGLE_ROW_UPDATED = "NotSingleRowUpdated";

        private const string BEGIN_WHERE = @"/*BEGIN*/WHERE";
        private const string BEGIN_AND = @"/*BEGIN*/AND";

        protected string[] insertPrefixes = { "Insert", "Create", "Add" };
        protected string[] updatePrefixes = { "Update", "Modify", "Store" };
        protected string[] deletePrefixes = { "Delete", "Remove" };
        protected string[] modifiedOnlySuffixes = { "ModifiedOnly" };
        protected string[] unlessNullSuffixes = { "UnlessNull" };

        protected Type daoType;
        protected Type daoInterface;
        protected IDataSource dataSource;
        protected IAnnotationReaderFactory annotationReaderFactory;
        protected IDaoAnnotationReader annotationReader;
        protected ICommandFactory commandFactory;
        protected IDataReaderFactory dataReaderFactory;
        protected IDataReaderHandlerFactory dataReaderHandlerFactory;
        protected string sqlFileEncoding = Encoding.Default.WebName;
        protected IDbms dbms;
        protected Type beanType;
        protected IBeanMetaData beanMetaData;
        protected IDatabaseMetaData dbMetaData;
        protected Hashtable sqlCommands = new Hashtable();

        public DaoMetaDataImpl()
        {
        }

        public DaoMetaDataImpl(Type daoType, IDataSource dataSource, ICommandFactory commandFactory,
            IDataReaderFactory dataReaderFactory, IDatabaseMetaData dbMetaData)
            : this(daoType, dataSource, commandFactory, dataReaderFactory, new FieldAnnotationReaderFactory(), dbMetaData)
        {
            DaoType = daoType;
            DataSource = dataSource;
            CommandFactory = commandFactory;
            DataReaderFactory = dataReaderFactory;
            DatabaseMetaData = dbMetaData;
            Initialize();
        }

        public DaoMetaDataImpl(Type daoType, IDataSource dataSource, ICommandFactory commandFactory,
            IDataReaderFactory dataReaderFactory, IAnnotationReaderFactory annotationReaderFactory, IDatabaseMetaData dbMetaData)
        {
            DaoType = daoType;
            DataSource = dataSource;
            CommandFactory = commandFactory;
            DataReaderFactory = dataReaderFactory;
            AnnotationReaderFactory = annotationReaderFactory;
            DatabaseMetaData = dbMetaData;
            Initialize();
        }

        public virtual void Initialize()
        {
            daoInterface = GetDaoInterface(daoType);
            annotationReader = AnnotationReaderFactory.CreateDaoAnnotationReader(daoType);
            beanType = annotationReader.GetBeanType();
            dbms = DbmsManager.GetDbms(dataSource);
            beanMetaData = new BeanMetaDataImpl(beanType, dbMetaData, dbms, annotationReaderFactory);
            SetupSqlCommand();
        }

        public IAnnotationReaderFactory AnnotationReaderFactory
        {
            get { return annotationReaderFactory; }
            set { annotationReaderFactory = value; }
        }

        protected virtual void SetupSqlCommand()
        {
            var allMethods = daoInterface.GetMethods();
            var names = new Hashtable();
            foreach (var mi in allMethods)
            {
                names[mi.Name] = mi;
            }
            var enu = names.GetEnumerator();
            while (enu.MoveNext())
            {
                try
                {
                    var method = daoType.GetMethod((string)enu.Key);
                    if (method.IsAbstract) SetupMethod(method);
                }
                catch (AmbiguousMatchException) { }
            }
        }

        protected virtual string ReadText(string path, Assembly asm)
        {
            using (var stream = ResourceUtil.GetResourceAsStream(path, asm))
            {
                using (TextReader reader = new StreamReader(stream, Encoding.GetEncoding(SqlFileEncoding)))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        protected virtual void SetupMethod(MethodInfo mi)
        {
            SetupMethodByAttribute(mi);

            if (!CompletedSetupMethod(mi))
            {
                SetupMethodBySqlFile(mi);
            }

            if (!CompletedSetupMethod(mi) && annotationReader.IsSqlFile(mi.Name))
            {
                var fileName = GetSqlFilePath(mi) + ".sql";
                throw new SqlFileNotFoundRuntimeException(daoType, mi, fileName);
            }

            if (!CompletedSetupMethod(mi))
            {
                SetupMethodByAuto(mi);
            }
        }

        protected virtual void SetupMethodByAttribute(MethodInfo mi)
        {
            var sql = annotationReader.GetSql(mi.Name, dbms);
            if (sql != null)
            {
                SetupMethodByManual(mi, sql);
                return;
            }

            var procedureName = annotationReader.GetProcedure(mi.Name);
            if (procedureName != null)
            {
                SetupProcedure(mi, procedureName);
            }
        }

        protected virtual void SetupMethodBySqlFile(MethodInfo mi)
        {
            var baseName = GetSqlFilePath(mi);
            var dbmsPath = baseName + dbms.Suffix + ".sql";
            var standardPath = baseName + ".sql";
            var asm = daoInterface.Assembly;
            if (ResourceUtil.IsExist(dbmsPath, asm))
            {
                var sql = ReadText(dbmsPath, asm);
                SetupMethodByManual(mi, sql);
            }
            else if (ResourceUtil.IsExist(standardPath, asm))
            {
                var sql = ReadText(standardPath, asm);
                SetupMethodByManual(mi, sql);
            }
        }

        protected virtual string GetSqlFilePath(MethodInfo mi)
        {
            string baseName;
            var fileByAttribute = annotationReader.GetSqlFilePath(mi.Name);
            if (StringUtil.IsEmpty(fileByAttribute))
            {
                baseName = daoInterface.FullName + "_" + mi.Name;
            }
            else
            {
                fileByAttribute = fileByAttribute.Replace('/', '.');
                baseName = Regex.Replace(fileByAttribute, ".sql$", string.Empty);
            }
            return baseName;
        }

        protected virtual void SetupMethodByManual(MethodInfo mi, string sql)
        {
            if (IsSelect(mi))
            {
                SetupSelectMethodByManual(mi, sql);
            }
            else
            {
                SetupUpdateMethodByManual(mi, sql);
            }
        }

        protected virtual void SetupMethodByAuto(MethodInfo mi)
        {
            if (IsInsert(mi.Name))
            {
                SetupInsertMethodByAuto(mi);
            }
            else if (IsUpdate(mi.Name))
            {
                SetupUpdateMethodByAuto(mi);
            }
            else if (IsDelete(mi.Name))
            {
                SetupDeleteMethodByAuto(mi);
            }
            else
            {
                SetupSelectMethodByAuto(mi);
            }
        }

        protected virtual void SetupSelectMethodByManual(MethodInfo mi, string sql)
        {
            var cmd = CreateSelectDynamicCommand(CreateDataReaderHandler(mi));
            cmd.Sql = sql;
            cmd.ArgNames = GetArgNames(mi);
            cmd.ArgTypes = GetArgTypes(mi);
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual SelectDynamicCommand CreateSelectDynamicCommand(IDataReaderHandler drh)
        {
            return new SelectDynamicCommand(dataSource, commandFactory, drh, dataReaderFactory);
        }

        protected virtual SelectDynamicCommand CreateSelectDynamicCommand(
            IDataReaderHandler dataReaderHandler, string query)
        {
            var cmd = CreateSelectDynamicCommand(dataReaderHandler);
            var buf = new StringBuilder(255);
            var isBaseSqlIncludeWhereClause = false;
            if (StartsWithSelect(query))
            {
                buf.Append(query);
            }
            else
            {
                var sql = dbms.GetAutoSelectSql(BeanMetaData);
                buf.Append(sql);
                if (query != null)
                {
                    if (StartsWithOrderBy(query))
                    {
                        buf.Append(" ");
                    }
                    else if (StartsWithBeginComment(query))
                    {
                        buf.Append(" ");
                    }
                    else if (sql.LastIndexOf("WHERE", StringComparison.Ordinal) < 0)
                    {
                        buf.Append(" WHERE ");
                    }
                    else
                    {
                        buf.Append(" AND ");
                    }
                    buf.Append(query);
                }

                isBaseSqlIncludeWhereClause = IsIncludeWhereClause(sql);
            }

            var cmdSql = buf.ToString();
            //  外部結合の記述がFROM句ではなくWHERE句に書かれている場合
            if (isBaseSqlIncludeWhereClause)
            {
                cmdSql = _withBeginCommentToReplaceANDPattern.Replace(cmdSql, BEGIN_AND);
            }
            cmd.Sql = cmdSql;
            return cmd;
        }

        protected virtual AbstractSqlCommand CreateUpdateAutoStaticCommand(MethodInfo methodInfo, IDataSource ds, ICommandFactory factory, 
            IBeanMetaData beanMeta, string[] propertyNames)
        {
            return new UpdateAutoStaticCommand(ds, factory, beanMeta, propertyNames);
        }

        protected virtual AbstractSqlCommand CreateUpdateModifiedOnlyCommand(MethodInfo methodInfo, IDataSource ds, ICommandFactory factory, 
            IBeanMetaData beanMeta, string[] propertyNames)
        {
            return new UpdateModifiedOnlyCommand(ds, factory, beanMeta, propertyNames);
        }

        protected virtual AbstractSqlCommand CreateUpdateAutoDynamicCommand(MethodInfo methodInfo, IDataSource ds, ICommandFactory factory, 
            IBeanMetaData beanMeta, string[] propertyNames)
        {
            return new UpdateAutoDynamicCommand(ds, factory, beanMeta, propertyNames);
        }

        protected virtual AbstractSqlCommand CreateDeleteAutoStaticCommand(MethodInfo methodInfo, IDataSource ds, ICommandFactory factory, 
            IBeanMetaData beanMeta, string[] propertyNames)
        {
            return new DeleteAutoStaticCommand(ds, factory, beanMeta, propertyNames);
        }

        /// <summary>
        /// 現在は使用していません。(DAONET-3)
        /// </summary>
        protected virtual AbstractSqlCommand CreateInsertAutoStaticCommand(MethodInfo methodInfo, IDataSource ds, ICommandFactory factory, 
            IBeanMetaData beanMeta, string[] propertyNames)
        {
            return new InsertAutoStaticCommand(ds, factory, beanMeta, propertyNames);
        }

        protected virtual AbstractSqlCommand CreateInsertAutoDynamicCommand(MethodInfo methodInfo, IDataSource ds, ICommandFactory factory, 
            IBeanMetaData beanMeta, string[] propertyNames)
        {
            return new InsertAutoDynamicCommand(ds, factory, beanMeta, propertyNames);
        }

        protected static bool IsIncludeWhereClause(string sql)
        {
            if (sql != null)
            {
                if (sql.ToUpper().Contains(" WHERE "))
                {
                    return true;
                }
            }
            return false;
        }

        protected static bool StartsWithBeginComment(string query)
        {
            if (query != null)
            {
                var m = _startWithBeginCommentPattern.Match(query);
                if (m.Success)
                {
                    return true;
                }
            }
            return false;
        }

        protected static bool StartsWithSelect(string query) => StringUtil.StartWith(query, "select");

        protected static bool StartsWithOrderBy(string query)
        {
            if (query != null)
            {
                var replaceQuery = _startWithOrderByReplacePattern.Replace(query, string.Empty);
                if (StringUtil.StartWith(replaceQuery.Trim(), "order by"))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual IDataReaderHandler CreateDataReaderHandler(MethodInfo mi)
        {
            return CreateDataReaderHandler(mi, beanMetaData);
        }

        protected virtual IDataReaderHandler CreateDataReaderHandler(MethodInfo mi, IBeanMetaData bmd)
        {
            return dataReaderHandlerFactory.GetResultSetHandler(beanType, bmd, mi);
        }

        // [DAONET-76] (2009/05/05)
        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual ObjectGenericListDataReaderHandler CreateObjectGenericListDataReaderHandler(Type elementType)
        {
            return new ObjectGenericListDataReaderHandler(elementType);
        }

        // [DAONET-76] (2009/05/05)
        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual ObjectListDataReaderHandler CreateObjectListDataReaderHandler()
        {
            return new ObjectListDataReaderHandler();
        }

        // [DAONET-76] (2009/05/05)
        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual ObjectArrayDataReaderHandler CreateObjectArrayDataReaderHandler(Type elementType)
        {
            return new ObjectArrayDataReaderHandler(elementType);
        }

        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual BeanDataSetMetaDataDataReaderHandler CreateBeanDataSetMetaDataDataReaderHandler(IBeanMetaData bmd, Type returnType)
        {
            return new BeanDataSetMetaDataDataReaderHandler(returnType);
        }

        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual BeanDataTableMetaDataDataReaderHandler CreateBeanDataTableMetaDataDataReaderHandler(IBeanMetaData bmd, Type returnType)
        {
            return new BeanDataTableMetaDataDataReaderHandler(returnType);
        }

        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual BeanListMetaDataDataReaderHandler CreateBeanListMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanListMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
        }

        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual BeanMetaDataDataReaderHandler CreateBeanMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
        }

        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual BeanArrayMetaDataDataReaderHandler CreateBeanArrayMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanArrayMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
        }

        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual BeanGenericListMetaDataDataReaderHandler CreateBeanGenericListMetaDataDataReaderHandler(IBeanMetaData bmd)
        {
            return new BeanGenericListMetaDataDataReaderHandler(bmd, CreateRowCreator(), CreateRelationRowCreator());
        }

        [Obsolete("IDataReaderHandlerFactory.GetResultSetHandler()に移行。")]
        protected virtual ObjectDataReaderHandler CreateObjectDataReaderHandler()
        {
            return new ObjectDataReaderHandler();
        }

        protected virtual bool IsBeanTypeAssignable(Type type)
        {
            return beanType.IsAssignableFrom(type) ||
                type.IsAssignableFrom(beanType);
        }

        protected virtual void SetupUpdateMethodByManual(MethodInfo mi, string sql)
        {
            var cmd = new UpdateDynamicCommand(dataSource, commandFactory) {Sql = sql};
            var argNames = GetArgNames(mi);
            if (argNames.Length == 0 && IsUpdateSignatureForBean(mi))
                argNames = new[] { StringUtil.Decapitalize(beanType.Name) };
            cmd.ArgNames = argNames;
            cmd.ArgTypes = GetArgTypes(mi);
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual bool IsUpdateSignatureForBean(MethodInfo mi)
        {
            var paramTypes = GetArgTypes(mi);
            return paramTypes.Length == 1
                && IsBeanTypeAssignable(paramTypes[0]);
        }

        protected virtual void SetupInsertMethodByAuto(MethodInfo mi)
        {
            CheckAutoUpdateMethod(mi);
            var propertyNames = GetPersistentPropertyNames(mi.Name);
            ISqlCommand cmd;
            if (IsUpdateSignatureForBean(mi))
            {
                //  [DAONET-3]
                //  nullのプロパティをINSERTの対象に含めない
                //  Java版と合わせる為、InsertAutoStaticCommandは使用しません。
                //cmd = CreateInsertAutoStaticCommand(mi, _dataSource, _commandFactory, _beanMetaData, propertyNames);
                cmd = CreateInsertAutoDynamicCommand(mi, dataSource, commandFactory, beanMetaData, propertyNames);
            }
            else
            {
                throw new NotSupportedException("InsertBatchAutoStaticCommand");
            }
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual void SetupUpdateMethodByAuto(MethodInfo mi)
        {
            CheckAutoUpdateMethod(mi);
            var propertyNames = GetPersistentPropertyNames(mi.Name);
            AbstractSqlCommand cmd;
            if (IsUpdateSignatureForBean(mi))
            {
                if (IsUnlessNull(mi.Name))
                {
                    cmd = CreateUpdateAutoDynamicCommand(mi, dataSource, commandFactory, beanMetaData, propertyNames);
                }
                else if (IsModifiedOnly(mi.Name))
                {
                    cmd = CreateUpdateModifiedOnlyCommand(mi, dataSource, commandFactory, beanMetaData, propertyNames);
                }
                else
                {
                    cmd = CreateUpdateAutoStaticCommand(mi, dataSource, commandFactory, beanMetaData, propertyNames);
                }
            }
            else
            {
                throw new NotSupportedException("UpdateBatchAutoStaticCommand");
            }

            sqlCommands[mi.Name] = cmd;
        }

        protected virtual void SetupDeleteMethodByAuto(MethodInfo mi)
        {
            CheckAutoUpdateMethod(mi);
            var propertyNames = GetPersistentPropertyNames(mi.Name);
            ISqlCommand cmd;
            if (IsUpdateSignatureForBean(mi))

                cmd = CreateDeleteAutoStaticCommand(mi, dataSource, commandFactory, beanMetaData, propertyNames);
            else
                throw new NotSupportedException("DeleteBatchAutoStaticCommand");
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual string[] GetPersistentPropertyNames(string methodName)
        {
            var names = new ArrayList();
            var props = annotationReader.GetNoPersistentProps(methodName);
            if (props != null)
            {
                for (var i = 0; i < beanMetaData.PropertyTypeSize; ++i)
                {
                    var pt = beanMetaData.GetPropertyType(i);
                    if (pt.IsPersistent
                        && !IsPropertyExist(props, pt.PropertyName))
                        names.Add(pt.PropertyName);
                }
            }
            else
            {
                props = annotationReader.GetPersistentProps(methodName);
                if (props != null)
                {

                    foreach (var prop in props) names.Add(prop);
                    for (var i = 0; i < beanMetaData.PrimaryKeySize; ++i)
                    {
                        var pk = beanMetaData.GetPrimaryKey(i);
                        var pt = beanMetaData.GetPropertyTypeByColumnName(pk);
                        names.Add(pt.PropertyName);
                    }
                    if (beanMetaData.HasVersionNoPropertyType)
                        names.Add(beanMetaData.VersionNoPropertyName);
                    if (beanMetaData.HasTimestampPropertyType)
                        names.Add(beanMetaData.TimestampPropertyName);
                }
            }
            if (names.Count == 0)
            {
                for (var i = 0; i < beanMetaData.PropertyTypeSize; ++i)
                {
                    var pt = beanMetaData.GetPropertyType(i);
                    if (pt.IsPersistent) names.Add(pt.PropertyName);
                }
            }
            return (string[])names.ToArray(typeof(string));
        }

        protected virtual bool IsPropertyExist(string[] props, string propertyName)
        {
            return props.Any(prop => String.Compare(prop, propertyName, StringComparison.OrdinalIgnoreCase) == 0);
        }

        protected virtual void SetupSelectMethodByAuto(MethodInfo mi)
        {
            var query = annotationReader.GetQuery(mi.Name);
            var handler = CreateDataReaderHandler(mi);
            SelectDynamicCommand cmd;
            var argNames = GetArgNames(mi);
            var argTypes = GetArgTypes(mi);
            if (query != null && !StartsWithOrderBy(query))
            {
                cmd = CreateSelectDynamicCommand(handler, query);
            }
            else
            {
                cmd = CreateSelectDynamicCommand(handler);
                string sql;

                if (argTypes.Length == 1
                    && ValueTypes.GetValueType(argTypes[0]) == ValueTypes.OBJECT)
                {
                    argNames = new[] { "dto" };
                    sql = CreateAutoSelectSqlByDto(argTypes[0]);
                }
                else
                {
                    sql = CreateAutoSelectSql(argNames, argTypes);
                }
                if (query != null) sql += " " + query;
                cmd.Sql = sql;
            }
            cmd.ArgNames = argNames;
            cmd.ArgTypes = argTypes;
            sqlCommands[mi.Name] = cmd;
        }

        protected virtual string CreateAutoSelectSqlByDto(Type dtoType)
        {
            var sql = dbms.GetAutoSelectSql(BeanMetaData);
            var buf = new StringBuilder(sql);
            IDtoMetaData dmd = new DtoMetaDataImpl(dtoType, annotationReaderFactory.CreateBeanAnnotationReader(dtoType));
            var began = false;
            if (!(sql.LastIndexOf("WHERE", StringComparison.Ordinal) > 0))
            {
                buf.Append("/*BEGIN*/ WHERE ");
                began = true;
            }
            for (var i = 0; i < dmd.PropertyTypeSize; ++i)
            {
                var pt = dmd.GetPropertyType(i);
                var aliasName = pt.ColumnName;
                if (!beanMetaData.HasPropertyTypeByAliasName(aliasName))
                {
                    continue;
                }
                if (!beanMetaData.GetPropertyTypeByAliasName(aliasName).IsPersistent)
                {
                    continue;
                }
                var columnName = beanMetaData.ConvertFullColumnName(aliasName);
                var propertyName = "dto." + pt.PropertyName;

                // IFコメントを作成する
                CreateAutoIFComment(buf, columnName, propertyName, pt.PropertyType, i == 0, began);
            }
            if (began) buf.Append("/*END*/");
            return buf.ToString();
        }

        protected virtual string CreateAutoSelectSql(string[] argNames, Type[] argTypes)
        {
            var sql = dbms.GetAutoSelectSql(BeanMetaData);
            var buf = new StringBuilder(sql);
            if (argNames.Length != 0)
            {
                var began = false;
                if (!(sql.LastIndexOf("WHERE", StringComparison.Ordinal) > 0))
                {
                    buf.Append("/*BEGIN*/ WHERE ");
                    began = true;
                }
                for (var i = 0; i < argNames.Length; ++i)
                {
                    var columnName = beanMetaData.ConvertFullColumnName(argNames[i]);

                    // IFコメントを作成する
                    CreateAutoIFComment(buf, columnName, argNames[i], argTypes[i], i == 0, began);
                }
                if (began) buf.Append("/*END*/");
            }
            return buf.ToString();
        }

        /// <summary>
        /// IFコメントを作成する
        /// </summary>
        /// <param name="buf">SQLを格納する</param>
        /// <param name="columnName">テーブルの列名</param>
        /// <param name="propertyName">プロパティの名前</param>
        /// <param name="propertyType">プロパティの型</param>
        /// <param name="first">1つめのプロパティかどうか</param>
        /// <param name="began">BEGINコメントが開始されているかどうか</param>
        protected virtual void CreateAutoIFComment(StringBuilder buf, string columnName,
            string propertyName, Type propertyType, bool first, bool began)
        {
            buf.Append("/*IF ");

            // 評価式を作成する
            CreateIFExpression(buf, propertyName, propertyType);

            buf.Append("*/");
            buf.Append(" ");
            if (!began || !first)
            {
                buf.Append("AND ");
            }
            buf.Append(columnName);
            buf.Append(" = /*");
            buf.Append(propertyName);
            buf.Append("*/null");
            buf.Append("/*END*/");
        }

        /// <summary>
        /// 評価式を作成する
        /// </summary>
        /// <param name="buf">SQL</param>
        /// <param name="propertyName">プロパティの名前</param>
        /// <param name="propertyType">プロパティの型</param>
        protected virtual void CreateIFExpression(StringBuilder buf, string propertyName, Type propertyType)
        {
            var valueType = ValueTypes.GetValueType(propertyType);

            buf.Append(propertyName);

            if (valueType is SqlBaseType)
            {
                buf.Append(".IsNull == false");
            }
#if !NET_1_1
            else if (valueType is NullableBaseType)
            {
                buf.Append(".HasValue == true");
            }
#endif
#if NHIBERNATE_NULLABLES
            else if (valueType is NHibernateNullableBaseType)
            {
                buf.Append(".HasValue == true");
            }
#endif
            else
            {
                buf.Append(" != null");
            }
        }

        protected virtual void CheckAutoUpdateMethod(MethodInfo mi)
        {
            var parameterTypes = GetArgTypes(mi);
            if (parameterTypes.Length != 1
                || !IsBeanTypeAssignable(parameterTypes[0])
                && !parameterTypes[0].IsAssignableFrom(typeof(IList))
                && !parameterTypes[0].IsArray)
            {
                throw new IllegalSignatureRuntimeException("EDAO0006", mi.ToString());
            }
        }

        protected virtual bool IsSelect(MethodInfo mi)
        {
            if (IsInsert(mi.Name)) return false;
            if (IsUpdate(mi.Name)) return false;
            if (IsDelete(mi.Name)) return false;
            return true;
        }

        protected virtual bool IsInsert(string methodName) => insertPrefixes.Any(methodName.StartsWith);

        protected virtual bool IsUpdate(string methodName) => updatePrefixes.Any(methodName.StartsWith);

        protected virtual bool IsDelete(string methodName) => deletePrefixes.Any(methodName.StartsWith);

        protected virtual bool IsUnlessNull(string methodName) => unlessNullSuffixes.Any(methodName.EndsWith);

        protected virtual bool IsModifiedOnly(string methodName) => modifiedOnlySuffixes.Any(methodName.EndsWith);

        protected virtual string[] GetArgNames(MethodInfo mi) => MethodUtil.GetParameterNames(mi);

        protected virtual Type[] GetArgTypes(MethodInfo mi) => MethodUtil.GetParameterTypes(mi);

        #region IDaoMetaData メンバ

        public Type BeanType => beanType;

        public IBeanMetaData BeanMetaData => beanMetaData;

        protected bool CompletedSetupMethod(MethodInfo mi) => HasSqlCommand(mi.Name);

        public virtual bool HasSqlCommand(string methodName) => sqlCommands.Contains(methodName);

        public virtual ISqlCommand GetSqlCommand(string methodName)
        {
            var cmd = (ISqlCommand)sqlCommands[methodName];
            if (cmd == null)
                throw new MethodNotFoundRuntimeException(daoType, methodName, null);
            return cmd;
        }

        public virtual ISqlCommand CreateFindCommand(string query)
        {
            return CreateSelectDynamicCommand(new BeanListMetaDataDataReaderHandler(
                beanMetaData, CreateRowCreator(), CreateRelationRowCreator()), query);
        }

        public virtual ISqlCommand CreateFindArrayCommand(string query)
        {
            return CreateSelectDynamicCommand(new BeanArrayMetaDataDataReaderHandler(
                beanMetaData, CreateRowCreator(), CreateRelationRowCreator()), query);
        }

        public virtual ISqlCommand CreateFindBeanCommand(string query)
        {
            return CreateSelectDynamicCommand(new BeanMetaDataDataReaderHandler(
                beanMetaData, CreateRowCreator(), CreateRelationRowCreator()), query);
        }

        public virtual ISqlCommand CreateFindObjectCommand(string query)
        {
            return CreateSelectDynamicCommand(new ObjectDataReaderHandler(), query);
        }

        protected virtual IRowCreator CreateRowCreator() => new RowCreatorImpl();

        protected virtual IRelationRowCreator CreateRelationRowCreator() => new RelationRowCreatorImpl();

        #endregion

        public static Type GetDaoInterface(Type type)
        {
            if (type.IsInterface)
            {
                return type;
            }
            throw new DaoNotFoundRuntimeException(type);
        }

        public IDbms Dbms
        {
            set { dbms = value; }
        }

        public IDataSource DataSource
        {
            set { dataSource = value; }
        }

        public IDataReaderFactory DataReaderFactory
        {
            set { dataReaderFactory = value; }
        }

        public IDataReaderHandlerFactory DataReaderHandlerFactory
        {
            set { dataReaderHandlerFactory = value; }
        }

        public ICommandFactory CommandFactory
        {
            set { commandFactory = value; }
        }

        public string[] InsertPrefixes
        {
            set { insertPrefixes = value; }
        }

        public string[] UpdatePrefixes
        {
            set { updatePrefixes = value; }
        }

        public string[] DeletePrefixes
        {
            set { deletePrefixes = value; }
        }

        public string SqlFileEncoding
        {
            get { return sqlFileEncoding; }
            set { sqlFileEncoding = value; }
        }

        public Type DaoType
        {
            get { return daoType; }
            set { daoType = value; }
        }

        public IDatabaseMetaData DatabaseMetaData
        {
            set { dbMetaData = value; }
        }

        /// <summary>
        /// プロシージャの組み立て
        /// </summary>
        /// <param name="mi">メソッド情報</param>
        /// <param name="sql">ストアドプロシージャ名</param>
        protected virtual void SetupProcedure(MethodInfo mi, string sql)
        {
            var cmd = new ProcedureDynamicCommand(dataSource, commandFactory);
            cmd.Sql = sql;
            cmd.ArgNames = GetArgNames(mi);
            cmd.ArgTypes = GetArgTypes(mi);
            cmd.ArgDirections = AbstractProcedureHandler.GetParameterDirections(mi);
            cmd.ReturnType = mi.ReturnType;

            sqlCommands[mi.Name] = cmd;
        }
    }
}
