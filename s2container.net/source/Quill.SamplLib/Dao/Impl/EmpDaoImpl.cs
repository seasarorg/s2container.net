using System;
using System.Collections.Generic;
using Quill.Ado;
using Quill.SampleLib.Entity;
using Quill.SamplLib.Decorator;
using Quill.Scope;

namespace Quill.SampleLib.Dao.Impl {
    /// <summary>
    /// Empデータアクセスオブジェクト実装クラス
    /// QScope使用例
    /// </summary>
    public class EmpDaoImpl : IEmpDao {
        private const string SQL_SELECT = "SELECT EMPNO,ENAME,JOB as Job FROM dbo.EMP ORDER BY EMPNO";
        private const string SQL_UPDATE = "UPDATE dbo.EMP SET ENAME = /* Name */'aiueo' , JOB = /* Job */'tester' WHERE EMPNO = /* Id */7935";
        private const string SQL_INSERT = "INSERT INTO dbo.EMP (EMPNO,ENAME,JOB) VALUES((SELECT MAX(EMPNO) FROM dbo.EMP) + 1, /* Name */'aaa' , /* Job */'bbb' )";
        private static readonly string SQL_DELETE =
            "DELETE" + Environment.NewLine + "FROM dbo.EMP" + Environment.NewLine + "WHERE EMPNO = /* Id */100";

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <returns></returns>
        public virtual List<Employ> Select() {
            // トランザクション処理単品で実行（戻り値あり）
            return Tx.Execute(tx => tx.Select<Employ>(SQL_SELECT));
        }

        /// <summary>
        /// 更新実行
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="job"></param>
        public virtual void Update(string id, string name, string job) {
            // トランザクション処理単品で実行（戻り値なし）
            var parameters = new Dictionary<string, object>();
            parameters["Id"] = id;
            parameters["Name"] = name;
            parameters["Job"] = job;

            Tx.Execute(tx => tx.Update(SQL_UPDATE, parameters));
        }

        /// <summary>
        /// 挿入実行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="job"></param>
        public virtual void Insert(string name, string job) {
            var parameters = new Dictionary<string, object>();
            parameters["Name"] = name;
            parameters["Job"] = job;

            // 他のDecoratorを適用しつつトランザクション処理実行１
            QScope<LogDecorator>.Execute(() => {
                Tx.Execute(tx => tx.Update(SQL_INSERT, parameters));
            });
        }

        /// <summary>
        /// 削除実行
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(string id) {
            var parameters = new Dictionary<string, object>();
            parameters["Id"] = id;

            // 他のDecoratorを適用しつつトランザクション処理実行２
            Tx.ExecuteWith<LogDecorator>(tx => tx.Update(SQL_DELETE, parameters));
        }
    }
}
