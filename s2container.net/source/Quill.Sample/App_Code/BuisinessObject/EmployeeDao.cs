using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quill.Sample.App_Code.Entity;
using Quill.Scope;
using Quill.Ado;

namespace Quill.Sample.App_Code.BuisinessObject {
    public class EmployeeDao {
        private const string SQL_SELECT = "SELECT EMPNO,ENAME,JOB as Job FROM dbo.EMP ORDER BY EMPNO";
        private const string SQL_UPDATE = "UPDATE dbo.EMP SET ENAME = /* Name */'aiueo' , JOB = /* Job */'tester' WHERE EMPNO = /* Id */7935";
        private const string SQL_INSERT = "INSERT INTO dbo.EMP (EMPNO,ENAME,JOB) VALUES((SELECT MAX(EMPNO) FROM dbo.EMP) + 1, /* Name */'aaa' , /* Job */'bbb' )";

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <returns></returns>
        public static List<Employ> Select() {
            try {
                return Tx.Execute((tx) => {
                    return tx.Select<Employ>(SQL_SELECT);
                });
            } catch(System.Exception ex) {
                Employ emp = new Employ();
                emp.Id = 100;
                emp.Name = ex.Message;
                emp.Job = ex.StackTrace;
                List<Employ> results = new List<Employ>();
                results.Add(emp);
                return results;
            }
        }

        public static void Update(string id, string name, string job) {
            var parameters = new Dictionary<string, object>();
            parameters["Id"] = id;
            parameters["Name"] = name;
            parameters["job"] = job;

            Tx.Execute(connection => {
                connection.Update(SQL_UPDATE, (no, pname, dbParam) => {
                    dbParam.Value = parameters[pname];
                });
            });
        }

        public static void Insert(string name, string job) {
            var parameters = new Dictionary<string, object>();
            parameters["Name"] = name;
            parameters["job"] = job;

            Tx.Execute(connection => {
                connection.Update(SQL_INSERT, (no, pname, dbParam) => {
                    dbParam.Value = parameters[pname];
                });
            });
        }
    }
}
