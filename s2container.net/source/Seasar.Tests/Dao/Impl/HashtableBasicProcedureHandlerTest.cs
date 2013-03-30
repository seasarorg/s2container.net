using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Seasar.Extension.Unit;
using Seasar.Dao.Impl;
using System.Data;
using System.Collections;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class HashtableBasicProcedureHandlerTest : S2TestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        /// <summary>
        /// outパラメータの取得テスト
        /// </summary>
        [Test, S2]
        public void TestExecuteOutputParam()
        {
            // ## Arrange ##
            const int TEST_VALUE = 99;
            HashtableBasicProcedureHandler handler = new HashtableBasicProcedureHandler(DataSource, CommandFactory, "dbo.SelectForOutputParamMulti");
            Object[] arguments = { 100, "SALESMAN", TEST_VALUE };
            handler.ArgumentTypes = new Type[] { typeof(int), typeof(string), typeof(int) };
            handler.ArgumentNames = new string[] { "Mgr", "Job", "TestValue" };
            handler.ArgumentDirection = new ParameterDirection[] { ParameterDirection.Output, ParameterDirection.Input, ParameterDirection.Input };

            // ## Act ##
            Hashtable result = handler.Execute(arguments);

            // ## Assert ##
            Assert.AreEqual(TEST_VALUE, (int)arguments[0]);
            Assert.GreaterThan(result.Count, 0);
        }

        /// <summary>
        /// outパラメータの取得テスト(nullが渡される)
        /// </summary>
        [Test, S2]
        public void TestExecuteOutputParamNull()
        {
            // ## Arrange ##
            HashtableBasicProcedureHandler handler = new HashtableBasicProcedureHandler(DataSource, CommandFactory, "dbo.SelectForOutputParamMulti");
            Object[] arguments = { 100, "SALESMAN", DBNull.Value };
            handler.ArgumentTypes = new Type[] { typeof(int), typeof(string), typeof(int) };
            handler.ArgumentNames = new string[] { "Mgr", "Job", "TestValue" };
            handler.ArgumentDirection = new ParameterDirection[] { ParameterDirection.Output, ParameterDirection.Input, ParameterDirection.Input };

            // ## Act ##
            Hashtable result = handler.Execute(arguments);

            // ## Assert ##
            Assert.AreEqual(0, (int)arguments[0]);
            Assert.GreaterThan(result.Count, 0);
        }
    }
}
