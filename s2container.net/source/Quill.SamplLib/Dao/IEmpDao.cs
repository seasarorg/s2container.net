using System.Collections.Generic;
using Quill.SampleLib.Entity;

namespace Quill.SampleLib.Dao {
    /// <summary>
    /// Empデータアクセスオブジェクト
    /// </summary>
    public interface IEmpDao {
        /// <summary>
        /// 検索実行
        /// </summary>
        /// <returns></returns>
        List<Employ> Select();

        /// <summary>
        /// 更新実行
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="job"></param>
        void Update(string id, string name, string job);

        /// <summary>
        /// 挿入実行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="job"></param>
        void Insert(string name, string job);

        /// <summary>
        /// 削除実行
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id);
    }
}
