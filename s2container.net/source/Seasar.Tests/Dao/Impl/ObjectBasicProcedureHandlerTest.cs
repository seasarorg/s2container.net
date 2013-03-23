using System;
using MbUnit.Framework;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;
using System.Data;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class ObjectBasicProcedureHandlerTest : S2DaoTestCase
    {
        [SetUp]
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
            const int KEY = 7521;
            ObjectBasicProcedureHandler handler = new ObjectBasicProcedureHandler(DataSource, CommandFactory, "dbo.SelectForOutputParam");
            int outputParam = 0;
            Object[] arguments = { outputParam, KEY };
            handler.ArgumentTypes = new Type[] { typeof(int), typeof(int) };
            handler.ArgumentNames = new string[] { "Mgr", "Empno" };
            handler.ArgumentDirection = new ParameterDirection[] { ParameterDirection.Output, ParameterDirection.Input };

            // ## Act ##
            handler.Execute(arguments, typeof(void));

            // ## Assert ##
            Assert.AreEqual(7698, (int)arguments[0]);
            Assert.AreEqual(KEY, (int)arguments[1]);
        }

        /// <summary>
        /// outパラメータの取得テスト(nullが渡される)
        /// </summary>
        [Test, S2]
        public void TestExecuteOutputParamNull()
        {
            // ## Arrange ##
            const int KEY = 7839;
            ObjectBasicProcedureHandler handler = new ObjectBasicProcedureHandler(DataSource, CommandFactory, "dbo.SelectForOutputParam");
            int outputParam = 0;
            Object[] arguments = { outputParam, KEY }; // outパラメータにnullが設定されているEMPNO
            handler.ArgumentTypes = new Type[] { typeof(int), typeof(int) };
            handler.ArgumentNames = new string[] { "Mgr", "Empno" };
            handler.ArgumentDirection = new ParameterDirection[] { ParameterDirection.Output, ParameterDirection.Input };

            // ## Act ##
            handler.Execute(arguments, typeof(void));

            // ## Assert ##
            Assert.AreEqual(0, (int)arguments[0]);
            Assert.AreEqual(KEY, (int)arguments[1]);
        }
    }
}
