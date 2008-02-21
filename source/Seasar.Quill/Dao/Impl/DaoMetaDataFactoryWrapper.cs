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
using Seasar.Dao;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Quill.Attrs;

namespace Seasar.Quill.Dao.Impl
{
    /// <summary>
    /// 外部からDaoMetaDataFactoryの設定を行えるように
    /// 拡張したDaoMetaDataFactory
    /// </summary>
    [Implementation]
    public class DaoMetaDataFactoryWrapper : IDaoMetaDataFactoryWrapper
    {
        /// <summary>
        /// 本家のDaoMetaDataFactoryに処理を委譲するためのインスタンス変数
        /// </summary>
        protected IDaoMetaDataFactory _daoMetaDataFactory = null;

        private IDataSource _dataSource;
        public IDataSource DataSource
        {
            set { _dataSource = value; }
            get { return _dataSource; }
        }

        private IDataReaderFactory _dataReaderFactory;
        public IDataReaderFactory DataReaderFactory
        {
            set { _dataReaderFactory = value; }
            get { return _dataReaderFactory; }
        }

        private ICommandFactory _commandFactory;
        public ICommandFactory CommandFactory
        {
            set { _commandFactory = value; }
            get { return _commandFactory; }
        }

        private IAnnotationReaderFactory _annotationReaderFactory;
        public IAnnotationReaderFactory AnnotationReaderFactory
        {
            set { _annotationReaderFactory = value; }
            get { return _annotationReaderFactory; }
        }

        /// <summary>
        /// コンストラクタ引数なしでもインスタンス生成できるように
        /// するためのコンストラクタ
        /// </summary>
        public DaoMetaDataFactoryWrapper()
        {
        }

        #region IDaoMetaDataFactory メンバ

        public IDaoMetaData GetDaoMetaData(Type daoType)
        {
            if (_daoMetaDataFactory == null)
            {
                _daoMetaDataFactory = CreateDaoMetaDataFactory();
            }
            return _daoMetaDataFactory.GetDaoMetaData(daoType);
        }

        #endregion

        /// <summary>
        /// 処理を委譲するDaoMetaDataFactoryの生成
        /// </summary>
        /// <returns></returns>
        protected virtual IDaoMetaDataFactory CreateDaoMetaDataFactory()
        {
            return new DaoMetaDataFactoryImpl(
                DataSource, CommandFactory, AnnotationReaderFactory, DataReaderFactory);
        }
    }
}
