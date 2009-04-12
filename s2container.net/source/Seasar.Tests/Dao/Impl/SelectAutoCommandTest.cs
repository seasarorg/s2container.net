using System;
using System.Collections;
using System.Text;
using MbUnit.Framework;
using System.Data;
using Seasar.Extension.Unit;
using Seasar.Dao.Unit;
using System.Diagnostics;
using log4net;
using Seasar.Dao;

namespace Seasar.Tests.Dao.Impl
{
    [TestFixture]
    public class SelectAutoCommandTest : S2DaoTestCase
    {
        public void SetUp()
        {
            Include("Seasar.Tests.Dao.Dao.dicon");
        }

        [Test, S2]
        public void TestExecute_QueryAnnotationAndReturnDataTable()
        {
            const int EMP_NO = 7499;
            const int DEPT_NO = 30;

            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDataSetDao));
            ISqlCommand cmd = dmd.GetSqlCommand("SelectDataTable");

            object ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret is EmployeeDataSet.EmpAndDeptDataTable, ret.GetType().Name);
            DataTable actualTable = (DataTable)ret;
            Assert.AreEqual(1, actualTable.Rows.Count);
            DataRow actualRow = actualTable.Rows[0];
            Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);
            Assert.AreEqual("ALLEN", actualRow["ENAME"]);
            Assert.AreEqual(DEPT_NO, actualRow["DEPTNO"]);
            Assert.AreEqual("SALES", actualRow["DNAME"]);
        }

        [Test, S2]
        public void TestExecute_QueryAnnotationAndReturnDataSet()
        {
            const int EMP_NO = 7499;
            const int DEPT_NO = 30;

            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDataSetDao));
            ISqlCommand cmd = dmd.GetSqlCommand("SelectDataSet");

            object ret = cmd.Execute(new object[] { EMP_NO });
            Assert.IsNotNull(ret, "1");
            Assert.IsTrue(ret is EmployeeDataSet);
            DataSet actualSet = (DataSet)ret;
            Assert.AreEqual(1, actualSet.Tables.Count);
            DataTable actualTable = actualSet.Tables[0];
            Assert.IsTrue(actualTable is EmployeeDataSet.EmpAndDeptDataTable);
            Assert.AreEqual(1, actualTable.Rows.Count);
            DataRow actualRow = actualTable.Rows[0];
            Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);
            Assert.AreEqual("ALLEN", actualRow["ENAME"]);
            Assert.AreEqual(DEPT_NO, actualRow["DEPTNO"]);
            Assert.AreEqual("SALES", actualRow["DNAME"]);
        }
    }
}
