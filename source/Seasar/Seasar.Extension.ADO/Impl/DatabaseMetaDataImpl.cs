#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Data.Common;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;
using Seasar.Framework.Exceptions;

namespace Seasar.Extension.ADO.Impl
{
    public class DatabaseMetaDataImpl : IDatabaseMetaData
    {
        private IDictionary primaryKeys = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

        private IDictionary columns = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

        private IDataSource dataSource;

        public DatabaseMetaDataImpl(IDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        #region IDatabaseMetaData �����o

        public System.Collections.IList GetPrimaryKeySet(string tableName)
        {
            if(!this.primaryKeys.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) primaryKeys[tableName];
        }

        public IList GetColumnSet(string tableName)
        {
            if(!this.columns.Contains(tableName)) CreateTableMetaData(tableName);
            return (IList) columns[tableName];
        }

        #endregion

        /// <summary>
        /// �e�[�u����`�����쐬����
        /// </summary>
        /// <param name="tableName">�e�[�u����</param>
        private void CreateTableMetaData(string tableName)
        {
            lock(this)
            {
                // IDbConnection���擾����
                IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
                try
                {
                    // �e�[�u����`�����擾���邽�߂�SQL���쐬����
                    string sql = "SELECT * FROM " + tableName;

                    // IDbCommand���擾����
                    IDbCommand cmd = dataSource.GetCommand(sql, cn);

                    // Transaction�̏������s��
                    DataSourceUtil.SetTransaction(dataSource, cmd);

                    // IDataAdapter���擾����
                    IDataAdapter adapter = dataSource.GetDataAdapter(cmd);

                    // �e�[�u����`�����擾����
                    DataTable[] metaDataTables = adapter.FillSchema(new DataSet(), SchemaType.Mapped);

                    if (metaDataTables.Length == 0)
                    {
                        // �e�[�u����`��񂪌�����Ȃ��ꍇ�́A���s����O�𔭐�������
                        throw new SRuntimeException("ESSR0067", new object[] { tableName });
                    }

                    // �e�[�u����`��񂩂�v���C�}���L�[���擾����
                    primaryKeys[tableName] = GetPrimaryKeySet(metaDataTables[0].PrimaryKey);

                    // �e�[�u����`��񂩂�J�������擾����
                    columns[tableName] = GetColumnSet(metaDataTables[0].Columns);
                }
                finally
                {
                    // IDbConnection��Close�������s��
                    DataSourceUtil.CloseConnection(dataSource, cn);
                }
            }
        }

        private IList GetPrimaryKeySet(DataColumn[] primarykeys)
        {
            IList list = new CaseInsentiveSet();
            foreach (DataColumn pkey in primarykeys)
            {
                list.Add(pkey.ColumnName);
            }
            return list;
        }

        private IList GetColumnSet(DataColumnCollection columns)
        {
            IList list = new CaseInsentiveSet();
            foreach (DataColumn column in columns)
            {
                list.Add(column.ColumnName);
            }
            return list;
        }
        
    }
}