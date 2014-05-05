using System.Reflection;

namespace Seasar.Quill.AOP
{
    /// <summary>
    /// InterceptorからInterceptされているメソッドの情報にアクセスするためのインターフェース
    /// </summary>
    /// <remarks>AOPアライアンス準拠</remarks>
    /// <seealso href="http://aopalliance.sourceforge.net/doc/index.html">AOP Alliance</seealso>
    public interface IMethodInvocation
    {
        /// <summary>
        /// Interceptされるメソッドの情報
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
        /// 他にチェーンされているInterceptorがあればそのInterceptorを呼び出す（再帰呼び出し）
        /// なければInterceptされているメソッドを実行
        /// </summary>
        /// <returns></returns>
        object Proceed();
    }
}
