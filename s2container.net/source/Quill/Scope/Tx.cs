using System;
using System.Collections.Generic;
using System.Data;
using Quill.Scope.Impl;

namespace Quill.Scope {
    /// <summary>
    /// トランザクション用Quillスコープ修飾静的クラス
    /// </summary>
    public static class Tx {
        #region Execute ======================================================================

        /// <summary>
        /// トランザクション開始～終了実行
        /// </summary>
        /// <param name="action">トランザクション管理する処理</param>
        /// <param name="args">修飾処理内で引き継ぐ情報</param>
        public static void Execute(Action<IDbTransaction> action,
            IDictionary<string, object> args = null) {
            QScope<TransactionDecorator, IDbTransaction>.Execute(action, args);
        }

        /// <summary>
        /// トランザクション開始～終了実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE">トランザクション管理する処理の戻り値型</typeparam>
        /// <param name="func">トランザクション管理する処理</param>
        /// <param name="args">修飾処理内で引き継ぐ情報</param>
        public static RETURN_TYPE Execute<RETURN_TYPE>(Func<IDbTransaction, RETURN_TYPE> func,
            IDictionary<string, object> args = null) {
            return QScope<TransactionDecorator, IDbTransaction>.Execute(func, args);
        }

        #endregion

        #region ExecuteWith ====================================================================

        /// <summary>
        /// トランザクション開始～終了実行
        /// </summary>
        /// <typeparam name="DECORATOR_TYPE">トランザクション処理の前後に実行する修飾クラス</typeparam>
        /// <param name="action">トランザクション管理する処理</param>
        /// <param name="args">修飾処理内で引き継ぐ情報</param>
        public static void ExecuteWith<DECORATOR_TYPE>(Action<IDbTransaction> action,
            IDictionary<string, object> args = null) 
            where DECORATOR_TYPE : class, IQuillDecorator{

            QScope<DECORATOR_TYPE>.Execute(() => Execute(action), args);
        }

        /// <summary>
        /// トランザクション開始～終了実行
        /// </summary>
        /// <typeparam name="DECORATOR_TYPE">トランザクション処理の前後に実行する修飾クラス</typeparam>
        /// <typeparam name="RETURN_TYPE">トランザクション管理する処理の戻り値型</typeparam>
        /// <param name="func">トランザクション管理する処理</param>
        /// <param name="args">修飾処理内で引き継ぐ情報</param>
        public static RETURN_TYPE ExecuteWith<DECORATOR_TYPE, RETURN_TYPE>(Func<IDbTransaction, RETURN_TYPE> func,
            IDictionary<string, object> args = null)
            where DECORATOR_TYPE : class, IQuillDecorator {

            return QScope<DECORATOR_TYPE>.Execute(() => Execute(func), args);
        }

        #endregion
    }
}
