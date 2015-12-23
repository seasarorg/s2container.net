namespace Quill.Aop {
    /// <summary>
    /// メソッドに対するInterceptorのインターフェイス
    /// </summary>
    /// <remarks>
    /// このインターフェイスはAOPアライアンス準拠。
    /// </remarks>
    /// <seealso href="http://aopalliance.sourceforge.net/doc/index.html">AOP Alliance</seealso>
    public interface IMethodInterceptor {
        /// <summary>
        /// メソッドがInterceptされる場合、このメソッドが呼び出されます
        /// </summary>
        /// <param name="invocation">IMethodInvocation</param>
        /// <returns>Interceptされるメソッドの戻り値</returns>
        object Invoke(IMethodInvocation invocation);
    }
}
