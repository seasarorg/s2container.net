using System.Collections.Generic;
using Quill.Ado;
using Quill.Sample.App_Code.Entity;
using Quill.Scope;

namespace Quill.Sample.App_Code.BuisinessObject {
    /// <summary>
    /// Employeeデータアクセスクラス
    /// </summary>
    public static class EmployeeDao {
        private const string SQL_SELECT = "SELECT EMPNO,ENAME,JOB as Job FROM dbo.EMP ORDER BY EMPNO";
        private const string SQL_UPDATE = "UPDATE dbo.EMP SET ENAME = /* Name */'aiueo' , JOB = /* Job */'tester' WHERE EMPNO = /* Id */7935";
        private const string SQL_INSERT = "INSERT INTO dbo.EMP (EMPNO,ENAME,JOB) VALUES((SELECT MAX(EMPNO) FROM dbo.EMP) + 1, /* Name */'aaa' , /* Job */'bbb' )";
        private const string SQL_DELETE = "DELETE FROM dbo.EMP WHERE EMPNO = /* Id */100";

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <returns></returns>
        public static List<Employ> Select() {
            return Tx.Execute((tx) => {
                return tx.Select<Employ>(SQL_SELECT);
            });
        }

        /// <summary>
        /// 更新実行
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="job"></param>
        public static void Update(string id, string name, string job) {
            var parameters = new Dictionary<string, object>();
            parameters["Id"] = id;
            parameters["Name"] = name;
            parameters["Job"] = job;

            Tx.Execute(connection => {
                connection.Update(SQL_UPDATE, (no, pname, dbParam) => {
                    dbParam.Value = parameters[pname];
                });
            });
        }

        /// <summary>
        /// 挿入実行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="job"></param>
        public static void Insert(string name, string job) {
            var parameters = new Dictionary<string, object>();
            parameters["Name"] = name;
            parameters["Job"] = job;

            Tx.Execute(connection => {
                connection.Update(SQL_INSERT, (no, pname, dbParam) => {
                    dbParam.Value = parameters[pname];
                });
            });
        }

        /// <summary>
        /// 削除実行
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(string id) {
            var parameters = new Dictionary<string, object>();
            parameters["Id"] = id;

            Tx.Execute(connection => {
                connection.Update(SQL_DELETE, (no, pname, dbParam) => {
                    dbParam.Value = parameters[pname];
                });
            });
        }
    }
}
