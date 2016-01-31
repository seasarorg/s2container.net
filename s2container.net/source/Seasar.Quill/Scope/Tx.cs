using System;
using System.Data;
using Quill.Scope.Impl;

namespace Quill.Scope {
    /// <summary>
    /// 
    /// </summary>
    public static class Tx {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public static void Execute(Action<IDbConnection> action) {
            QScope<TransactionDecorator, IDbConnection>.Execute(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <param name="func"></param>
        public static RETURN_TYPE Execute<RETURN_TYPE>(Func<IDbConnection, RETURN_TYPE> func) {
            return QScope<TransactionDecorator, IDbConnection>.Execute(func);
        }
    }
}
