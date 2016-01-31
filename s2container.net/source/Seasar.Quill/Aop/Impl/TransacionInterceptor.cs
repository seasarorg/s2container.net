
using Quill.Scope.Impl;

namespace Quill.Aop.Impl {
    /// <summary>
    /// Transactionインターセプター
    /// </summary>
    public class TransacionInterceptor : IMethodInterceptor {

        /// <summary>
        /// Transaction修飾クラス
        /// </summary>
        public TransactionDecorator Tx { get; set; }

        /// <summary>
        /// メソッドがInterceptされる場合、このメソッドが呼び出されます
        /// </summary>
        /// <param name="invocation">IMethodInvocation</param>
        /// <returns>Interceptされるメソッドの戻り値</returns>
        public object Invoke(IMethodInvocation invocation) {
            return Tx.Decorate(c => invocation.Proceed());
        }
    }
}
