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

using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using MbUnit.Framework;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Util;

namespace Seasar.Tests.Extension.ADO.Impl
{
    [TestFixture]
    public class DatabaseMetaDataImplTest : S2TestCase
    {
        private const string PATH = "Seasar.Tests.Ado.dicon";
        private const string DATASET_PATH = "Seasar.Tests.Extension.ADO.Impl.S2DotNetDemoDataSet";

        static DatabaseMetaDataImplTest()
        {
            var info = new FileInfo(SystemInfo.AssemblyFileName(
                Assembly.GetExecutingAssembly()) + ".config");
            XmlConfigurator.Configure(LogManager.GetRepository(), info);
        }

        public void SetUpGetPrimaryKeySetForDatabase()
        {
            Include(PATH);
        }

        [Test, S2]
        public void TestGetPrimaryKeySetForDatabase()
        {
            var dmd = new DatabaseMetaDataImpl(DataSource);
            var primaryKeySet = dmd.GetPrimaryKeySet("EMP");

            Assert.AreEqual(1, primaryKeySet.Count);
            Assert.AreEqual("EMPNO", primaryKeySet[0] as string);
        }

        public void SetUpGetColumnSetForDatabase()
        {
            Include(PATH);
        }

        [Test, S2]
        public void TestGetColumnSetForDatabase()
        {
            var dmd = new DatabaseMetaDataImpl(DataSource);
            var columSet = dmd.GetColumnSet("EMP");

            Assert.AreEqual(9, columSet.Count);
            Assert.IsTrue(columSet.Contains("EMPNO"));
            Assert.IsTrue(columSet.Contains("ENAME"));
            Assert.IsTrue(columSet.Contains("JOB"));
            Assert.IsTrue(columSet.Contains("MGR"));
            Assert.IsTrue(columSet.Contains("HIREDATE"));
            Assert.IsTrue(columSet.Contains("SAL"));
            Assert.IsTrue(columSet.Contains("COMM"));
            Assert.IsTrue(columSet.Contains("DEPTNO"));
            Assert.IsTrue(columSet.Contains("TSTAMP"));
        }

        public void SetUpGetAutoIncrementColumnSetForDatabase()
        {
            Include(PATH);
        }

        [Test, S2]
        public void TestGetAutoIncrementColumnSetForDatabase()
        {
            if (DataSource.GetCommand().GetExType().Name.Equals("SqlCommand"))
            {
                var dmd = new DatabaseMetaDataImpl(DataSource);
                var autoIncrementColumSet = dmd.GetAutoIncrementColumnSet("IDTABLE");

                Assert.AreEqual(1, autoIncrementColumSet.Count);
                Assert.IsTrue(autoIncrementColumSet.Contains("ID"));
            }
        }

        public void SetUpGetPrimaryKeySetForDataSet()
        {
            Include(PATH);
        }

        [Test, S2]
        public void TestGetPrimaryKeySetForDataSet()
        {
            var dmd = new DatabaseMetaDataImpl(null) {MetaDataSetClassName = DATASET_PATH};
            var primaryKeySet = dmd.GetPrimaryKeySet("EMP");

            Assert.AreEqual(1, primaryKeySet.Count);
            Assert.AreEqual("EMPNO", primaryKeySet[0] as string);
        }

        public void SetUpGetColumnSetForDataSet()
        {
            Include(PATH);
        }

        [Test, S2]
        public void TestGetColumnSetForDataSet()
        {
            var dmd = new DatabaseMetaDataImpl(null) {MetaDataSetClassName = DATASET_PATH};
            var columSet = dmd.GetColumnSet("EMP");

            Assert.AreEqual(9, columSet.Count);
            Assert.IsTrue(columSet.Contains("EMPNO"));
            Assert.IsTrue(columSet.Contains("ENAME"));
            Assert.IsTrue(columSet.Contains("JOB"));
            Assert.IsTrue(columSet.Contains("MGR"));
            Assert.IsTrue(columSet.Contains("HIREDATE"));
            Assert.IsTrue(columSet.Contains("SAL"));
            Assert.IsTrue(columSet.Contains("COMM"));
            Assert.IsTrue(columSet.Contains("DEPTNO"));
            Assert.IsTrue(columSet.Contains("TSTAMP"));
        }

        public void SetUpGetAutoIncrementColumnSetForDataSet()
        {
            Include(PATH);
        }

        [Test, S2]
        public void TestGetAutoIncrementColumnSetForDataSet()
        {
            var dmd = new DatabaseMetaDataImpl(null) {MetaDataSetClassName = DATASET_PATH};
            var autoIncrementColumSet = dmd.GetAutoIncrementColumnSet("IDTABLE");

            Assert.AreEqual(1, autoIncrementColumSet.Count);
            Assert.IsTrue(autoIncrementColumSet.Contains("ID"));
        }
    }
}
