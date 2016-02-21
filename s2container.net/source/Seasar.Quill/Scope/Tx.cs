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
        public static void Execute(Action<IDbTransaction> action) {
            QScope<TransactionDecorator, IDbTransaction>.Execute(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RETURN_TYPE"></typeparam>
        /// <param name="func"></param>
        public static RETURN_TYPE Execute<RETURN_TYPE>(Func<IDbTransaction, RETURN_TYPE> func) {
            return QScope<TransactionDecorator, IDbTransaction>.Execute(func);
        }
    }
}
