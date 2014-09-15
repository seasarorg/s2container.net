using System;

namespace Seasar.Quill.Container
{
    /// <summary>
    /// インスタンス管理インターフェース
    /// </summary>
    public interface IInstanceManager
    {
        /// <summary>
        /// コンポーネント生成オブジェクトの設定
        /// </summary>
        /// <param name="creator">コンポーネント生成オブジェクト</param>
        void SetComponentCreator(params IComponentCreater[] creator);

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="t">コンポーネントの型</param>
        /// <returns>インスタンス</returns>
        object GetInstance(Type t);

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <param name="i">コンポーネントのインターフェース（スーパークラス）</param>
        /// <param name="impl">コンポーネントの型</param>
        /// <returns>インスタンス</returns>
        object GetInstance(Type i, Type impl);
    }
}
