
namespace Seasar.Quill.AOP
{
    /// <summary>
    /// メソッドに対するInterceptorのインターフェース
    /// </summary>
    /// <remarks>AOPアライアンス準拠</remarks>
    /// <seealso href="http://aopalliance.sourceforge.net/doc/index.html">AOP Alliance</seealso>
    public interface IMethodInterceptor
    {
        /// <summary>
        /// メソッドがインターセプターされる場合、このメソッドが呼び出される
        /// </summary>
        /// <param name="invocation">Interceptされているメソッド情報</param>
        /// <returns>Interceptされるメソッドの戻り値</returns>
        object Invoke(IMethodInvocation invocation);
    }
}
