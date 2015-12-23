using System.Reflection;

namespace Quill.Aop {
    /// <summary>
    /// Interceptorからインターセプトされているメソッドの情報にアクセスするためのインターフェイス
    /// </summary>
    /// <remarks>
    /// このインターフェイスはAOPアライアンス準拠。
    /// </remarks>
    /// <seealso href="http://aopalliance.sourceforge.net/doc/index.html">AOP Alliance</seealso>
    public interface IMethodInvocation {
        /// <summary>
        /// InterceptされるメソッドのMethodBase
        /// </summary>
        MethodBase Method { get; }

        /// <summary>
        /// Interceptされるオブジェクト
        /// </summary>
        object Target { get; }

        /// <summary>
        /// Interceptされるメソッドの引数
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// 他にチェーンされているInterceptorがあれば、Interceptorを呼び出します（再帰的に呼び出される）
        /// 他にチェーンされているInterceptorが無ければ、Interceptされているメソッドを実行します
        /// </summary>
        /// <returns>Interceptされたメソッドの戻り値</returns>
        object Proceed();
    }
}
