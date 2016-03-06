using System.Collections.Generic;
using Quill.SampleLib.Dao;
using Quill.SampleLib.Entity;

namespace Quill.Sample.App_Code.BuisinessObject {
    /// <summary>
    /// Empデータアクセス用ビジネスオブジェクト
    /// </summary>
    public static class EmpBO {
               
        /// <summary>
        /// 検索実行
        /// </summary>
        /// <returns></returns>
        public static List<Employ> Select() {
            return GetDao().Select();
        }

        /// <summary>
        /// 更新実行
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="job"></param>
        public static void Update(string id, string name, string job) {
            GetDao().Update(id, name, job);
        }

        /// <summary>
        /// 挿入実行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="job"></param>
        public static void Insert(string name, string job) {
            GetDao().Insert(name, job);
            
        }

        /// <summary>
        /// 削除実行
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(string id) {
            GetDao().Delete(id);
        }

        /// <summary>
        /// Dao取得
        /// </summary>
        /// <returns></returns>
        private static IEmpDao GetDao() {
            return QuillManager.Container.GetComponent<IEmpDao>(withInjection: true);
        }
    }
}
