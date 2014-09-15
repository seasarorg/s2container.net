using System;

namespace Seasar.Quill.Container
{
    /// <summary>
    /// Quillコンテナインターフェース
    /// </summary>
    public interface IQuillContainer
    {
        /// <summary>
        /// インスタンス生成管理オブジェクトの設定
        /// </summary>
        /// <param name="manager">インスタンス管理オブジェクト</param>
        void SetInstanceManager(IInstanceManager manager);

        /// <summary>
        /// 実装型走査オブジェクトの設定
        /// </summary>
        /// <param name="detectors">実装型走査オブジェクト</param>
        void SetImplTypeDetector(params IImplTypeDetector[] detectors);

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <typeparam name="T">取得する型</typeparam>
        /// <returns>コンポーネントのインスタンス</returns>
        T GetComponent<T>();

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <typeparam name="IF">コンポーネントインターフェース（スーパークラス含む）</typeparam>
        /// <typeparam name="IMPL">実装クラス</typeparam>
        /// <returns>コンポーネントのインスタンス</returns>
        IF GetComponent<IF, IMPL>() where IMPL : IF;

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="t">取得する型</param>
        /// <returns>コンポーネントのインスタンス</returns>
        object GetComponent(Type t);

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="i">コンポーネントインターフェース（スーパークラス含む）</param>
        /// <param name="t">実装クラス</param>
        /// <returns>コンポーネントのインスタンス</returns>
        object GetComponent(Type i, Type t);
    }
}
